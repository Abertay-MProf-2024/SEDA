using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditScreen : MonoBehaviour
{
    public void FinishGame()
    {
        SceneManager.LoadSceneAsync(0);
    }
}

