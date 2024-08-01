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
    public static bool hasFloodHappened = false;

    public static float cropOutput = 1f;
    public static int soilGradeWeatherEffect = 0;
    public static bool isFlooding = false;

    static WeatherTypes currentWeather;

    [SerializeField] GameObject tornadoEffectPrefab;
    [SerializeField] GameObject thunderstormEffectPrefab;
    [SerializeField] GameObject floodEffectPrefab;

    [SerializeField] AudioClip fairWeatherSound; // New audio clip for fair weather (A)
    [SerializeField] AudioClip tornadoSound; // New audio clip for tornado (A)
    [SerializeField] AudioClip thunderstormSound; // New audio clip for thunderstorm (A)
    [SerializeField] AudioClip floodSound; // New audio clip for flood (A)

    private AudioSource audioSource; // AudioSource component (A)

    static GameObject currentWeatherEffect;

    public GridSystem owningGridSystem;
    int gridLength = 1;
    int gridWidth = 1;

    private static WeatherSystem instance; // Static instance (A)

    private void Awake()
    {
        if (instance == null) // Ensure only one instance exists (A)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>(); // Add AudioSource component (A)
        TimeSystem.AddMonthlyEvent(this, SetWeather, 1, true, 6);
    }

    private void Start()
    {
        PlayCurrentWeatherSound(); // Play weather sound on game start (A)
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
                PlayWeatherSound(tornadoSound); // Play tornado sound (A)
            }
            else if (!hasFloodHappened && Inventory.healthBar < 60)
            {
                currentWeather = WeatherTypes.Flood;
                //SetFloodEffect();
                SetWeatherEffect(floodEffectPrefab);
                cropOutput = 0.5f;
                soilGradeWeatherEffect = 20;
                isFlooding = true;
                hasFloodHappened = true;
                PlayWeatherSound(floodSound); // Play flood sound (A)
            }
            else if (currentWeather != WeatherTypes.Fair)
            {
                FairWeather();
            }
        }
    }

    void SetFloodEffect()
    {
        gridLength = owningGridSystem.GetGridLength();
        gridWidth = owningGridSystem.GetGridWidth();

        for (int x = 0; x < gridLength; x++)
        {
            for (int z = 0; z < gridWidth; z++)
            {
                GridObject Energyobj = owningGridSystem.GetGridObject(x, z);
                if (Energyobj != null)
                {
                    Energyobj.SetTerrainWaterEnergy(true);
                    Energyobj.terrain.SetTerrainMaterialProperties(true);
                }
            }
        }
    }

    public void CailleachAppeared()
    {
        currentWeather = WeatherTypes.Thunderstorm;
        SetWeatherEffect(thunderstormEffectPrefab);
        cropOutput = 0.9f;
        soilGradeWeatherEffect = -20;
        PlayWeatherSound(thunderstormSound); // Play thunderstorm sound (A)
    }

    public static void FairWeather()
    {
        currentWeather = WeatherTypes.Fair;
        SetWeatherEffect(null);
        cropOutput = 1f;
        soilGradeWeatherEffect = 0;
        isFlooding = false;
        PlayWeatherSoundInstance(instance.fairWeatherSound); // Play fair weather sound (A)
    }

    static void SetWeatherEffect(GameObject weatherPrefab)
    {
        Destroy(currentWeatherEffect);

        if (weatherPrefab)
        {
            currentWeatherEffect = Instantiate(weatherPrefab);

            if (currentWeatherEffect.GetComponent<Canvas>() != null)
            {
                currentWeatherEffect.GetComponent<Canvas>().worldCamera = FindAnyObjectByType<Camera>();
            }
        }
    }

    // Method to play weather sound (A)
    private void PlayWeatherSound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    // Static method to play weather sound (A)
    private static void PlayWeatherSoundInstance(AudioClip clip)
    {
        if (instance != null && instance.audioSource != null && clip != null)
        {
            instance.audioSource.clip = clip;
            instance.audioSource.Play();
        }
    }

    // Method to play current weather sound on game start (A)
    private void PlayCurrentWeatherSound()
    {
        switch (currentWeather)
        {
            case WeatherTypes.Fair:
                PlayWeatherSound(fairWeatherSound);
                break;
            case WeatherTypes.Tornado:
                PlayWeatherSound(tornadoSound);
                break;
            case WeatherTypes.Thunderstorm:
                PlayWeatherSound(thunderstormSound);
                break;
            case WeatherTypes.Flood:
                PlayWeatherSound(floodSound);
                break;
        }
    }

    public static WeatherTypes GetCurrentWeather()
    {
        return currentWeather;
    }
}
