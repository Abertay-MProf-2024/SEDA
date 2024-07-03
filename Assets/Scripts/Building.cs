using System.Collections;
using UnityEngine;

public class Building : MonoBehaviour
{
    public TileBase resourceData;
    public TileBase oldresourceData;
    public TileBase newresourceData;

    float buff;
    float nerf;

    Terrainsystem Terrainsystem;

    int Upkeepmet = 1;

    //does the building required water energy to be able to be built on.
    public bool RequireWaterEnergy;

    //should it consider the soil grade for the output.
    public bool IsItBasedOnSoilGrade;

    float soilGradeModifier = 1f;
    
    private void Start()
    {
        oldresourceData =  resourceData;

        PayConstructionCosts();
        
        if (resourceData.tileUnder == null)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit);
            resourceData.tileUnder = hit.transform.gameObject.GetComponent<GridObject>();
        }

        resourceData.tileUnder.GetOwningGridSystem().ToggleBuildMode(resourceData, true);

        UpdateTotalBuildingCount(true);
        //Impact();
       
        if ( !resourceData.isResourceTapped )
        {
            TimeSystem.AddMonthlyEvent(UpdateResources);
        }

        TimeSystem.AddMonthlyEvent(PayUpkeep);
        StartCoroutine(FindGridObject());
        
    }

    IEnumerator FindGridObject()
    {
        yield return new WaitForSeconds(1);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            Debug.DrawLine(transform.position, transform.position + Vector3.down * 100, Color.red, 500);
            Terrainsystem = hit.transform.gameObject.GetComponent<Terrainsystem>();
            Terrainsystem.owningGridObject.buildingInstance = this;
        }

        

    }

    public void PayConstructionCosts()
    {
        Inventory.SpendFood(resourceData.buildingCostFood);
        Inventory.SpendMaterials(resourceData.buildingCostMaterial);
    }

    /** Generate resources according to the following equation: Base Output * buffs/nerfs * total crop output level */
    public void UpdateResources()
    {
        IfDependsonSoilGrade();
        if (RequireWaterEnergy && (Inventory.isFlooding || resourceData.tileUnder.terrain.Wenergy))
        {
            Inventory.food += Mathf.FloorToInt(resourceData.baseOutputFood * soilGradeModifier * (1 + buff + nerf) * Inventory.cropOutput * Upkeepmet);
            Inventory.constructionMaterials += Mathf.FloorToInt(resourceData.baseOutputMaterial * (1 + buff + nerf) * Inventory.cropOutput * Upkeepmet);
        }
    }
    
    public void PayUpkeep()
    {
        if (resourceData.upKeepCostWater && (Inventory.isFlooding || resourceData.tileUnder.terrain.Wenergy))
        {
            Upkeepmet = 1;
            Inventory.SpendFood(resourceData.upKeepCostFood);
            Inventory.SpendMaterials(resourceData.upKeepCostMaterial);
        }
        else
        {
            Upkeepmet = 0;

        }
    }

    public void IfDependsonSoilGrade()
    {
        if (IsItBasedOnSoilGrade)
        {
            soilGradeModifier = (Terrainsystem.health / 100);
        }
    }

    public GridObject GetOwningGridObject()
    {
        return resourceData.tileUnder;
    }    

    public void SetGridObject(GridObject gridObject)
    {
        resourceData.tileUnder = gridObject;
    }

    public void Impact()
    {
        GridPosition pos = GetOwningGridObject().GetGridPosition();
        int radius = resourceData.impactRadiusTiles;

        for (int x = pos.x - radius; x < pos.x + radius; x++)
        {
            for (int z = pos.z - radius; z < pos.z + radius; z++)
            {
                if (x >= 0 && z >= 0 && x < GetOwningGridObject().GetOwningGridSystem().GetGridLength() && z < GetOwningGridObject().GetOwningGridSystem().GetGridWidth())
                {
                    // TODO: Filter by structure type
                    Building objectInRadius;
                    if ((objectInRadius = GetOwningGridObject().GetOwningGridSystem().GetGridObject(x, z).GetBuilding()) && (new GridPosition(x, z) != pos))
                    {
                        SetBuffs(objectInRadius);
                    }
                }
            }
        }
    }

    public void SetBuffs(Building resource)
    {
        resource.buff += resourceData.buffAmount;
        resource.buff -= resourceData.nerfAmount;
    }

    private void UpdateTotalBuildingCount(bool buildingIsBeingCreated)
    {
        int changeAmount;

        if (buildingIsBeingCreated)
        {
            changeAmount = 1;
        }
        else
        {
            changeAmount = -1;
        }

        switch (resourceData.structureType)
        {
            case TileBase.StructureTypes.LoggingCamp:
                Inventory.numOfLoggingCamps += changeAmount;
                break;
            case TileBase.StructureTypes.Forest:
                Inventory.numOfForests += changeAmount;
                break;
            case TileBase.StructureTypes.Rock:
                Inventory.numOfRocks += changeAmount;
                break;
            case TileBase.StructureTypes.Mine:
                Inventory.numOfMines += changeAmount;
                break;
        }
    }

    private void OnDestroy()
    {
        UpdateTotalBuildingCount(false);
    }

    public void VeilChangeActivate()
    {
        if (newresourceData != null)
        {
            resourceData = newresourceData;
            resourceData.inGameAsset = newresourceData.inGameAsset;
        }
        else
            gameObject.SetActive(false);
    }

    public void VeilChangeDeactivate()
    {
        if (newresourceData != null)
        {
            resourceData = oldresourceData;
            resourceData.inGameAsset = oldresourceData.inGameAsset;
        }
        else
            gameObject.SetActive(true);
    }
}
