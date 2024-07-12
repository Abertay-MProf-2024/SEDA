using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // The active instance of the pause menu
    private static PauseMenu instance;

    /** The static property GameIsPaused can be accessed by any class to determine whether the game is paused. */
    private static bool gameIsPaused = false;
    public static bool GameIsPaused { get { return gameIsPaused; } }

    [SerializeField]
    [Tooltip("The name of the Main Menu scene.")]
    public string mainMenuSceneName = "UI Screen";


    /** The pause menu is a singleton
     *  There is only one pause menu active at any given time
     */
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameIsPaused = true;
        TimeSystem.Pause();    // Pause the game when the pause menu is opened
    }

    /** Destroy the pause menu object */
    public void ClosePauseMenu()
    {
        Destroy(gameObject);
    }

    /** Exit the current scene and load the the Main Menu
     *  If the pause menu has been opened inside the Main Menu, simply close the pause menu
     */
    public void ReturnToMainMenu()
    {
            SceneManager.LoadSceneAsync(mainMenuSceneName);
    }

    /** Quit the game */
    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    /** When the active instance of the pause menu is closed, the game will unpause */
    private void OnDestroy()
    {
        if (instance == this)
        {
            gameIsPaused = false;
            TimeSystem.Unpause();    // Unpause when the pause menu is closed
        }
    }
}
