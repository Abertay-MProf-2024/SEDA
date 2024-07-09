using UnityEngine;
public enum WeatherTypes
{
    Fair,
    Tornado,
    Thunderstorm,
    Flood
}

public class WeatherSystem : MonoBehaviour
{
    // Weather events status
    static bool hasTornadoHappened = false;
    static bool hasFloodHappened = false;

    public static float cropOutput = 1f;
    public static  int soilGradeWeatherEffect = 0;
    public static bool isFlooding = false;

    static WeatherTypes currentWeather;

    [SerializeField] GameObject tornadoEffectPrefab;
    [SerializeField] GameObject thunderstormEffectPrefab;
    [SerializeField] GameObject floodEffectPrefab;

    static GameObject currentWeatherEffect;

    private void Start()
    {
        TimeSystem.AddMonthlyEvent(SetWeather);
    }

    public void SetWeather()
    {
        if (currentWeather != WeatherTypes.Thunderstorm)
        {
            if (!hasTornadoHappened && Inventory.numOfLoggingCamps >= 3)
            {
                currentWeather = WeatherTypes.Tornado;
                SetWeatherEffect(tornadoEffectPrefab);
                cropOutput = 0.7f;
                hasTornadoHappened = true;
            }
            else if (!hasFloodHappened && Inventory.healthBar < 60)
            {
                currentWeather = WeatherTypes.Flood;
                SetWeatherEffect(floodEffectPrefab);
                cropOutput = 0.5f;
                soilGradeWeatherEffect = 20;
                isFlooding = true;
                hasFloodHappened = true;
            }
            else if (currentWeather != WeatherTypes.Fair)
            {
                FairWeather();
            }
        }
    }

    public void CailleachAppeared()
    {
        currentWeather = WeatherTypes.Thunderstorm;
        SetWeatherEffect(thunderstormEffectPrefab);
        cropOutput = 0.9f;
        soilGradeWeatherEffect = -20;
    }

    public static void FairWeather()
    {
        currentWeather = WeatherTypes.Fair;
        SetWeatherEffect(null);
        cropOutput = 1f;
        soilGradeWeatherEffect = 0;
        isFlooding = false;
    }

    static void SetWeatherEffect(GameObject weatherPrefab)
    {
        Destroy(currentWeatherEffect);

        if (weatherPrefab)
            currentWeatherEffect = Instantiate(weatherPrefab);
    }

    public static WeatherTypes GetCurrentWeather()
    {
        return currentWeather;
    }
}
