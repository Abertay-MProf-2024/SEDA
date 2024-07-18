using UnityEngine;
using UnityEngine.EventSystems;

public class BuildSystem : MonoBehaviour
{
    public GameObject Tutorial_Step_5;
    public GameObject Tutorial_Step_4;
    public bool Is_Set_Tutorial_Step_5=true;

    [SerializeField]
    BuildingTypeSelect buildingTypeSelect;

    public static bool isInBuildMode;

    public void PlaceBuilding(Ray ray)
    {
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit) && hit.collider.gameObject.layer != 5)
        {
            if (hit.collider.gameObject.tag == "Grid")
            {
                GridObject hitGridObject;
                if (hitGridObject = hit.collider.gameObject.GetComponent<GridObject>())
                {
                    hitGridObject.TryBuild(buildingTypeSelect.currentBuildingType);
                }
            }
        }
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
