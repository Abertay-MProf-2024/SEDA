using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIFuntions : MonoBehaviour
{
    // Game Manager prefab
    [SerializeField] GameObject gameManager;

    //Main Menu Canvases
    public Canvas MainMenuCanvas;
    public Canvas LoadMenuCanvas;
    //public Canvas PauseMenuCanvas;

    //Level Selection Canvases
    public Canvas LevelSelection;
    public GameObject MapOverview;
    public GameObject Level1_Info;
    public GameObject ACInfo;

    // Text boxes
    public TextMeshProUGUI timeResource;

    

    // Start is called before the first frame update
    void Start()
    {
        MainMenuCanvas.enabled = true;
        LoadMenuCanvas.enabled = false;
        //PauseMenuCanvas.enabled = false;

        LevelSelection.enabled = false;
        MapOverview.SetActive(false);
    }

    public void Load_clickhere()
    {
        MainMenuCanvas.enabled = true;
        LoadMenuCanvas.enabled = false;
        //PauseMenuCanvas.enabled = false;

        LevelSelection.enabled = false;
        MapOverview.SetActive(false);
    }

    public void Settings_clickhere()
    {
        //PauseMenuCanvas.enabled = false;
    }
    
    public void NewLevel()
    {
        MainMenuCanvas.enabled = false;

        LevelSelection.enabled = true;
        MapOverview.SetActive(true);
        Level1_Info.SetActive(false);
        ACInfo.SetActive(false);

        // Instantiate a new Game Manager (complete with Save Data!)
        Instantiate(gameManager);
    }

    public void LoadLevel()
    {
        MainMenuCanvas.enabled = false ;
        LoadMenuCanvas.enabled = true ;
        //PauseMenuCanvas.enabled = false ;
    }
    public void Settings()
    {
       // PauseMenuCanvas.enabled = true;
        //MainMenuCanvas.enabled = false;
        LoadMenuCanvas.enabled = false;
    }

    public void PlayGame()
    {
        Inventory.overworldTime--;
        Inventory.levelTime += 36;
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }


    // Level Selection Menus

    public void Level1()
    {
        Level1_Info.SetActive(true);
    }

    public void Level1_clickhere()
    {
        Level1_Info.SetActive(false);
    }

    public void ACButton()
    {
        ACInfo.SetActive(true);
    }

    public void AC_clickhere()
    {
        ACInfo.SetActive(false);
    }
}
