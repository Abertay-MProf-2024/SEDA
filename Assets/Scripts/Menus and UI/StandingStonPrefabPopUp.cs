using UnityEngine;

public class StandingStonPrefabPopUp : MonoBehaviour
{
    StandingStone standingStone;
    [SerializeField] GameObject standingStoneBlur;

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
}
