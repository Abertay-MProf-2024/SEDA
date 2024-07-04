using TMPro;
using UnityEngine;

public class ResourceUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI foodDisplay;
    [SerializeField] TextMeshProUGUI materialsDisplay;
    [SerializeField] TextMeshProUGUI healthBarDisplay;

    private void Update()
    {
        foodDisplay.text = Inventory.food.ToString();
        materialsDisplay.text =  Inventory.constructionMaterials.ToString();
        healthBarDisplay.text = Inventory.healthBar.ToString();
    }
}
