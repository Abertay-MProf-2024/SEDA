using UnityEngine;

[ExecuteAlways]
public class GridSystem : MonoBehaviour
{
    [SerializeField] int gridLength = 1;
    [SerializeField] int gridWidth = 1;
    [SerializeField] float gridHeight = 0f;
    [SerializeField] int cellSize = 1;

    [SerializeField] GridObject gridObjectPrefab;

    GridObject[,] gridTiles;

    private void OnEnable()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        ClearGrid();
        gridTiles = new GridObject[gridLength, gridWidth];
        CreateGrid();
    }

    private void ClearGrid()
    {
        gridTiles = null;
        for (int i = gameObject.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(gameObject.transform.GetChild(i).gameObject);
        }
    }

    private void CreateGrid()
    {
        Vector3 tileRotation = new Vector3(90, 0, 0);
        

        for (int x = 0; x < gridLength; x++)
        {
            for (int y = 0;  y < gridWidth; y++)
            {
                gridTiles[x, y] = Instantiate(gridObjectPrefab, new Vector3(gameObject.transform.position.x + x * cellSize, gridHeight, gameObject.transform.position.z + y * cellSize), Quaternion.Euler(tileRotation));
                gridTiles[x, y].transform.localScale = new Vector3(cellSize, cellSize, cellSize);
                gridTiles[x, y].transform.parent = gameObject.transform;
                gridTiles[x, y].SetOwningGridSystem(this);
            }
        }
    }

    public GridObject GetGridObject(int x, int z)
    {
        if (x >= 0 && z >= 0 && x < GetGridLength() && z < GetGridWidth())
            return gridTiles[x, z];
        else return null;
    }

    public int GetGridLength() { return gridLength; }
    public int GetGridWidth() {  return gridWidth; }

    public void ToggleBuildMode(TileBase buildingType, bool BuildModeOn)
    {
       
        BuildSystem.isInBuildMode = BuildModeOn;

        for (int x = 0; x < gridLength; x++)
        { 
            for (int y = 0; y < gridWidth; y++)
            {
                gridTiles[x, y].ToggleBuildModePerTile(buildingType);
            }
        }
    }

    public int GetCellSize()
    { return cellSize; }
}
