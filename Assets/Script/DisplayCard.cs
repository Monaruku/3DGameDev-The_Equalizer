using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DisplayCard : MonoBehaviour
{
    public Card displayCard;
    public int displayId;

    public int id;
    public string cardName;
    public Image image;
    public int cost;
    public int power;
    public string cardDesc;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;

    public GameObject cardBack;
    public bool displayCardBack;

    public GameObject Hand;
    public int numberOfCardsInDeck;


    // Start is called before the first frame update
    void Start()
    {
        numberOfCardsInDeck = PlayerDeck.deckSize;
        Hand = GameObject.Find("PlayerHand");
        displayCardBack = true;
        displayCard = CardDatabase.cardList[displayId];
    }

    // Update is called once per frame
    void Update()
    {
        id = displayCard.id;
        cardName = displayCard.cardName;
        image.sprite = displayCard.sprite;
        cost = displayCard.cost;
        power = displayCard.power;
        cardDesc = displayCard.cardDesc;

        nameText.text = cardName;
        costText.text = cost.ToString();
        descriptionText.text = cardDesc;

        if (this.tag == "Clone")
        {
            displayCard = PlayerDeck.staticDeck[numberOfCardsInDeck - 1];
            numberOfCardsInDeck -= 1;
            PlayerDeck.deckSize -= 1;
            displayCardBack = false;
            this.tag = "Untagged";
        }
    }
}
