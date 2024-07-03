// Remy Pijuan 2024

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

    //[SerializeField]
    //PlayerInput PlayerInput;

    InputAction possessCameraAction;
    InputAction cameraPanAction;
    InputAction unpossessCameraAction;
    InputAction zoomAction;
    
    public InputActionAsset action;

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
        Time.timeScale = 0f;    // Pause the game when the pause menu is opened

        possessCameraAction = action.FindAction("PossessCamera");
        cameraPanAction = action.FindAction("PanCamera");
        unpossessCameraAction = action.FindAction("UnpossessCamera");
        zoomAction = action.FindAction("MouseWheelZoom");

        if(possessCameraAction != null)
            possessCameraAction.Disable();
        if(cameraPanAction != null)
            cameraPanAction.Disable();
        if(unpossessCameraAction != null)
            unpossessCameraAction.Disable();
        if(zoomAction != null)
            zoomAction.Disable();

        //PlayerInput.SwitchCurrentActionMap("Pause Menu");
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
            Time.timeScale = 1f;    // Unpause when the pause menu is closed

            possessCameraAction.Enable();
            cameraPanAction.Enable();
            unpossessCameraAction.Enable();
            zoomAction.Enable();
            //PlayerInput.SwitchCurrentActionMap("Camera");
        }
    }
}
