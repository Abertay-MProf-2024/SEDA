using UnityEngine;
using UnityEngine.UI;


public class BuildingTypeSelect : MonoBehaviour
{
    public GridSystem gridSystem;
    [Header("Buttons")]
    public GameObject[] buildingButtons;
    public TileBase currentBuildingType;
   

    public void ColorChange(GameObject activeButton)
    {
        if (activeButton.GetComponent<Image>().color == Color.white)
        {
            activeButton.GetComponent<Image>().color = Color.green;
        }

        else if (activeButton.GetComponent<Image>().color == Color.green)
        {
            activeButton.GetComponent<Image>().color = Color.white;
        }

        foreach (GameObject button in buildingButtons)
        {
            if (button != activeButton)
            {
                button.GetComponent<Image>().color = Color.white;
            }
        }
    }
    //ResetButtonColour
    public void GridOff()
    {
        foreach (GameObject button in buildingButtons)
        {
           
                button.GetComponent<Image>().color = Color.white;
            
        }
    }

    public void SelectBuilding(TileBase building)
    {
        if (currentBuildingType == building)
        {
            currentBuildingType = null;
           
            gridSystem.ToggleBuildMode(currentBuildingType, false);
            
        }
        else
        {
            currentBuildingType = building;
            gridSystem.ToggleBuildMode(building, true);
        }
    }

    public void GridDisplay(TileBase building)
    {
        gridSystem.ToggleBuildMode(building, false, true);
    }

    void BuildingCanPlace(int buildingType, GirdStatus[,] girdArray)
    {
        foreach (GirdStatus element in girdArray )
        {
            element.gameObject.GetComponent<GirdStatus>().Buildmode = true;
        }
    }

    void BuildingCantPlace(int buildingType, GirdStatus[,] girdArray)
    {
        foreach (GirdStatus element in girdArray)
        {
            element.gameObject.GetComponent<GirdStatus>().Buildmode = false;
        }
    }

    public void CloseButton()
    {
        currentBuildingType = null;
        gridSystem.ToggleBuildMode(currentBuildingType, false);

    }
}
