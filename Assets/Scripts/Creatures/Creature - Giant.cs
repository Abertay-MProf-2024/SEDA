using UnityEngine;
using UnityEngine.UI;

public class Giant : Building
{

    [SerializeField] Button satisfybutton;
    Terrainsystem ts1;
    Terrainsystem ts2;

    public GameObject giantcreature;

    [SerializeField] GameObject endpoint1;
    [SerializeField] GameObject endpoint2;


    private void Start()
    {
        RaycastHit hit;
        if (Physics.Raycast(endpoint1.transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            
            Debug.DrawLine(endpoint1.transform.position, endpoint1.transform.position + Vector3.down *100, Color.red, 500);
            ts1 = hit.transform.gameObject.GetComponent<Terrainsystem>();
        }
        
        if (Physics.Raycast(endpoint2.transform.position, Vector3.down, out hit, Mathf.Infinity, LayerMask.GetMask("Terrain")))
        {
            Debug.DrawLine(endpoint2.transform.position, endpoint2.transform.position + Vector3.down * 100, Color.yellow, 500);
            ts2 = hit.transform.gameObject.GetComponent<Terrainsystem>();
        }
        ts1.creaturetype = CreatureTypes.Giant;
        ts2.creaturetype = CreatureTypes.Giant;
    }

    public void Interact()
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
        Inventory.constructionMaterials -=resourceData.bribeCostConstruction;
        
        Destroy(giantcreature);
        satisfybutton.gameObject.SetActive(false);
        ts1.creaturetype = CreatureTypes.None; 
        ts2.creaturetype = CreatureTypes.None;
    }
}