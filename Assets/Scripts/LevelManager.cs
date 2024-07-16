using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
    [SerializeField] int successSoilHealth;


    GameObject outlineParent;

    // radius highlight prefabs
    [SerializeField] GameObject waterOutline;
    [SerializeField] GameObject energyOutline;
    [SerializeField] GameObject extraOutline;

    [SerializeField] GameObject musicPlayer;

    [SerializeField] BuildingCostUI buildingCostUI;

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
        Instantiate(musicPlayer);
        SceneMusic.instance.ChangeMusicTrack(sceneMusic);

        Inventory.food = startingFoodAmount;
        Inventory.constructionMaterials = startingConstructionMaterialAmount;
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

            if ((building = hit.transform.gameObject.GetComponent<Building>()) && building.resourceData.impactSource
                && building.GetOwningGridObject())
            {
                // Check BuildingClickSound function and play click sound
                BuildingClickSound clickSound = building.GetComponent<BuildingClickSound>();
                if (clickSound != null)
                {
                    clickSound.PlayClickSound();
                }

                radius = building.resourceData.impactRadiusTiles;

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
                        Vector3 worldPosition = building.GetOwningGridObject().GetOwningGridSystem().GetGridObject(x, z).transform.position;
                        Instantiate(extraOutline, worldPosition, Quaternion.identity, outlineParent.transform);
                    }
                }

                //to pop up the building details when clicked on a building
                buildingCostUI.gameObject.SetActive(true);
                buildingCostUI.BuildingNameDisplay.text = building.name.ToString();
                buildingCostUI.ReqFoodDisplay.text = building.resourceData.buildingCostFood.ToString();
                buildingCostUI.ReqConstMatDisplay.text = building.resourceData.buildingCostMaterial.ToString();

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
                                Vector3 worldPosition = terrainTile.owningGridObject.GetOwningGridSystem().GetGridObject(x, z).transform.position;
                                Instantiate(energyOutline, worldPosition, Quaternion.identity, outlineParent.transform);
                            }
                        }
                    }
                }
                if (terrainTile.Wradius != 0)
                {
                    if (terrainTile.Wenergy)
                    {
                        radius = terrainTile.Wradius;

                        GridPosition gridPos = terrainTile.owningGridObject.GetGridPosition();

                        outlineParent = new GameObject();
                        outlineParent.name = "outlineParent";

                        for (int x = gridPos.x - radius; x <= gridPos.x + radius; x++)
                        {
                            for (int z = gridPos.z - radius; z <= gridPos.z + radius; z++)
                            {
                                Vector3 worldPosition = terrainTile.owningGridObject.GetOwningGridSystem().GetGridObject(x, z).transform.position;
                                Instantiate(waterOutline, worldPosition, Quaternion.identity, outlineParent.transform);
                            }
                        }
                    }
                }
            }
        }
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
