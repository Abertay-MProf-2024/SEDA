using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : MonoBehaviour
{
    [SerializeField] CreditScreen CreditScreen;

    // Start is called before the first frame update
    void Start()
    {
        TimeSystem.Pause();
    }

    /*
     * Main Menu should always be position 0 in the build order
     */
    public void GoToMainMenu()
    {
        if(CreditScreen)
            Instantiate (CreditScreen.gameObject);
        
    }

    

    // Update is called once per frame
    void OnDestroy()
    {
        //TimeSystem.Unpause();
    }
}
