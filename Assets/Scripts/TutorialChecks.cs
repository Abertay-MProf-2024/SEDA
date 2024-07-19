using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class TutorialChecks : MonoBehaviour
{
    [SerializeField] GameObject Tutorial1BUTTON;
    [SerializeField] GameObject Tutorial2BUTTON;
    //[SerializeField]  GameObject Tutorial3BUTTON;
    //[SerializeField]  GameObject Tutorial4BUTTON;
    //[SerializeField]  GameObject Tutorial5BUTTON;
    [SerializeField] GameObject Tutorial6BUTTON;
    [SerializeField] GameObject Tutorial7BUTTON;
    //[SerializeField]  GameObject Tutorial8;

    public static bool TapandDrag = false;
    public static bool ZoomInZoomOut = false;
    public static bool TutorialMode = false;

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



}