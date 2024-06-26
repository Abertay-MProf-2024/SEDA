using UnityEngine;
using UnityEngine.UI;

public class Kelpie : Building
{
    [SerializeField] Button satisfybutton;
    Terrainsystem ts1;
    
    public GameObject kelpiecreature;


    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            //Debug.DrawLine(transform.position, transform.position + Vector3.down * 100, Color.red, 500);
            ts1 = hit.transform.gameObject.GetComponent<Terrainsystem>();
        }

        ts1.creaturetype = CreatureTypes.Kelpie;
        kelpiecreature.SetActive(false);
        KelpieImpact();
    }

    void KelpieImpact()
    {

    }

    public void StandingStoneInteract()
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

        Destroy(kelpiecreature);
        satisfybutton.gameObject.SetActive(false);
        ts1.creaturetype = CreatureTypes.None;
    }
}
