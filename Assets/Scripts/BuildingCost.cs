using TMPro;
using UnityEngine;

public class BuildingCostUI : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI BuildingNameDisplay;
    [SerializeField] public TextMeshProUGUI DescriptionDisplay;

    [SerializeField] BuildingTypeSelect buildingTypeSelect;


    // To automatically show the details of the Buildings when clicked on
    private void Update()
    { 
        if (buildingTypeSelect.currentBuildingType)
        {
            BuildingNameDisplay.text = buildingTypeSelect.currentBuildingType.inGameAsset.name.ToString();
            DescriptionDisplay.text = buildingTypeSelect.currentBuildingType.tileDescription.ToString();

        }
    }
}
