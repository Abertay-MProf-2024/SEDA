using UnityEngine;

public class GridObject : MonoBehaviour
{
    GridSystem owningGridSystem;

    public Terrainsystem terrain;
    TerrainTypes terrainType;
    public Building buildingInstance;

    private void Start()
    {
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        // Check for the terrain type under this GridObject, and set references to it
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            if (terrain = hit.transform.gameObject.GetComponent<Terrainsystem>())
            {
                terrain.owningGridObject = this;
                terrainType = terrain.terraintype;
            }
        }
        else
        {
            terrainType = TerrainTypes.None;
        }

        
    }

    public void ToggleBuildModePerTile(TileBase buildingType)
    {
        float alpha = 0.75f;

        Color transparentGreen = new Color(0, 0.3215686f, 0.07343697f, alpha);
        Color transparentOrange = new Color(0.990566f, 0.5814224f, 0, alpha);
        Color transparentBrown = new Color(0.3207547f, 0.1755072f, 0, .8f);
        Color transparentRed = new Color(0.9921568f, 0, 0.02855804f, alpha);

        if (!BuildSystem.isInBuildMode)
        {
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
        }
        else if (CanBuildOnTile(buildingType))
        {
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;

            switch (terrain.CurrentsoilType)
            {
                case Terrainsystem.SoilType.A:
                case Terrainsystem.SoilType.B:
                    gameObject.GetComponentInChildren<MeshRenderer>().material.color = transparentGreen;
                    break;
                case Terrainsystem.SoilType.C:
                case Terrainsystem.SoilType.D:
                    gameObject.GetComponentInChildren<MeshRenderer>().material.color = transparentOrange;
                    break;
                case Terrainsystem.SoilType.E:
                    gameObject.GetComponentInChildren<MeshRenderer>().material.color = transparentBrown;
                    break;
            }
        }
        else
        {
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
            gameObject.GetComponentInChildren<MeshRenderer>().material.color = transparentRed;
        }
    }

    public GridSystem GetOwningGridSystem()
    {
        return owningGridSystem;
    }

    public void SetOwningGridSystem(GridSystem system)
    {
        owningGridSystem = system;
    }

    // Instantiates a building on top of this tile.
    public bool TryBuild(TileBase building)
    {
        if (CanBuildOnTile(building))
        {
            buildingInstance = Instantiate(building.inGameAsset, transform).GetComponent<Building>();
            buildingInstance.resourceData = building;
            
            buildingInstance.transform.localPosition = Vector3.zero;
            buildingInstance.SetGridObject(this);
            if (buildingInstance.resourceData.isImpactSoilGrade == true)
            {
                terrain.ChangeinGrade(buildingInstance.resourceData.buffSoilGradeAmount, buildingInstance.resourceData.nerfSoilGradeAmount, buildingInstance.resourceData.isImpactSoilGrade);
                buildingInstance.Impact();
            }
            return true;
        }
        return false;
    }

    // Returns whether an object can be built on this GridObject
    public bool CanBuildOnTile(TileBase building)
    {
        bool canBuild = false;

        if (building == null)
        { return false; }
        
        for (int i = 0; i < building.tileTerrainTypes.Count; i++)
        {
            if (terrain && terrain.creaturetype == CreatureTypes.None)
            {
                if (terrainType == building.tileTerrainTypes[i])
                    canBuild = true;
            }
        }

        if (buildingInstance != null)
        {
            canBuild = false;
        }

        if (Inventory.food < building.buildingCostFood)
        {
            canBuild = false;
        }

        if (Inventory.constructionMaterials < building.buildingCostMaterial)
        {
            canBuild = false;
        }

        /*if(buildingInstance.RequireWaterEnergy && terrain.Wenergy == false)
        {
           canBuild = false;
        }*/

        return canBuild;
    }

    public GridPosition GetGridPosition()
    {
        return new GridPosition(transform.localPosition.x / GetOwningGridSystem().GetCellSize(), transform.localPosition.z / GetOwningGridSystem().GetCellSize());
    }

    public Terrainsystem GetTerrainType()
    {
        return terrain;
    }

    public Building GetBuilding()
    { return buildingInstance; }

    public void SetTerrainEnergy(bool hasEnergy)
    {
        if (terrain != null)
            terrain.Lenergy = hasEnergy;
    }
    public void SetTerrainWaterEnergy(bool hasEnergy)
    {
        if (terrain != null)
            terrain.Wenergy = hasEnergy;
    }


}
