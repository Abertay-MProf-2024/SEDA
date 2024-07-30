using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryBoard : MonoBehaviour
{
    [SerializeField] GameObject Storyboard1;
    [SerializeField] GameObject Storyboard2;
    [SerializeField] GameObject Storyboard3;

    public static bool StoryBoarddone = false;

    //to turn off the storyboards, once played
    void Start()
    {
        if (StoryBoarddone)
        {
            Storyboard1.SetActive(false);
            Storyboard2.SetActive(false);
            Storyboard3.SetActive(false);
        }
    }

    public void TurnItOff()
    {
       StoryBoarddone=true;
    }

   
    
}
