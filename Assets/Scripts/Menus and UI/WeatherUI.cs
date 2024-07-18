using TMPro;
using UnityEngine;

public class WeatherUI : MonoBehaviour
{
    [SerializeField] Sprite currentWeatherIcon;

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
                currentWeatherIcon = fairWeatherIcon;
                break;
            case WeatherTypes.Tornado:
                currentWeatherIcon = tornadoIcon;
                break;
            case WeatherTypes.Thunderstorm:
                currentWeatherIcon = thunderstormIcon;
                break;
            case WeatherTypes.Flood:
                currentWeatherIcon = floodIcon;
                break;

        }
    }
}
