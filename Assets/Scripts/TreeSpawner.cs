using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TreeSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject TreeTile;

    [SerializeField]
    Transform InitialLocation;

    Vector3 Location;
    Vector3 Rotation;

    int widthLimit = 13;

    private void Awake()
    {
        Location = InitialLocation.position;
        Rotation = new Vector3(0, 0, 0);
    }

    public void SpawnTree()
    {
        Location.z += 2;

        if (Location.z > widthLimit * 2)
        {
            Location.z = 0;
            Location.x -= 2;
        }

        Instantiate(TreeTile, Location, Quaternion.Euler(Rotation));
    }
}
