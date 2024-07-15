using TMPro;
using UnityEngine;

public class WeatherUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentWeatherButtonText;
    [SerializeField] TextMeshProUGUI currentWeatherText;

    private void Update()
    {
        string weatherName = WeatherSystem.GetCurrentWeather().ToString();
        
        // So the name can look nice in the UI
        if (weatherName == "Fair")
        {
            weatherName = "Fair Weather";
        }

        currentWeatherText.text = weatherName;
    }
}
