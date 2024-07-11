using UnityEngine;

public class BuildingCost : MonoBehaviour
{
    private static BuildingCost instance;
    [Header("Building consumption")]
    public static int build1CostFood = 50;
    public static int build1CostMaterials = 0;
    public static int build2CostFood = 0;
    public static int build2CostMaterials = 0;
    public static int build3CostFood = 0;
    public static int build3CostMaterials = 0;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
}
