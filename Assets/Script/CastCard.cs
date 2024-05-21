using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CastCard : MonoBehaviour
{

    public int id;
    public string cardName;
    public Image image;
    public int cost;
    public int power;
    public string cardDesc;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public TextMeshProUGUI costText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void initCastCard(DisplayCard card)
    {
        id = card.id;
        cardName = card.cardName;
        image.sprite = card.image.sprite;
        cost = card.cost;
        power = card.power;
        cardDesc = card.cardDesc;

        nameText.text = cardName;
        costText.text = cost.ToString();
        descriptionText.text = cardDesc;
    }
}
