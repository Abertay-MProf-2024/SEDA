using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class StandingStone : MonoBehaviour
{
    [SerializeField]
    InputActionAsset actionAsset;

    public Kelpie kelpie;
    
    public Cailleach cailleach;

    InputAction placeAction;
    InputAction tapLocation;

    Terrainsystem TS1;

    [SerializeField] GameObject IslandToChange;

    [SerializeField] StandingStonPrefabPopUp StandingStonePrefab;

    // This function reference is necessary for callback registering/deregistering to work properly
    Action<InputAction.CallbackContext> click;

    private void Start()
    {
        //kelpie = FindFirstObjectByType<Kelpie>();
        //cailleach = FindFirstObjectByType<Cailleach>();
        if(kelpie)
            kelpie.gameObject.SetActive(false);
        if(cailleach)
            cailleach.gameObject.SetActive(false);

        placeAction = actionAsset.FindAction("click");
        click = ctx => Interact();
        placeAction.performed += click;

        tapLocation = actionAsset.FindAction("PanCamera");

        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            //Debug.DrawLine(transform.position, transform.position + Vector3.down * 100, Color.red, 500);
            TS1 = hit.transform.gameObject.GetComponent<Terrainsystem>();
        }
    }

    public void OpenStandingStone()
    {
        if (StandingStonePrefab)
        {
            StandingStonPrefabPopUp standingStoneUI = Instantiate(StandingStonePrefab.gameObject).GetComponent<StandingStonPrefabPopUp>();
            standingStoneUI.SetStandingStoneReference(this);
        }
    }

    void Interact()
    {
        Ray ray = Camera.main.ScreenPointToRay(tapLocation.ReadValue<UnityEngine.Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "StandingStone")
        {
            OpenStandingStone();
        }
    }

    public void VeilSwitch()
    {
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
                t.InitialTerrainList();
                t.ChangeinGrade(0,20,true);
                t.SetTerrainMaterialProperties();
            }
            else
            {
                t.terraintype = t.OldsoilType;
                t.InitialTerrainList();
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
                    if (building.resourceData == building.oldresourceData)
                    {
                        building.VeilChangeActivate();
/*                        newResourceData = 
                            Destroy
                            Instantiate*/
                        
                    }
                    else
                    {
                        building.VeilChangeDeactivate();
                        
                    }
                }
            }
        }
    }

    private void OnDestroy()
    {
        placeAction.performed -= click;
    }
}
