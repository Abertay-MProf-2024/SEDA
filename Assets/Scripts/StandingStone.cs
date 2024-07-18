using System.Collections;
using UnityEngine;

public class StandingStone : MonoBehaviour
{
    public Kelpie kelpie;
    
    public Cailleach cailleach;

    Terrainsystem TS1;

    [SerializeField] GameObject IslandToChange;

    [SerializeField] StandingStonPrefabPopUp StandingStonePrefab;

    GameObject standingStoneVFX;

    [SerializeField] GameObject vfxIdle;
    [SerializeField] GameObject vfxEngaged;
    [SerializeField] GameObject vfxActivate;

    bool isIdle = true;

    private void Start()
    {
        //kelpie = FindFirstObjectByType<Kelpie>();
        //cailleach = FindFirstObjectByType<Cailleach>();
        if(kelpie)
            kelpie.gameObject.SetActive(false);
        if(cailleach)
            cailleach.gameObject.SetActive(false);

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            //Debug.DrawLine(transform.position, transform.position + Vector3.down * 100, Color.red, 500);
            TS1 = hit.transform.gameObject.GetComponent<Terrainsystem>();
        }

        standingStoneVFX = Instantiate(vfxIdle, transform, false);
    }

    public void OpenStandingStone()
    {
        if (StandingStonePrefab)
        {
            StandingStonPrefabPopUp standingStoneUI = Instantiate(StandingStonePrefab.gameObject).GetComponent<StandingStonPrefabPopUp>();
            standingStoneUI.SetStandingStoneReference(this);
        }
    }

    public void VeilSwitch()
    {
        isIdle = !isIdle;
        
        if (isIdle)
        {
            StartCoroutine(SwapVFX(vfxIdle));
        }
        else
        {
            StartCoroutine(SwapVFX(vfxEngaged));
        }

        if(kelpie != null)
            kelpie.StandingStoneKelpieImpact();
        if (cailleach != null)
            cailleach.StandingStoneCailleachImpact();
        
        Terrainsystem[] list = IslandToChange.GetComponentsInChildren<Terrainsystem>();

        foreach (Terrainsystem t in list)
        {
            if (t.terraintype == t.OldsoilType)
            {
                t.terraintype = t.NewSoilType;
                //t.InitialTerrainList();
                t.ChangeinGrade(0,20,true);
                t.SetTerrainMaterialProperties();
            }
            else
            {
                t.terraintype = t.OldsoilType;
                //t.InitialTerrainList();
                t.ChangeinGrade(0, 20, true);
                t.SetTerrainMaterialProperties();
            }

        }

        foreach (Terrainsystem t in list)
        {
            if (t.owningGridObject != null)
            {
                Building building = t.owningGridObject.GetBuilding();
                if (building != null)
                {
                    if (building.resourceData == building.oldResourceData)
                    {
                        building.VeilChangeActivate(); 
                    }
                    else
                    {
                        building.VeilChangeDeactivate();
                    }
                }
            }
        }
    }

    IEnumerator SwapVFX(GameObject newEffect)
    {
        Destroy(standingStoneVFX);
        standingStoneVFX = Instantiate(vfxActivate, transform, false);
        yield return new WaitForSeconds(2f);

        Destroy(standingStoneVFX);
        standingStoneVFX = Instantiate(newEffect, transform, false);
    }
}
