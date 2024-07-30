using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] AudioClip levelMenuMusic;

    [Header("User Interface")]
    [SerializeField] TextMeshProUGUI timeDisplay;
    [SerializeField] TextMeshProUGUI levelProgressionDisplay;
    [SerializeField] GameObject infoDisplay1;
    [SerializeField] GameObject infoDisplay2;
    [SerializeField] GameObject infoDisplay3;

    // Buttons
    [SerializeField] Button level1Button;
    [SerializeField] Button level2Button;
    [SerializeField] Button level3Button;

    [Header("Loading Screen")]
    [SerializeField] GameObject loadingScreen;
    [SerializeField][TextArea] string[] loadingHintText;

    private LevelSelect instance;

    int levelToLoad = 1;

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
        SceneMusic.instance.ChangeMusicTrack(levelMenuMusic);

        TimeSystem.Pause();

        // Set level buttons based on number of levels completed
        //if (GameManager.levelsCompleted == 1)
        {
            //level1Button.interactable = false;
            level2Button.interactable = true;
            //level3Button.interactable = false;
        }
        //else if (GameManager.levelsCompleted == 2)
        {
            //level1Button.interactable = false;
            //level2Button.interactable = false;
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
            TimeSystem.Unpause();   // Unpause when the level selection menu is closed
        }
    }

    public void DisplayInfo1()
    {
        infoDisplay1.SetActive(true);
    }

    public void DisplayInfo2()
    {
        infoDisplay2.SetActive(true);
    }

    public void DisplayInfo3()
    {
        infoDisplay3.SetActive(true);
    }

    public void SetLevelToLoad(int level)
    {
        levelToLoad = level;
    }

    public void LoadLevel()
    {
        Instantiate(loadingScreen);
        SetRandomHintText();
        Inventory.overworldTime--;
        Inventory.levelTime += 12;
        Inventory.ClearResources();
        SceneManager.LoadSceneAsync(levelToLoad);
    }

    void SetRandomHintText()
    {
        if (loadingHintText.Length > 0)
        {
            int hintNumber = Random.Range(0, loadingHintText.Length);
            loadingScreen.GetComponentInChildren<TextMeshProUGUI>().text = loadingHintText[hintNumber];
        }
    }
}
