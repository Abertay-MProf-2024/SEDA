using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Put a reference to the Pause_Settings Menu prefab here.")]
    PauseMenu PauseMenuPrefab;

    /** Instantiates a new Pause Menu
     *  The Pause Menu will handle its own destruction
     */
    public void OpenPauseMenu()
    {
        if (PauseMenuPrefab)
        {
            Instantiate(PauseMenuPrefab.gameObject);
        }
    }
}
