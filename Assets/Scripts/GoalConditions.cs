using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoalConditions : MonoBehaviour
{
    [SerializeField] public TextMeshProUGUI FoodText;

    [SerializeField] public TextMeshProUGUI ConstructionMaterialText;

    //to automatically update the goal conditions for Food and Construction Materials for each level
    private void Update()
    {
        FoodText.text = "Food: " + Inventory.foodtoComplete.ToString();
        ConstructionMaterialText.text = "Construction Materials: " + Inventory.CMtoComplete.ToString();
    }
}
