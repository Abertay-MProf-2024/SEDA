using UnityEngine;

public class LevelComplete : MonoBehaviour
{
    // UI prefab
    [SerializeField] GameObject levelSelectPrefab;

    private void Start()
    {
        TimeSystem.Pause();
    }

    public void OpenLevelSelectScreen()
    {
        Instantiate(levelSelectPrefab);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        TimeSystem.Unpause();
    }
}
