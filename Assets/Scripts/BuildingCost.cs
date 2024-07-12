using TMPro;
using UnityEngine;

public class BuildingCostUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI BuildingNameDisplay;
    [SerializeField] TextMeshProUGUI ReqFoodDisplay;
    [SerializeField] TextMeshProUGUI ReqConstMatDisplay;

    [SerializeField] BuildingTypeSelect buildingTypeSelect;

    private void Update()
    { 
        if (buildingTypeSelect.currentBuildingType)
        {
            BuildingNameDisplay.text = buildingTypeSelect.currentBuildingType.name.ToString();
            ReqFoodDisplay.text = buildingTypeSelect.currentBuildingType.buildingCostFood.ToString();
            ReqConstMatDisplay.text = buildingTypeSelect.currentBuildingType.buildingCostMaterial.ToString();
        }
    }
}
