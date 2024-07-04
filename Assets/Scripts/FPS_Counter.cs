using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FPS_Counter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI FPS;

    int frameCounter = 0;
    float timeCounter = 0f;
    float lastFramerate = 0f;
    public float refreshTime = 0.5f;

    private void Start()
    {
        if (refreshTime <= 0f)
        {
            refreshTime = 0.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (timeCounter < refreshTime)
        {
            timeCounter += Time.deltaTime;
            frameCounter++;
        }
        else
        {
            lastFramerate = frameCounter / timeCounter;
            frameCounter = 0;
            timeCounter = 0f;
        }

        print("Frame Counter: " + frameCounter);
        print("Time Counter" + timeCounter);
        print("Last Framerate: " + lastFramerate);

        FPS.text = "FPS: " + Mathf.RoundToInt(lastFramerate);
    }
}
