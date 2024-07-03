using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timeDisplay;
    [SerializeField] TextMeshProUGUI levelProgressionDisplay;
    [SerializeField] GameObject infoDisplay;
    
    // Buttons
    [SerializeField] Button level1Button;
    [SerializeField] Button level2Button;
    [SerializeField] Button level3Button;

    private LevelSelect instance;

    /** The level select menu is a singleton
     *  There is only one level selection menu active at any given time
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

    // Start is called before the first frame update
    void Start()
    {
        TimeSystem.Pause();

        // Set level buttons based on number of levels completed
        if (GameManager.levelsCompleted == 1)
        {
            level1Button.interactable = false;
            level2Button.interactable = true;
            level3Button.interactable = false;
        }
        else if (GameManager.levelsCompleted == 2)
        {
            level1Button.interactable = false;
            level2Button.interactable = false;
            level3Button.interactable = true;
        }
    }

    private void Update()
    {
        timeDisplay.text = "Time Left: " + Inventory.overworldTime.ToString() + " Years";
        levelProgressionDisplay.text = "Levels Completed: " + GameManager.levelsCompleted.ToString() + " / 3";
    }

    /** When the active instance of the level select menu is closed, game time will resume */
    private void OnDestroy()
    {
        if (instance == this)
        {
            Time.timeScale = 1f;    // Unpause when the level selection menu is closed
        }
    }

    public void DisplayInfo()
    {
        infoDisplay.SetActive(true);
    }

    public void LoadLevel()
    {
        Inventory.overworldTime--;
        Inventory.levelTime += 12;
        Inventory.ClearResources();
        SceneManager.LoadSceneAsync(GameManager.levelsCompleted + 1);
    }
}
