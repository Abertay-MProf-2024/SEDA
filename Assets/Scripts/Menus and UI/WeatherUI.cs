using TMPro;
using UnityEngine;

public class WeatherUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentWeatherButtonText;
    [SerializeField] TextMeshProUGUI currentWeatherText;

    private void Update()
    {
        currentWeatherButtonText.text = currentWeatherText.text = Inventory.GetCurrentWeather().ToString();
    }
}
