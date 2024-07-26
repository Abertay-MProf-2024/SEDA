using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private void Start()
    {
        TimeSystem.Pause();
    }

    /*
     * Restart the current level
     */
    public void RestartLevel()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        TimeSystem.Unpause();
        WeatherSystem.hasFloodHappened = false;
        Terrainsystem.tilecount = 0;
        //BuildingTypeSelect.GridOff();
    }

    private void OnDestroy()
    {
        TimeSystem.Unpause();
    }
}
