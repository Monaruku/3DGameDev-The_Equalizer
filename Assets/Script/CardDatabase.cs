using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDatabase : MonoBehaviour
{
    public static List<Card> cardList = new List<Card>();
    public Sprite noneSprite;
    public Sprite attackSprite;
    public Sprite blockSprite;
    public Sprite SpecialSprite;

    void Awake()
    {
        cardList.Add(new Card(0, "None", noneSprite, 0, 0, "None"));
        cardList.Add(new Card(1, "Attack", attackSprite, 2, 1, "Attack the enemy"));
        cardList.Add(new Card(2, "Block", blockSprite, 1, 1, "Block an attack"));
        cardList.Add(new Card(3, "Special", SpecialSprite, 3, 1, "Strong attack"));
    }
}
