using UnityEngine;

public class Building : MonoBehaviour
{
    public TileBase resourceData;

    float buff;
    float nerf;

    Terrainsystem Terrainsystem;

    private void Start()
    {
        PayConstructionCosts();
        resourceData.tileUnder.GetOwningGridSystem().ToggleBuildMode(resourceData, true);
        UpdateTotalBuildingCount(true);
        //Impact();
       
        if ( !resourceData.isResourceTapped )
        {
            TimeSystem.AddMonthlyEvent(UpdateResources);
        }

        TimeSystem.AddMonthlyEvent(PayUpkeep);
    }

    public void PayConstructionCosts()
    {
        Inventory.SpendFood(resourceData.buildingCostFood);
        Inventory.SpendMaterials(resourceData.buildingCostMaterial);
    }

    /** Generate resources according to the following equation: Base Output * buffs/nerfs * total crop output level */
    public void UpdateResources()
    {
        Inventory.food += Mathf.FloorToInt(resourceData.baseOutputFood * (1 + buff + nerf) * Inventory.cropOutput);
        Inventory.constructionMaterials += Mathf.FloorToInt(resourceData.baseOutputMaterial * (1 + buff + nerf) * Inventory.cropOutput);
    }
    
    public void PayUpkeep()
    {
        if (resourceData.upKeepCostWater && (Inventory.isFlooding || resourceData.tileUnder.terrain.Wenergy))
        {
            Inventory.SpendFood(resourceData.upKeepCostFood);
            Inventory.SpendMaterials(resourceData.upKeepCostMaterial);
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
}
