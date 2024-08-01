using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Inventory : MonoBehaviour
{
    private static Inventory instance;

    public static int overworldTime;
    public static int levelTime = 36;
    public static int food = 100;
    public static int constructionMaterials = 100;
    public static int healthBar = 100;
    public static int totalhealth = 0;
    public static int count = 0;

    // Building Types
    public static int numOfLoggingCamps = 0;
    public static int numOfForests = 0;
    public static int numOfMines = 0;
    public static int numOfRocks = 0;

    [SerializeField]
    int initialOverworldTime;
    [SerializeField]
    int initialFood;
    [SerializeField]
    int initialConstructionMaterials;

    [SerializeField] TextMeshProUGUI foodDisplay;
    [SerializeField] TextMeshProUGUI constructionMaterialDisplay;
    [SerializeField] TextMeshProUGUI healthBarDisplay;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            overworldTime = initialOverworldTime;
            food = initialFood;
            constructionMaterials = initialConstructionMaterials;
            SceneManager.sceneLoaded += ReassignInitialVariables;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void ReassignInitialVariables(Scene scene, LoadSceneMode mode)
    {
        overworldTime = initialOverworldTime;
        food = initialFood;
        constructionMaterials = initialConstructionMaterials;

        numOfLoggingCamps = 0;
        numOfForests = 0;
        numOfMines = 0;
        numOfRocks = 0;
    }

    public static void SpendFood(int foodSpent)
    {
        food -= foodSpent;
        
        if (food < 0)
        {
            food = 0;
        }
    }

    public static void SpendMaterials(int materialSpent)
    {
        constructionMaterials -= materialSpent;

        if (constructionMaterials < 0)
        {
            constructionMaterials = 0;
        }
    }

    public static void ClearResources()
    {
        food = 0;
        constructionMaterials = 0;
    }

    public static void HealthBarChange()
    {
        healthBar =  Terrainsystem.totalHealth  / Terrainsystem.tilecount;
        Debug.Log(healthBar);
    }

}
