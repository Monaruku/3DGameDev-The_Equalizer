using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using static UnityEditor.Progress;

public class ItemInfo : MonoBehaviour
{
    public int ItemID;
    public TextMeshProUGUI Price;
    public TextMeshProUGUI Quantity;
    public GameObject ShopManager;


    // Update is called once per frame
    void Update()
    {
        Price.text = "Price: $" + ShopManager.GetComponent<ShopManager>().shopItems[2, ItemID].ToString();

        if (ItemID == 3 || ItemID == 4 || ItemID == 5)
        {
            Quantity.text = "" + ShopManager.GetComponent<ShopManager>().shopItems[3, ItemID].ToString();
        }
        else
        {
            Quantity.text = ShopManager.GetComponent<ShopManager>().shopItems[3, ItemID].ToString();
        }

        if (ShopManager.GetComponent<ShopManager>().coins < ShopManager.GetComponent<ShopManager>().shopItems[2, ItemID])
        {
            this.GetComponent<Button>().interactable = false;
        }
    }
}
