using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeatherUI : MonoBehaviour
{
    [SerializeField] Image currentWeatherIcon;
    [SerializeField] TextMeshProUGUI weatherText;

    [SerializeField] Sprite fairWeatherIcon;
    [SerializeField] Sprite tornadoIcon;
    [SerializeField] Sprite thunderstormIcon;
    [SerializeField] Sprite floodIcon;

    private void Update()
    {
        WeatherTypes weatherName = WeatherSystem.GetCurrentWeather();
        
        // So the name can look nice in the UI
        switch(weatherName)
        {
            case WeatherTypes.Fair:
                currentWeatherIcon.sprite = fairWeatherIcon;
                weatherText.text = "Fair Weather";
                break;
            case WeatherTypes.Tornado:
                currentWeatherIcon.sprite = tornadoIcon;
                weatherText.text = "Tornado";
                break;
            case WeatherTypes.Thunderstorm:
                currentWeatherIcon.sprite = thunderstormIcon;
                weatherText.text = "Thunderstorm";
                break;
            case WeatherTypes.Flood:
                currentWeatherIcon.sprite = floodIcon;
                weatherText.text = "Flood";
                break;

        }
    }
}
