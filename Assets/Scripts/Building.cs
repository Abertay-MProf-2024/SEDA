using System.Collections;
using UnityEngine;

public class Building : MonoBehaviour
{
    public TileBase resourceData;
    [HideInInspector] public TileBase oldresourceData;
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
        
        if (Terrainsystem == null)
        {
            RaycastHit hit;
            Physics.Raycast(transform.position, Vector3.down, out hit);
            SetGridObject(hit.transform.gameObject.GetComponent<GridObject>());
        }

        Terrainsystem.owningGridObject.GetOwningGridSystem().ToggleBuildMode(resourceData, true);

        UpdateTotalBuildingCount(true);
        TimeSystem.AddMonthlyEvent(Impact);
       
        if ( !resourceData.isResourceTapped )
        {
            TimeSystem.AddMonthlyEvent(HarvestSurroundingResources);
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

        if (resourceData.baseOutputEnergy)
        {
            Terrainsystem.Lenergy = true;
            Terrainsystem.radius = resourceData.impactRadiusTiles;
            Terrainsystem.TriggerEnergy();
        }

        if (resourceData.baseOutputWater)
        {
            Terrainsystem.Wenergy = true;
            Terrainsystem.Wradius = resourceData.impactRadiusTiles;
            Terrainsystem.TriggerEnergy();
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

        Inventory.food += Mathf.FloorToInt(resourceData.baseOutputFood * soilGradeModifier * WeatherSystem.cropOutput * Upkeepmet);
        Inventory.constructionMaterials += Mathf.FloorToInt(resourceData.baseOutputMaterial * WeatherSystem.cropOutput * Upkeepmet);

        if (gameObject.GetComponentInChildren<ResourceUpdatePopup>())
        {
            gameObject.GetComponentInChildren<ResourceUpdatePopup>().AnimatePopup();
        }
    }
    
    public void PayUpkeep()
    {
        if (!resourceData.upKeepCostEnergy || Terrainsystem.Lenergy)
        {
            if (!resourceData.upKeepCostWater || (WeatherSystem.isFlooding || Terrainsystem.Wenergy))
            {
                Upkeepmet = 1;
                Inventory.SpendFood(resourceData.upKeepCostFood);
                Inventory.SpendMaterials(resourceData.upKeepCostMaterial);
            }
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
        return Terrainsystem.owningGridObject;
    }    

    public void SetGridObject(GridObject gridObject)
    {
        if (gridObject.terrain == null)
        {
            gridObject.SetTerrain();
        }

        Terrainsystem = gridObject.terrain;
        Terrainsystem.owningGridObject = gridObject;
    }

    public void Impact()
    {
        GridPosition pos = GetOwningGridObject().GetGridPosition();
        int radius = resourceData.impactRadiusTiles;

        for (int x = pos.x - radius; x <= pos.x + radius; x++)
        {
            for (int z = pos.z - radius; z <= pos.z + radius; z++)
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
        resource.buff = 0;
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
            resourceData.inGameAsset = newresourceData.inGameAsset;
            resourceData = newresourceData;
            //gameObject.GetComponent<MeshRenderer>().sharedMaterial = newresourceData.inGameAsset.GetComponent<MeshRenderer>().sharedMaterial;

        }
        else
            Destroy(gameObject);
            //gameObject.SetActive(false);
    }

    public void VeilChangeDeactivate()
    {
        if (oldresourceData != null)
        {
            resourceData = oldresourceData;
            resourceData.inGameAsset = oldresourceData.inGameAsset;
            gameObject.GetComponent<MeshRenderer>().sharedMaterial = oldresourceData.inGameAsset.GetComponent<MeshRenderer>().sharedMaterial;
        }
        else
            Destroy(gameObject);
        //gameObject.SetActive(true);
    }

    public void HarvestSurroundingResources()
    {
        GridPosition pos = GetOwningGridObject().GetGridPosition();
        int radius = resourceData.impactRadiusTiles;

        for (int x = pos.x - radius; x <= pos.x + radius; x++)
        {
            for (int z = pos.z - radius; z <= pos.z + radius; z++)
            {
                if (x >= 0 && z >= 0 && x < GetOwningGridObject().GetOwningGridSystem().GetGridLength() && z < GetOwningGridObject().GetOwningGridSystem().GetGridWidth())
                {
                    // TODO: Filter by structure type
                    Building objectInRadius;
                    if ((objectInRadius = GetOwningGridObject().GetOwningGridSystem().GetGridObject(x, z).GetBuilding()) && (new GridPosition(x, z) != pos))
                    {
                        foreach(CollectorType collector in resourceData.collectorBuildings)
                        {
                            if (collector.ToString() == objectInRadius.resourceData.structureType.ToString()
                                && (!objectInRadius.resourceData.upKeepCostEnergy || objectInRadius.Terrainsystem.Lenergy)
                                && (!objectInRadius.resourceData.upKeepCostWater || (WeatherSystem.isFlooding || objectInRadius.Terrainsystem.Wenergy)))
                            {
                                UpdateResources();
                                return;
                            }
                        }    
                    }
                }
            }
        }
    }
}
