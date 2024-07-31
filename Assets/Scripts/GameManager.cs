using UnityEngine;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private SaveData dataManager;
    private GameData gameData;

    public static int levelsCompleted = 0;

    public bool check;
    public bool isread;
    public bool issave;

    [SerializeField] AudioMixer mixer;

    public static bool isTutorialComplete = false;

    /*
     * There can only be one Game Manager.
     * It persists throughout a single session of gameplay.
     * When a new Game Manager is created, it always replaces the old one.
     * This means that a New Game has been started.
     */

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
        dataManager = GetComponent<SaveData>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        
        gameData = dataManager.LoadGameData();
        
        dataManager.SaveGameData(gameData);

        SetInitialAudioVolume("masterVolume");
        SetInitialAudioVolume("musicVolume");
        SetInitialAudioVolume("sfxVolume");
    }
    
    // 示例：更新游戏数据的方法
    public void UpdateGameData(int newScore, float newVolume, string newName,bool isss)
    {
        gameData.a = newScore;
        gameData.b = newVolume;
        gameData.c = newName;
        gameData.d = isss;
    }

    void SetInitialAudioVolume(string mixerGroup)
    {
        if (PlayerPrefs.HasKey(mixerGroup))
        {
            mixer.SetFloat(mixerGroup, GetDecibelLevel(PlayerPrefs.GetFloat(mixerGroup)));
        }
        else
        {
            mixer.SetFloat(mixerGroup, GetDecibelLevel(0.5f));
        }
    }

     /* 
     * Get the decibel level from a float with a range of 0 to 1
     * This equation remaps the volume value so that from 0 to 1 maps to a decibel level of -40 to 0
     * Equation: output = output_start + ((output_end - output_start) / (input_end - input_start)) * (input - input_start)
     */
    public float GetDecibelLevel(float volume)
    {
        if (volume < 0.1f)
            return -80;

        return -40 + (10 + 40) * volume;
    }

    private void OnApplicationQuit()
    {
        // 在应用退出时保存游戏数据
        dataManager.SaveGameData(gameData);
    }
}
