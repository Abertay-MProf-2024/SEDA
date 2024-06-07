using UnityEngine;

public class Resource : PlaceableObject
{
    [SerializeField]
    TileBase resourceData;

    float buff;

    private void Start()
    {
        if ( !resourceData.isResourceTapped )
        {
            TimeSystem.AddMonthlyEvent(UpdateResources);
        }

        TimeSystem.AddMonthlyEvent(PayUpkeep);
    }

    /** Generate resources according to the following equation: Base Output * Building Level * Building Stage */
    public void UpdateResources()
    {
        Inventory.food += Mathf.FloorToInt(resourceData.baseOutputFood * resourceData.buildingLevelMulti * resourceData.buildingOutputStage * (1+buff));
        Inventory.constructionMaterials += Mathf.FloorToInt(resourceData.baseOutputConstruction * resourceData.buildingLevelMulti * resourceData.buildingOutputStage * (1+buff));
        print(Inventory.constructionMaterials);
    }
    
    public void PayUpkeep()
    {
        Inventory.SpendFood(resourceData.upKeepCostFood);
        Inventory.SpendMaterials(resourceData.upKeepCostConstruction);
    }

    public override void SetGridObject(GridObject gridObject)
    {
        base.SetGridObject(gridObject);
        resourceData.tileUnder = gridObject;
        gridObject.objectOnTile = this;
    }

    public void Impact()
    {
        GridPosition pos = owningGridObject.GridPosition;
        int radius = resourceData.impactRadiusTiles;
        
        for (int x = pos.x - radius; x <= pos.x + radius; x++)
        {
            for (int z = pos.z - radius; z <= pos.z + radius; z++)
            {
                // TODO: Filter by structure type
                Resource objectInRadius;
                if (objectInRadius = owningGridObject.GridSystem.gridObjectsArray[x, z].objectOnTile as Resource)
                {
                    objectInRadius.TransferFood(resourceData.transferFood);
                    objectInRadius.TransferMaterials(resourceData.transferConstruction);
                }
            }
        }
    }

    public void TransferFood(int food)
    {
        Inventory.food += resourceData.transferFood;
    }

    public void TransferMaterials(int materials)
    {
        Inventory.constructionMaterials += resourceData.transferConstruction;
    }

    public void SetBuffs(Resource resource)
    {
        resource.buff += resourceData.buffAmount;
        resource.buff -= resourceData.nerfAmount;
    }
}
