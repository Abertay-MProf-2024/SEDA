using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public enum TerrainTypes
{
    Grassland,
    Wetland,
    Highland,
    River,
    Barren,
    Mountain,
    Glen,
    Loch,
    Shore
}

public class Terrainsystem : MonoBehaviour
{
    public SoilType soilType;
    public enum SoilType
    {
        A = 110,
        B = 105,
        C = 100,
        D = 95,
        E = 90
    }

    public List<SoilType> allowedSoilGrade;

 
    private SoilType soiltype = new SoilType();
    public bool ResourceAffect;
    private GameObject terraintyp;

    /*enum WaterType
    {
        A = 110,
        B = 105,
        C = 100,
        D = 95,
        E = 90
    }
    enum Grade
    {
        A = 110,
        B = 105,
        C = 100,
        D = 95,
        E = 90
    }*/

    [SerializeField] public TerrainTypes terraintype;


    //if the tile has energy
    public bool energy = false;

    private GirdStatus Terrain_gridStatus;
    public GridSystemTest gridSystem;

    //the radius in which it gives off energy
    public int radius;

    //the total health of the soil (A to E grade)
    int health;

    //reference to Gridsystem GRID


    private void Start()
    {
        GridPosition pos = gridSystem.GetGridPosition(transform.position);
        Terrain_gridStatus = gridSystem.GetGridSystem().GetGridGameObjectsArray()[pos.x + gridSystem.GridWidth, pos.z + gridSystem.GridLength];

        InitialTerrainList();

        BuildOnLand();
    }

    private void TriggerEnergy()
    {
        //if the terrain has energy being emitted, then set all the terraintiles' energy bool true.
        if (energy)
        {
            for (int x = -(radius); x != radius; x++)
            {
                for (int y = -radius; y != radius; y++)
                {
                    Debug.Log(radius);
                    //Tile[x,y].energy = true;
                }
            }

        }
    }

    void InitialTerrainList()
    {
        switch (terraintype)
        {
            case TerrainTypes.Grassland:
                {
                    allowedSoilGrade.Add(SoilType.A);
                    allowedSoilGrade.Add(SoilType.B);
                    allowedSoilGrade.Add(SoilType.C);
                    break;
                }

            case TerrainTypes.Highland:
                {
                    allowedSoilGrade.Add(SoilType.A);
                    allowedSoilGrade.Add(SoilType.B);
                    allowedSoilGrade.Add(SoilType.C);
                    break;
                }
            case TerrainTypes.Wetland:
                {
                    allowedSoilGrade.Add(SoilType.D);
                    allowedSoilGrade.Add(SoilType.E);
                    break;
                }
            case TerrainTypes.River:
                {
                    allowedSoilGrade.Add(SoilType.B);
                    allowedSoilGrade.Add(SoilType.C);
                    allowedSoilGrade.Add(SoilType.D);
                    break;
                }
            case TerrainTypes.Barren:
                {
                    allowedSoilGrade.Add(SoilType.E);
                    break;
                }
            case TerrainTypes.Mountain:
                {
                    allowedSoilGrade.Add(SoilType.C);
                    allowedSoilGrade.Add(SoilType.D);
                    break;
                }
        }
    }

    void ChangeinGrade()
    {
        int i = 0;
        do
        {
            if (ResourceAffect)
            {
                health = (int)soilType;

                //reference to Resource, to reduce it by (health)
                switch (health)
                {
                    case int n when (n >= 105 && n <= 110):
                        soiltype = SoilType.A;
                        Debug.Log("A grade soil");
                        break;
                    case int n when (n >= 100 && n <= 105):
                        soiltype = SoilType.B;
                        Debug.Log("B grade soil");
                        break;
                    case int n when (n >= 95 && n <= 100):
                        soiltype = SoilType.C;
                        Debug.Log("C grade soil");
                        break;
                    case int n when (n >= 90 && n <= 95):
                        soiltype = SoilType.D;
                        Debug.Log("D grade soil");
                        break;
                    case int n when (n >= 85 && n <= 90):
                        soiltype = SoilType.E;
                        Debug.Log("E grade soil");
                        break;
                }
            }
            i++;
        }
        while (i < allowedSoilGrade.Count);
    }

    void BuildOnLand()
    {
        if (terraintype == TerrainTypes.River || terraintype == TerrainTypes.Loch || terraintype == TerrainTypes.Glen)
        {
            Terrain_gridStatus.canBuild = false;
            print("Can Build? " + Terrain_gridStatus.canBuild);
        }
    }
}

/*void TileHealth()
{
    health = (int)soilType;

    //reference to Resource, to reduce it by (health)
    switch (health)
    {
        case int n when (n >= 105 && n <= 110):
            Debug.Log("A grade soil");

            break;
        case int n when (n >= 100 && n <= 105):
            Debug.Log("B grade soil");
            break;
        case int n when (n >= 95 && n <= 100):
            Debug.Log("C grade soil");
            break;
        case int n when (n >= 90 && n <= 95):
            Debug.Log("D grade soil");
            break;
        case int n when (n >= 85 && n <= 90):
            Debug.Log("E grade soil");
            break;
    }

}*/

/*public override void SetGridObject(GridObject gridObject)
{
    base.SetGridObject(gridObject);
    TerrainTile.tileUnder = gridObject;
    gridObject.objectOnTile = this;
    Debug.LogWarning(TerrainTile.tileUnder);
}*/

/*private void TerrainTileObject()
{
    //Terain_gridObject.
}*/

