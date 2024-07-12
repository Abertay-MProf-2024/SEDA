using System;
using UnityEngine;
using System.IO;

[Serializable]
public class GameData
{
    public int a;
    public float b;
    public string c;
    public bool d;
}


public class SaveData : MonoBehaviour
{
    

    public void SaveGameData(GameData data)
    {
        // 将数据类序列化为 JSON 字符串
        string json = JsonUtility.ToJson(data);

        // 构建保存文件的路径
        string path = GetFilePath();

        // 将 JSON 字符串写入文件
        File.WriteAllText(path, json);

        Debug.Log("Game data saved to " + path);
    }

    public GameData LoadGameData()
    {
        // 构建保存文件的路径
        string path = GetFilePath();

        // 检查文件是否存在
        if (File.Exists(path))
        {
            // 读取文件内容（JSON 字符串）
            string json = File.ReadAllText(path);

            // 将 JSON 字符串反序列化为数据类实例
            GameData data = JsonUtility.FromJson<GameData>(json);

            //Debug.Log("Game data loaded from " + path);
            return data;
        }
        else
        {
            //Debug.LogWarning("Save file not found at " + path);
            return null;
        }
    }
    private string GetFilePath()
    {
        // 获取相对于 Unity 项目 Assets 目录的文件路径
        return Application.dataPath + "/savefile.json";
    }
}