using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private SaveData dataManager;
    private GameData gameData;

    public static int levelsCompleted = 0;

    public int aaa;
    public float bbb;
    public string ccc;
    public bool ddd;

    public int aa;
    public float bb;
    public string cc;
    public bool dd;

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
        // 尝试加载游戏数据
        gameData = dataManager.LoadGameData();
        if (gameData == null)
        {
            // 如果没有找到保存的数据，初始化新的数据
            gameData = new GameData
            {
                a = 4,
                b = 11.2f,
                c = "ssss",
                d = true
            };
        }
        dataManager.SaveGameData(gameData);
    }
    private void Update()
    {
        debugnum();
        if (check&&issave )
        {
            gameData.a = aaa;
            gameData.b = bbb;
            gameData.c = ccc;
            gameData.d = ddd;
        }
        else if(check && isread)
        {
            UpdateGameData(aaa, bbb, ccc, ddd);
        }
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
    void debugnum()
    {
        aa = gameData.a;
        bb = gameData.b;
        cc = gameData.c;
        dd = gameData.d;    
    }
}
