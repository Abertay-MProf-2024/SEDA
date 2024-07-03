using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Terrainsystem;


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
    public List<SoilType> allowedSoilGrade;

    SoilType testsoil;

    public bool ResourceAffect;

    [SerializeField] public TerrainTypes terraintype;
    [SerializeField] public CreatureTypes creaturetype;


    //if the tile gives/has land energy
    public bool Lenergy = false;
    //if the tile gives/has water energy
    public bool Wenergy = false;


    public GridObject owningGridObject;

    //the radius in which it gives off energy
    public int radius;
    //the radius in which it gives off energy
    public int Wradius;


    //the total health of the soil (A to E grade)
    public int health;

    //VeilSwitch Details
    public TerrainTypes OldsoilType;
    public TerrainTypes NewSoilType;


    private void Start()
    {

        OldsoilType = terraintype;

        health = (int)CurrentsoilType;
        

        if (ResourceAffect)
        {
            TimeSystem.AddMonthlyEvent(HealthBar);
            //TimeSystem.AddMonthlyEvent(ChangeinGrade);

        }

        InitialTerrainList();
        HealthBar();
        SetTerrainMaterialProperties();
 
        StartCoroutine(Stupidity());
        // ChangeinGrade();
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

    IEnumerator Stupidity()
    {
        yield return new WaitForSeconds(10);
        TriggerEnergy();
        //ChangeinGrade();
        
    }

    void HealthBar()
    {
        Inventory.count++;
        Inventory.totalhealth += (int)CurrentsoilType;
        Inventory.HealthBarChange();
    }
    void SetTerrainMaterialProperties()
    {
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();

        if (renderer == null)
            return;

        Material[] materialsArray = GetComponent<MeshRenderer>().materials;

        float quality = health / 100f;

        foreach (Material mat in materialsArray)
        {
            mat.SetFloat("_SoilQuality", quality);
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

    public void ChangeinGrade(float buffamount, float nerfamount, bool impact)
    {
        float totalChangeInGrade = buffamount - nerfamount + Inventory.soilGradeWeatherEffect;
        if (impact)
        {
            health = (int)CurrentsoilType + (int)totalChangeInGrade;

            //reference to Building, to reduce it by (health)

            if (health > 0)
            {
                if (health > 80)
                {
                    testsoil = SoilType.A;
                    if (allowedSoilGrade.Contains(testsoil))
                        CurrentsoilType = SoilType.A;
                }

                else if (health > 60 && health <= 80)
                {
                    testsoil = SoilType.B;
                    if (allowedSoilGrade.Contains(testsoil))
                        CurrentsoilType = SoilType.B;

                }
                else if (health > 40 && health <= 60)
                {
                    testsoil = SoilType.C;
                    if (allowedSoilGrade.Contains(testsoil))
                        CurrentsoilType = SoilType.C;

                }
                else if (health > 20 && health <= 40)
                {
                    testsoil = SoilType.D;
                    if (allowedSoilGrade.Contains(testsoil))
                        CurrentsoilType = SoilType.D;

                }
                else if (health >= 0 && health <= 20)
                {
                    testsoil = SoilType.E;
                    if (allowedSoilGrade.Contains(testsoil))
                        CurrentsoilType = SoilType.E;
                }
            }
        }

    }

    /*public void Impact()
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

            }*/

}

/*void TileHealth()
{
    health = (int)soilType;

    //reference to Building, to reduce it by (health)
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

