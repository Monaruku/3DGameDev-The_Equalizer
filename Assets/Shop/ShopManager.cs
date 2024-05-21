using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public int[,] shopItems = new int[20, 20];
    public float coins;
    public Text coinsTxt;
    private int cumulativePriceIncrease = 50;
    public AudioClip buttonPressSound;  
    private AudioSource audioSource;
    public GameObject ButtonRef;
    public GameObject shopUI;
    public bool potionInteractable;
    public int maxMana;

    public GameObject _player;
    public GameObject _turn;

    private string playerTag = "Player";

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        //ID
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;
        shopItems[1, 4] = 4;
        shopItems[1, 5] = 5;

        //Price
        shopItems[2, 1] = 50;
        shopItems[2, 2] = 20;
        shopItems[2, 3] = 100;
        shopItems[2, 4] = 100;
        shopItems[2, 5] = 100;

        //Quantity
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 100;
        shopItems[3, 4] = 100;
        shopItems[3, 5] = 100;

        ButtonRef = null;
        potionInteractable = true;

        _player = GameObject.FindGameObjectWithTag(playerTag);
        maxMana = 5;
    }

    // Update is called once per frame
    private void Update()
    {
        if (shopUI.activeInHierarchy)
        {
            coinsTxt.text = "Coins: " + coins.ToString();
        }
    }
    public void Buy(GameObject buttonRef)
    {
        ButtonRef = buttonRef;
        ItemInfo itemInfo = ButtonRef.GetComponent<ItemInfo>();
        int itemID = itemInfo.ItemID;

        if ((itemID == 1 || itemID == 2) && shopItems[3, itemID] >= 4)
        {
            Debug.Log("Maximum quantity reached for item ID " + itemID);
            ButtonRef.GetComponent<ClickRestrictedButton>().canClick = false;
        }

        if (coins >= shopItems[2, itemID])
        {
            coins -= shopItems[2, itemID];

            if (itemID == 3)
            {
                shopItems[3, itemID] += 10;
                _player.GetComponent<StarterAssets.ThirdPersonController>().changeAttackModifier(10);

                shopItems[2, itemID] += cumulativePriceIncrease;
                cumulativePriceIncrease += 100;
            }
            else if (itemID == 4)
            {
                shopItems[3, itemID] += 10;
                _player.GetComponent<StarterAssets.ThirdPersonController>().increaseMaxHealth(10);

                shopItems[2, itemID] += cumulativePriceIncrease;
                cumulativePriceIncrease += 100;
            }
            else if (itemID == 5)
            {
                shopItems[3, itemID] += 10;
                maxMana += 1;

                shopItems[2, itemID] += cumulativePriceIncrease;
                cumulativePriceIncrease += 100;
            }
            else
            {
                shopItems[3, itemID]++;
                _player.GetComponent<StarterAssets.ThirdPersonController>().changePotionAmt(1);
            }

            coinsTxt.text = "Coins: " + coins.ToString();
        }
    }



}