using UnityEngine;

public class GirdStatus : MonoBehaviour
{

    public bool canBuild ; // gets ref from grid
    public bool Buildmode ;
    void Start()
    {
        
        Buildmode = false;

    }

   /* private void Awake()
    {
        //canBuild = GetComponentInParent<GridDebugObject>().tilebase.canBuild;
    }*/

    
    void Update()
    {
        Color transparentWhite = new Color(1, 1, 1, 0f);
        Color transparentGreen = new Color(0, 1, 0, 0.3f);
        Color transparentRed = new Color(1, 0, 0, 0.3f);

        if (!Buildmode)
        {
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                //material.color = transparentWhite;
        }
        else if (canBuild)
        {

            gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;
            gameObject.GetComponentInChildren<MeshRenderer>().material.color = transparentGreen;
        }
        else
        {
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
            gameObject.GetComponentInChildren<MeshRenderer>().material.color = transparentRed;
        }
    }
}
