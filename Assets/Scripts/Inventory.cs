// Remy Pijuan 2024

using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WeatherTypes
{
    Fair,
    Tornado,
    Thunderstorm,
    Flood
}

public class Inventory : MonoBehaviour
{
    private static Inventory instance;

    public static int overworldTime;
    public static int levelTime = 36;
    public static int food = 100;
    public static int constructionMaterials = 100;
    public static int healthBar = 0;
    public static int totalhealth = 0;
    public static int count = 0;

    // Building Types
    public static int numOfLoggingCamps = 0;
    public static int numOfForests = 0;
    public static int numOfMines = 0;
    public static int numOfRocks = 0;

    // Weather events status
    static bool hasTornadoHappened = false;
    static bool hasCailleachAppeared = false;
    static bool hasFloodHappened = false;

    public static float cropOutput = 1f;
    public static int soilGradeWeatherEffect = 0;
    public static bool isFlooding = false;

    static WeatherTypes currentWeather;

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

        hasFloodHappened = false;
        hasCailleachAppeared = false;
        hasTornadoHappened = false;

        currentWeather = WeatherTypes.Fair;
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
        healthBar =  totalhealth  / count;
        Debug.Log("TotalHealth : " + totalhealth);
        Debug.Log("Count : " + count);
        Debug.Log("Healthbar : " + healthBar);
    }

    public static void SetWeather()
    {
        if (!hasTornadoHappened && numOfLoggingCamps >= 3)
        {
            currentWeather = WeatherTypes.Tornado;
            cropOutput = 0.7f;
            hasTornadoHappened = true;
        }
        else if (hasCailleachAppeared)
        {
            currentWeather = WeatherTypes.Thunderstorm;
            cropOutput = 0.9f;
            soilGradeWeatherEffect = -20;
            hasCailleachAppeared = false;
        }
        else if (!hasFloodHappened && healthBar < 60)
        {
            currentWeather = WeatherTypes.Flood;
            cropOutput = 0.5f;
            soilGradeWeatherEffect = 20;
            isFlooding = true;
            hasFloodHappened = true;
        }
        else if (currentWeather != WeatherTypes.Fair)
        {
            currentWeather = WeatherTypes.Fair;
            cropOutput = 1f;
            soilGradeWeatherEffect = 0;
            isFlooding = false;
        }
    }

    public static void CailleachAppeared()
    {
        hasCailleachAppeared = true;
    }

    public static WeatherTypes GetCurrentWeather()
    {
        return currentWeather;
    }
}
