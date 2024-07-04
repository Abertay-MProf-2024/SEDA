using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;

public enum CollectorType
{
    None,
    Barn,
    [Description("Wind Turbine")]
    WindTurbine,
    [Description("Water Pump")]
    WaterPump,
    Warehouse,
    [Description("Logging Camp")]
    LoggingCamp
}

[CreateAssetMenu(fileName = "TileBase", menuName = "TileBase")]
public class TileBase : ScriptableObject
{
    //GeneralBase
    [Header("General")]
    [Tooltip("Name of the object that will be displayed in-game e.g. on hover over via UI.")]
    public string tileName;
    [Tooltip("Image (drag and drop to here) (Resolution of Images in UI Spec Sheet")]
    public Sprite icon;
    [Tooltip("GameObject with 3D static Mesh (Drag and Drop) (Scale See Metrics & Scale (See Grid scale)")]
    public GameObject inGameAsset;
    [Tooltip("Number of Tiles on grid Width")]
    public int sizeWidthTile = 1;
    [Tooltip("Number of Tiles on grid Length")]
    public int sizeLengthTile = 1;
    [Tooltip("Structure Types")]
    public StructureTypes structureType;
    [Description("Structure Types")]
    public enum StructureTypes
    {
        Barn,
        [Description("Wind Turbine")]
        WindTurbine,
        [Description("Water Pump")]
        WaterPump,
        Warehouse,
        [Description("Logging Camp")]
        LoggingCamp,
        Giant,
        Kelpie,
        Cailleach,
        [Description("Cow Pasture")]
        CowPasture,
        [Description("Sheep Pasture")]
        SheepPasture,
        [Description("Spring Barley")]
        SpringBarley,
        Forest,
        [Description("Ruined Buildings")]
        RuinedBuildings,
        [Description("Artificial Fertiliser")]
        ArtificialFertiliser,
        [Description("Peas and Beans")]
        PeasAndBeans,
        Compost
    }
    [Tooltip("Grab reference and information of the tile under the structure/ the tile this structure is placed on top of.")]
    public GridObject tileUnder;
    [Tooltip("List of biomesTypes(levels) this structure can be build within.")]
    public List<BiomeTypes> biomeTypes;
    public enum BiomeTypes
    {
        Island
    }

    [Tooltip("List of tileTerrainTypes this structure can be placed on.")]
    public List<TerrainTypes> tileTerrainTypes;

    //BuildBase
    [Header("Creature")]
    [Tooltip("Checks if a tile is Creature, if not it hides the Building section inengine and in the hierarchy.")]
    public bool CreatureTile;
    [Tooltip("Amount of Food to Bribe - Creature")]
    public int bribeCostFood = 15;
    [Tooltip("Amount of Construction Material to Bribe - Creature")]
    public int bribeCostConstruction = 5;
    
    

    //BuildBase
    [Header("Building")]
    [Tooltip("Checks if a tile is buildable, if not it hides the Building section inengine and in the hierarchy.")]
    public bool canBuild;

    [Tooltip("Building cost of constructing the building.-Food")]
    public int buildingCostFood;
    [Tooltip("Building cost of constructing the building.-Construction")]
    public int buildingCostMaterial;
    [Tooltip("Whether a structure requires energy.")]
    public bool upKeepCostEnergy;
    [Tooltip("Whether a structure requres water.")]
    public bool upKeepCostWater;
    [Tooltip("Monthly upkeep cost of sustaining building-Food")]
    public int upKeepCostFood;
    [Tooltip("Monthly upkeep cost of sustaining building-Construction")]
    public int upKeepCostMaterial;

    [Header("Resources")]
    [Tooltip("checks if the resource will be added to the monthly output (some structures need to be tapped to receive the base output")]
    public bool isResourceTapped;
    [Tooltip("The type of Structure this collects resources from, if any.")]
    public CollectorType[] collectorBuildings;
    //ResourceOutput
    [Tooltip("Whether a structure generates energy.")]
    public bool baseOutputEnergy;
    [Tooltip("Whether a structure generates water.")]
    public bool baseOutputWater;
    [Tooltip("BaseOutput-Food")]
    public int baseOutputFood;
    [Tooltip("BaseOutput-Construction")]
    public int baseOutputMaterial;
    [Tooltip("Multiplier of resources when receiving transferred resources (%)")]
    public float buildingOutputMulti;

    [Header("ResourceImpact")]
    [Tooltip("Check if this structure has an impact on other tiles within a radius.")]
    public bool impactSource;
    [Tooltip("Number of tiles in each direction that this building can Impact. (all 8 directions from centre).")]
    public int impactRadiusTiles;
    [Tooltip("multiplier to output from the buff Source")]
    public float buffAmount;
    [Tooltip("multiplier to output from the nerf Source")]
    public float nerfAmount;
    [Tooltip("list of objects this applies the buff to if insideImpactRadius")]
    public List<TileBase> tileImpactBuff;
    [Tooltip("list of objects this applies the nerf to if insideImpactRadius")]
    public List<TileBase> tileImpactNerf;
    [Tooltip("Check if this structure impacts the soil grade of surrounding terrain.")]
    public bool isImpactSoilGrade;
    [Tooltip("The amount to buff soil grade.")]
    public float buffSoilGradeAmount;
    [Tooltip("The amount to nerf soil grade.")]
    public float nerfSoilGradeAmount;
}