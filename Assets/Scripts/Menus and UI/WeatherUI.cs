using TMPro;
using UnityEngine;

public class WeatherUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentWeatherButtonText;
    [SerializeField] TextMeshProUGUI currentWeatherText;

    private void Update()
    {
        string weatherName = Inventory.GetCurrentWeather().ToString();
        
        // So the name can look nice in the UI
        if (weatherName == "Fair")
        {
            weatherName = "Fair Weather";
        }

        currentWeatherButtonText.text = currentWeatherText.text = weatherName;
    }
}
