using System.Collections;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    // Level Settings is a singleton
    private static LevelManager instance;

    [SerializeField] AudioClip sceneMusic;

    // time in months
    public int levelTimeStore;

    [SerializeField] int startingFoodAmount;
    [SerializeField] int startingConstructionMaterialAmount;

    [SerializeField] int successFoodAmount;
    [SerializeField] int successConstructionMaterialsAmount;
    [SerializeField] public int successSoilHealth;

    [SerializeField] TileBase RiverTileBase;
   

    GameObject outlineParent;

    [Header("VFX")]
    // radius highlight prefabs
    [SerializeField] GameObject waterOutline;
    [SerializeField] GameObject energyOutline;
    [SerializeField] GameObject extraOutline;
    [SerializeField] GameObject waterSplash;
    [SerializeField] GameObject tapDust;

    [Space(20)]
    [SerializeField] GameObject musicPlayer;

    [SerializeField] BuildingCostUI buildingCostUI;

    [Header("Time")]
    [SerializeField] Month startingMonth;
    [SerializeField] int startingDay = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Inventory.levelTime = levelTimeStore;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Inventory.foodtoComplete = successFoodAmount;
        Inventory.CMtoComplete = successConstructionMaterialsAmount;

        Instantiate(musicPlayer);
        SceneMusic.instance.ChangeMusicTrack(sceneMusic);
       
        Inventory.food = startingFoodAmount;
        Inventory.constructionMaterials = startingConstructionMaterialAmount;
        StartCoroutine(FindSoilHealth());

        if (TimeSystem.instance)
            TimeSystem.instance.ManuallySetDate(startingMonth, startingDay);

        TimeSystem.AddMonthlyEvent(this, Inventory.HealthBarChange, 1, true, 3);
        TimeSystem.AddMonthlyEvent(this, Terrainsystem.ResetValuesSoilGrade, 1, true, 4);
    }

    

    IEnumerator FindSoilHealth()
    {
        yield return new WaitForSeconds(1);
        Inventory.HealthBarChange();
        Terrainsystem.ResetValuesSoilGrade();
    }

    public void SelectTile(Ray ray)
    {
        if (outlineParent)
        {
            Destroy(outlineParent);
            return;
        }

        int radius = 0;

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Building building;
            GridObject gridTile;
            Terrainsystem terrainTile;

            if (building = hit.transform.gameObject.GetComponent<Building>())
            {
                SelectBuilding(building);
            }
            else if ((gridTile = hit.transform.gameObject.GetComponent<GridObject>()) && (terrainTile = gridTile.terrain) && gridTile.terrain.owningGridObject)
            {
                if (terrainTile.radius != 0)
                {
                    if (terrainTile.Lenergy)
                    {
                        radius = terrainTile.radius;

                        GridPosition gridPos = terrainTile.owningGridObject.GetGridPosition();

                        outlineParent = new GameObject();
                        outlineParent.name = "outlineParent";

                        for (int x = gridPos.x - radius; x <= gridPos.x + radius; x++)
                        {
                            for (int z = gridPos.z - radius; z <= gridPos.z + radius; z++)
                            {
                                GridObject currentGridObject = terrainTile.owningGridObject.GetOwningGridSystem().GetGridObject(x, z);
                                if (currentGridObject.terrain && 
                                    (currentGridObject.terrain.terraintype == TerrainTypes.Grassland || currentGridObject.terrain.terraintype == TerrainTypes.Highland)
                                    && currentGridObject.terrain.specialtype == SpecialTypes.None)
                                {
                                    Vector3 worldPosition = currentGridObject.transform.position;
                                    Instantiate(energyOutline, worldPosition, Quaternion.identity, outlineParent.transform);
                                }
                            }
                        }
                    }
                }
                else if (terrainTile.Wradius != 0)
                {
                    if (terrainTile.GetWaterEnergy())
                    {
                        radius = terrainTile.Wradius;

                        GridPosition gridPos = terrainTile.owningGridObject.GetGridPosition();

                        outlineParent = new GameObject();
                        outlineParent.name = "outlineParent";

                        for (int x = gridPos.x - radius; x <= gridPos.x + radius; x++)
                        {
                            for (int z = gridPos.z - radius; z <= gridPos.z + radius; z++)
                            {
                                GridObject currentGridObject = terrainTile.owningGridObject.GetOwningGridSystem().GetGridObject(x, z);
                                if (currentGridObject.terrain && 
                                    (currentGridObject.terrain.terraintype == TerrainTypes.Grassland || currentGridObject.terrain.terraintype == TerrainTypes.Highland)
                                    && currentGridObject.terrain.specialtype == SpecialTypes.None)
                                {
                                    Vector3 worldPosition = currentGridObject.transform.position;
                                    Instantiate(waterOutline, worldPosition, Quaternion.identity, outlineParent.transform);
                                }
                            }
                        }
                        buildingCostUI.gameObject.SetActive(true);

                        buildingCostUI.BuildingNameDisplay.text = RiverTileBase.name.ToString();
                        buildingCostUI.DescriptionDisplay.text = RiverTileBase.tileDescription.ToString();
                    }
                }
                else
                {
                    GameObject dust = Instantiate(tapDust, hit.point, Quaternion.identity, hit.transform);
                    Destroy(dust, 2);
                }
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
            {

            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, LayerMask.GetMask("Ocean")))
            {
                GameObject waterVFX = Instantiate(waterSplash, hit.point, Quaternion.identity, hit.transform);
                Destroy(waterVFX, 2);
            }
        }
    }

    public bool DestroyBuildingOutline()
    {
        if (outlineParent)
        {
            Destroy(outlineParent);
            return true;
        }
        return false;
    }

    public bool SelectBuilding(Building building)
    {
        if (outlineParent)
        {
            Destroy(outlineParent);
        }


        if (building != null && building.resourceData.impactSource
            && building.GetOwningGridObject())
        {
            BuildingClickSound clickSound;
            if (clickSound = building.GetComponent<BuildingClickSound>())
            {
                clickSound.PlayClickSound();
            }


            int radius = building.resourceData.impactRadiusTiles;

            GridPosition gridPos;
            if (building.transform.parent)
                gridPos = building.transform.parent.gameObject.GetComponent<GridObject>().GetGridPosition();
            else
            {
                gridPos = building.GetOwningGridObject().GetGridPosition();
            }

            outlineParent = new GameObject();
            outlineParent.name = "outlineParent";

            for (int x = gridPos.x - radius; x <= gridPos.x + radius; x++)
            {
                for (int z = gridPos.z - radius; z <= gridPos.z + radius; z++)
                {
                    GridObject currentGridObject = building.GetOwningGridObject().GetOwningGridSystem().GetGridObject(x, z);
                    if (currentGridObject.terrain && 
                        (currentGridObject.terrain.terraintype == TerrainTypes.Grassland
                        || currentGridObject.terrain.terraintype == TerrainTypes.Highland)
                        && currentGridObject.terrain.specialtype == SpecialTypes.None)
                    {
                        Vector3 worldPosition = currentGridObject.transform.position;
                        Instantiate(extraOutline, worldPosition, Quaternion.identity, outlineParent.transform);
                    }
                }
            }

            //to pop up the building details when clicked on a building
            buildingCostUI.gameObject.SetActive(true);
            buildingCostUI.BuildingNameDisplay.text = building.resourceData.inGameAsset.name.ToString();
            buildingCostUI.DescriptionDisplay.text = building.resourceData.tileDescription.ToString();
            //buildingCostUI.ReqFoodDisplay.text = building.resourceData.buildingCostFood.ToString();
            //buildingCostUI.ReqConstMatDisplay.text = building.resourceData.buildingCostMaterial.ToString();


            return true;
        }
        return false;
    }

    public static bool AreSuccessConditionsMet()
    {
        bool success = true;

        if (Inventory.food < instance.successFoodAmount)
        {
            success = false;
        }

        if (Inventory.constructionMaterials < instance.successConstructionMaterialsAmount)
        {
            success = false;
        }

        if (Inventory.healthBar < instance.successSoilHealth)
        {
            success = false;
        }    

        return success;
    }
}
