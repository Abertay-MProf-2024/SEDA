using System.Collections;
using TMPro;
using UnityEngine;

public class ResourceUpdatePopup : MonoBehaviour
{
    [SerializeField] TextMeshPro text;

    public void AnimatePopup()
    {
        text.gameObject.SetActive(true);
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveUp()
    {
        int count = 0;

        while (count < 60)
        {
            yield return new WaitForSeconds(0.01f);
            text.transform.position += new Vector3(0, (0.01f), 0);
            count++;
        }

        text.gameObject.SetActive(false);
    }
}
