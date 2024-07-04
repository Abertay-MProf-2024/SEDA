using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private SaveData dataManager;
    private GameData gameData;

    public static int levelsCompleted = 0;

    public bool check;
    public bool isread;
    public bool issave;

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
    }
    
    // 示例：更新游戏数据的方法
    public void UpdateGameData(int newScore, float newVolume, string newName,bool isss)
    {
        gameData.a = newScore;
        gameData.b = newVolume;
        gameData.c = newName;
        gameData.d = isss;
    }

    private void OnApplicationQuit()
    {
        // 在应用退出时保存游戏数据
        dataManager.SaveGameData(gameData);
    }
}
