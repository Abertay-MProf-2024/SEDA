using System.Collections;
using UnityEngine;

public class BuildingPlacementVFX : MonoBehaviour
{
    [SerializeField] Transform vfxTransform;
    [SerializeField] GameObject vfxPrefab;

    GameObject vfxInstance;

    public void PlayVFX()
    {
        vfxInstance = Instantiate(vfxPrefab, vfxTransform);
        vfxInstance.transform.position = vfxTransform.position;
        StartCoroutine(SetDestroyVFX(vfxInstance));
    }

    IEnumerator SetDestroyVFX(GameObject vfx)
    {
        yield return new WaitForSeconds(1);
        Destroy(vfxInstance);
    }
}