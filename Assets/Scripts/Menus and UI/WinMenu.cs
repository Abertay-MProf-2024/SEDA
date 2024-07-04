using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TimeSystem.Pause();
    }

    /*
     * Main Menu should always be position 0 in the build order
     */
    public void GoToMainMenu()
    {
        SceneManager.LoadSceneAsync(0);
    }

    // Update is called once per frame
    void OnDestroy()
    {
        TimeSystem.Unpause();
    }
}
