using System.Collections;
using UnityEngine;

public enum TerrainTypes
{
    None,
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

public enum SpecialTypes
{
    None,
    River,
    Mountain,
    Shore
}

public enum CreatureTypes
{
    None,
    Giant,
    Kelpie,
    Cailleach
}

public class Terrainsystem : MonoBehaviour
{

    public SoilType CurrentsoilType;
    public enum SoilType
    {
        A = 100,
        B = 80,
        C = 60,
        D = 40,
        E = 20
    }

    TileBase creaturetile;

    public bool ResourceAffect;

    [SerializeField] public TerrainTypes terraintype;
    [SerializeField] public SpecialTypes specialtype;
    [SerializeField] public CreatureTypes creaturetype;


    //if the tile gives/has land energy
    public bool Lenergy = false;
    //if the tile gives/has water energy
    [SerializeField] bool Wenergy = false;


    public GridObject owningGridObject;

    //the radius in which it gives off energy
    public int radius;
    //the radius in which it gives off energy
    public int Wradius;


    //the total health of the soil (A to E grade)
    public int health;

    //to calculate the avg soilHealth
    public static int totalHealth;
    //to count the number of tiles
    public static int tilecount;

    //VeilSwitch Details
    [HideInInspector] public TerrainTypes OldsoilType;
    public TerrainTypes NewSoilType;


    private void Start()
    {

        OldsoilType = terraintype;

        health = (int)CurrentsoilType;

        tilecount++;

        TimeSystem.AddMonthlyEvent(this,HealthBar, 1, true, 2);
        //TimeSystem.AddMonthlyEvent(ResetValuesSoilGrade, 1, true, 2);

        //TimeSystem.AddMonthlyEvent(ChangeinGrade, 1, true, 2);

        HealthBar();
        SetTerrainMaterialProperties();

        StartCoroutine(SetEnergyTiles()); 
    }

    public void TriggerEnergy()
    {
        //if the terrain has energy being emitted, then set all the terraintiles' energy bool true.
        if (Lenergy && owningGridObject)
        {
            GridPosition pos = owningGridObject.GetGridPosition();

            for (int x = pos.x - radius; x <= pos.x + radius; x++)
            {
                for (int z = pos.z - radius; z <= pos.z + radius; z++)
                {
                    GridObject Energyobj = owningGridObject.GetOwningGridSystem().GetGridObject(x, z);
                    if (Energyobj != null)
                    {
                        Energyobj.SetTerrainEnergy(true);
                    }
                }
            }
        }
        if (Wenergy && owningGridObject)
        {
            GridPosition pos = owningGridObject.GetGridPosition();

            for (int x = pos.x - Wradius; x <= pos.x + Wradius; x++)
            {
                for (int z = pos.z - Wradius; z <= pos.z + Wradius; z++)
                {
                    GridObject Energyobj = owningGridObject.GetOwningGridSystem().GetGridObject(x, z);
                    if (Energyobj != null)
                    {
                        Energyobj.SetTerrainWaterEnergy(true);
                    }
                }
            }
        }

    }

    IEnumerator SetEnergyTiles()
    {
        yield return new WaitForSeconds(10);
        TriggerEnergy();
        
    }

    public static void ResetValuesSoilGrade()
    {
        totalHealth = 0;
    }

    void HealthBar()
    {
        totalHealth = totalHealth + (int)CurrentsoilType;
    }

    public void SetTerrainMaterialProperties(bool flood=false)
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();

        if (renderer == null)
            return;

        Material[] materialsArray = GetComponent<MeshRenderer>().materials;

        // Set Soil Quality
        float quality;
        if (flood)
            quality = 1;
        else
            quality = health / 100f;

        // Set Land Type
        bool landType;
        if (terraintype == TerrainTypes.Highland)
            landType = true;
        else
            landType = false;

        bool hasGravel;
        if (owningGridObject && owningGridObject.buildingInstance && owningGridObject.buildingInstance.resourceData.hasGravel)
            hasGravel = true;
        else
            hasGravel = false;

        foreach (Material mat in materialsArray)
        {
            mat.SetFloat("_SoilQuality", quality);
            mat.SetFloat("_LandType_Grass_High", landType ? 1f : 0f);
            mat.SetFloat("_GravelBlended", hasGravel ? 1f : 0f);
        }
    }

    public void ChangeinGrade(float buffamount, float nerfamount, bool impact)
    {
        float totalChangeInGrade = buffamount - nerfamount + WeatherSystem.soilGradeWeatherEffect;
        if (impact)
        {

            health = (int)CurrentsoilType + (int)totalChangeInGrade;
            
            if(health >100)
                health = 100;

            //reference to Building, to reduce it by (health)

            if (health > 0)
            {
                if (health > 80)
                {
                    CurrentsoilType = SoilType.A;
                }
                else if (health > 60 && health <= 80)
                {
                    CurrentsoilType = SoilType.B;
                }
                else if (health > 40 && health <= 60)
                {
                    CurrentsoilType = SoilType.C;
                }
                else if (health > 20 && health <= 40)
                {
                    CurrentsoilType = SoilType.D;
                }
                else if (health >= 0 && health <= 20)
                {
                    CurrentsoilType = SoilType.E;
                }
            }
        }
    }

    public bool GetWaterEnergy()
    {
        return Wenergy;
    }

    public void SetWaterEnergy(bool hasWater)
    {
        Wenergy = hasWater;
        SetTerrainMaterialProperties();
    }
}

