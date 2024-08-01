using UnityEngine;

public class StandingStonPrefabPopUp : MonoBehaviour
{
    // The active instance of the standing stone menu instance
    private static StandingStonPrefabPopUp instance;

    StandingStone standingStone;
    [SerializeField] GameObject standingStoneBlur;

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

    private void Start()
    {
        if (TutorialChecks.TutorialMode)
        {
            standingStoneBlur.SetActive(false);
        }
    }

    public void SetStandingStoneReference(StandingStone stone)
    {
        standingStone = stone;
    }

    public void SwitchIsland()
    {
        standingStone.VeilSwitch();
        CloseMenu();
    }

    public void CloseMenu()
    {
        Destroy(gameObject);
    }

    public void BlurTutorial()
    {
        if (TutorialChecks.TutorialMode)
        {
            TutorialChecks tutorialChecksObject = FindAnyObjectByType<TutorialChecks>();

            if (tutorialChecksObject != null)
            {
                tutorialChecksObject.StandingStoneBlur();
            }
        }
    }
    private void OnDestroy()
    {
        if (instance == this)
        {
            //TimeSystem.Unpause();    // Unpause when the standing stone 
        }
    }
}
