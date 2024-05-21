using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class Card
{
    public int id;
    public string cardName;
    public Sprite sprite;
    public int cost;
    public int power;
    public string cardDesc;

    public Card()
    {

    }

    public Card(int id, string cardName, Sprite sprite, int cost, int power, string cardDesc )
    {
        this.id = id;
        this.cardName = cardName;
        this.sprite = sprite;
        this.cost = cost;
        this.power = power;
        this.cardDesc = cardDesc;
    }
}
