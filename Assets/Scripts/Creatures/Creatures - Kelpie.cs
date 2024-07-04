using UnityEngine;
using UnityEngine.UI;

public class Kelpie : Building
{
    [SerializeField] Button satisfybutton;
    Terrainsystem ts1;
    
    public GameObject kelpiecreature;
    [SerializeField] GameObject endpoint2;


    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(endpoint2.transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            //Debug.DrawLine(transform.position, transform.position + Vector3.down * 100, Color.red, 500);
            ts1 = hit.transform.gameObject.GetComponent<Terrainsystem>();
        }

        ts1.creaturetype = CreatureTypes.Kelpie;
        //kelpiecreature.SetActive(false);
        
    }

    public void StandingStoneKelpieImpact()
    {
        kelpiecreature.SetActive(true);

        foreach (Terrainsystem kelpieTile in FindObjectsByType<Terrainsystem>(FindObjectsSortMode.None))
        {
            if (kelpieTile.terraintype == TerrainTypes.River)
            {
                
                kelpieTile.Wenergy = false;
            }
        }
    }

    public void KelpieInteract()
    {
        Debug.Log("click works");

        if (Inventory.food >= resourceData.bribeCostFood && Inventory.constructionMaterials >= resourceData.bribeCostConstruction)
        {
            satisfybutton.gameObject.SetActive(true);
            Debug.Log(" ENOUGH RESOURCES   " + Inventory.food + resourceData.bribeCostFood);
            Debug.Log(" ENOUGH RESOURCES    " + Inventory.constructionMaterials + resourceData.bribeCostConstruction);
        }
        else
        {
            Debug.Log("NOT ENOUGH RESOURCES   " + Inventory.food + resourceData.bribeCostFood);
            Debug.Log("NOT ENOUGH RESOURCES   " + Inventory.constructionMaterials + resourceData.bribeCostConstruction);

            satisfybutton.gameObject.SetActive(false);
        }
    }


    public void SetCreatureGone()
    {
        Inventory.food -= resourceData.bribeCostFood;
        Inventory.constructionMaterials -= resourceData.bribeCostConstruction;

        foreach (Terrainsystem kelpieTile in FindObjectsByType<Terrainsystem>(FindObjectsSortMode.None))
        {
            if (kelpieTile.terraintype == TerrainTypes.River)
            {
                /* GridPosition pos = giantTile.owningGridObject.GetGridPosition();
                 GridObject CreatureObj = giantTile.owningGridObject.GetOwningGridSystem().GetGridObject(pos.x, pos.z);*/
                kelpieTile.Wenergy = true;
                kelpieTile.TriggerEnergy();
            }
        }
        satisfybutton.gameObject.SetActive(false);
        ts1.creaturetype = CreatureTypes.None;
        Destroy(kelpiecreature);
    }
}
