using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildSystem : MonoBehaviour
{

 
    public GameObject Tutorial_Step_5;
    public GameObject Tutorial_Step_4;
    public bool Is_Set_Tutorial_Step_5=true;


    [SerializeField]
    InputActionAsset actionAsset;

    [SerializeField]
    BuildingTypeSelect buildingTypeSelect;

    InputAction placeAction;
    InputAction tapLocation;

    public static bool isInBuildMode = false;

    // This function reference is necessary for callback registering/deregistering to work properly
    Action<InputAction.CallbackContext> possessCamera;

    private void Start()
    {
        placeAction = actionAsset.FindAction("PossessCamera");
        possessCamera = ctx => PlaceBuilding();
        placeAction.performed += possessCamera;

        tapLocation = actionAsset.FindAction("PanCamera");
    }

    void PlaceBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(tapLocation.ReadValue<UnityEngine.Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.tag == "Grid")
        {
            GridObject hitGridObject;
            if (hitGridObject = hit.collider.gameObject.GetComponent<GridObject>())
            {

                hitGridObject.TryBuild(buildingTypeSelect.currentBuildingType);

                //ShowTutorial();// Designer Tutorial test

            }
        }
    }

    private void OnDestroy()
    {
        placeAction.performed -= possessCamera;
    }

    public void ShowTutorial()// Designer Tutorial test
    {
        if (Is_Set_Tutorial_Step_5)
        {
            Tutorial_Step_5.SetActive(true);
            Tutorial_Step_4.SetActive(false);
        }
    }
}
