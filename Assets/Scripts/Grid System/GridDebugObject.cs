using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;
    private GridObject gridObject;
    public TileBase tilebase;
    public Terrainsystem terrainsystem;


    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
        
    }
}
