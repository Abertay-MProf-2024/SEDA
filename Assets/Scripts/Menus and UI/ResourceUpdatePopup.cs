using System.Collections;
using TMPro;
using UnityEngine;

public class ResourceUpdatePopup : MonoBehaviour
{
    [SerializeField] TextMeshPro textBox;
    [SerializeField] SpriteRenderer icon;

    [Space(10)]
    [Header("Sprites")]
    [SerializeField] Sprite foodSprite;
    [SerializeField] Sprite materialSprite;

    Building buildingRef;
    Vector3 initialPopupLocation;

    private void Start()
    {
        buildingRef = gameObject.GetComponentInParent<Building>();
        SetVisibility(false);

        Camera cam = FindAnyObjectByType<Camera>();
        Quaternion lookAtCamRotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        transform.rotation = lookAtCamRotation;

        initialPopupLocation = transform.position;
    }

    public void AnimatePopup()
    {
        if (buildingRef.GetFoodGenerated() > 0)
        {
            textBox.text = "+" + buildingRef.GetFoodGenerated();
            icon.sprite = foodSprite;
        }
        else
        {
            textBox.text = "+" + buildingRef.GetMaterialsGenerated();
            icon.sprite = materialSprite;
        }

        SetVisibility(true);
        StartCoroutine(MoveUp());
    }

    IEnumerator MoveUp()
    {
        int count = 0;

        while (count < 60)
        {
            yield return new WaitForSeconds(0.01f);
            textBox.transform.position += new Vector3(0, (0.01f), 0);
            count++;
        }

        SetVisibility(false);
        transform.position = initialPopupLocation;
    }

    void SetVisibility(bool isVisible)
    {
        GetComponent<MeshRenderer>().enabled = icon.enabled = isVisible;
    }
}
