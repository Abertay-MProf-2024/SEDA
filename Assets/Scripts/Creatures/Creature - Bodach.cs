using UnityEngine;
using UnityEngine.UI;

public class Cailleach : Building
{
    [SerializeField] Button satisfybutton;
    Terrainsystem TSC1;

    public GameObject cailleachcreature;
    [SerializeField] GameObject endpoint2;


    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(endpoint2.transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            //Debug.DrawLine(transform.position, transform.position + Vector3.down * 100, Color.red, 500);
            TSC1 = hit.transform.gameObject.GetComponent<Terrainsystem>();
        }
        TSC1.creaturetype = CreatureTypes.Cailleach;
        //cailleachcreature.SetActive(false);
    }

    public void StandingStoneCailleachImpact()
    {
        cailleachcreature.SetActive(true);

        // Weather conditions..
        Inventory.CailleachAppeared();

        /*foreach (Terrainsystem kelpieTile in FindObjectsByType<Terrainsystem>(FindObjectsSortMode.None))
        {
            if (kelpieTile.terraintype == TerrainTypes.River)
            {
                kelpieTile.Wenergy = false;
            }
        }*/
    }

    public void CailleachInteract()
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


        // weather is normal
        /*foreach (Terrainsystem cailleachTile in FindObjectsByType<Terrainsystem>(FindObjectsSortMode.None))
        {
            if (cailleachTile.terraintype == TerrainTypes.River)
            {
                *//* GridPosition pos = giantTile.owningGridObject.GetGridPosition();
                 GridObject CreatureObj = giantTile.owningGridObject.GetOwningGridSystem().GetGridObject(pos.x, pos.z);*//*
                cailleachTile.Wenergy = true;
            }
        }*/

        Destroy(cailleachcreature);
        satisfybutton.gameObject.SetActive(false);
        TSC1.creaturetype = CreatureTypes.None;
        
    }
    
    

}
