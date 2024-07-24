using UnityEngine;

public class TutorialChecks : MonoBehaviour
{
    [SerializeField] GameObject Tutorial1BUTTON;
    [SerializeField] GameObject Tutorial2BUTTON;
    //[SerializeField]  GameObject Tutorial3BUTTON;
    //[SerializeField]  GameObject Tutorial4BUTTON;
    //[SerializeField]  GameObject Tutorial5BUTTON;
    [SerializeField] GameObject Tutorial5BUTTON;
    [SerializeField] GameObject Tutorial6BUTTON;
    [SerializeField] GameObject Tutorial7;

    [SerializeField] GameObject BuildMODE;

    [SerializeField] GameObject tutorialSteps;

    public static bool TapandDrag = false;
    public static bool ZoomInZoomOut = false;
    public static bool TutorialMode = false;
    public static bool GiantTalkedTo = false;

    private void Start()
    {
        TutorialMode = true;
    }
    private void Update()
    {
        check();
    }
    public void TurnTutorialModeON()
    {
        TutorialMode = true;

    }

    public void TurnTutorialModeOFF()
    {
        TutorialMode = false;
    }

    public void ResetValues()
    {
        TutorialMode = false;
        tutorialSteps.SetActive(false);
    }


    public void check()
    {
        if(TapandDrag)
        {
            Tutorial1BUTTON.SetActive(true);
        }

        if(ZoomInZoomOut)
        {
            Tutorial2BUTTON.SetActive(true);
        }
    }

    public  void TryBuildLoggingCamp()
    {
        BuildMODE.gameObject.SetActive(false);
        Tutorial5BUTTON.SetActive(true);
    }

    public void StandingStoneBlur()
    {
        Tutorial6BUTTON.SetActive(true);
    }

    public void GiantInteracted()
    {
        GiantTalkedTo = true;
        Tutorial7.SetActive(false);
    }
}
