// Remy Pijuan, 2024

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
    }

    private void OnDestroy()
    {
        TimeSystem.Unpause();
    }
}
