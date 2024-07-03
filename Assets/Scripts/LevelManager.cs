// Remy Pijuan 2024

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager : MonoBehaviour
{
    // Level Settings is a singleton
    private static LevelManager instance;

    [SerializeField] InputActionAsset inputs;

    InputAction tapAction;
    InputAction tapLocation;

    // This function reference is necessary for callback registering/deregistering to work properly
    Action<InputAction.CallbackContext> possessCamera;

    // time in months
    public int levelTimeStore;

    [SerializeField] int successFoodAmount;
    [SerializeField] int successConstructionMaterialsAmount;
    [SerializeField] int successSoilHealth;


    GameObject outlineParent;

    // radius highlight prefabs
    [SerializeField] GameObject waterOutline;
    [SerializeField] GameObject energyOutline;
    [SerializeField] GameObject extraOutline;


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
        tapAction = inputs.FindAction("PossessCamera");
        possessCamera = ctx => SelectTile();
        tapAction.performed += possessCamera;

        tapLocation = inputs.FindAction("PanCamera");
    }

    void SelectTile()
    {
        if (outlineParent)
        {
            Destroy(outlineParent);
            return;
        }

        int radius = 0;

        Ray ray = Camera.main.ScreenPointToRay(tapLocation.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Building building;
            GridObject gridTile;
            Terrainsystem terrainTile;

            if ((building = hit.transform.gameObject.GetComponent<Building>()) && building.resourceData.impactSource)
            {
                radius = building.resourceData.impactRadiusTiles;

                GridPosition gridPos = building.transform.parent.gameObject.GetComponent<GridObject>().GetGridPosition();

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
            }
            else if ((gridTile = hit.transform.gameObject.GetComponent<GridObject>()) && (terrainTile = gridTile.terrain) && gridTile.terrain.owningGridObject)
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
                else if (terrainTile.Wenergy)
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
                            Instantiate(waterOutline, worldPosition, Quaternion.identity, outlineParent.transform);
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

    private void OnDestroy()
    {
        tapAction.performed -= possessCamera;
    }
}
