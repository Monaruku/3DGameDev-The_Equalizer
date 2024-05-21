using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeck : MonoBehaviour
{
    public List<Card> deck = new List<Card>();
    public static List<Card> staticDeck = new List<Card>();

    public int x;
    public static int deckSize;
    public int howMuchToDraw;

    public GameObject cardInDeck1;
    public GameObject cardInDeck2;
    public GameObject cardInDeck3;

    public GameObject[] clones;
    public GameObject Hand;
    public GameObject CardToHand;

    // Start is called before the first frame update
    void Start()
    {
        //x = 0;
        //deckSize = 30;
        //for (int i = 0; i < deckSize; i++)
        //{
        //    x = Random.Range(1, 4);
        //    deck[i] = CardDatabase.cardList[x];
        //}
        //StartCoroutine(drawDeck());
    }

    // Update is called once per frame
    void Update()
    {
        staticDeck = deck;

        if (deckSize < 20)
        {
            cardInDeck1.SetActive(false);
        }
        if (deckSize < 5)
        {
            cardInDeck2.SetActive(false);
        }
        if (deckSize < 2)
        {
            cardInDeck3.SetActive(false);
        }

        if (TurnSystem.startTurn == true)
        {
            foreach (Transform child in Hand.transform)
            {
                Destroy(child.gameObject);
            }
            StartCoroutine(drawDeck());
            TurnSystem.startTurn = false;
            Debug.Log(TurnSystem.startTurn);
        }
    }

    public void resetDeck()
    {
        foreach (Transform child in Hand.transform)
        {
            Destroy(child.gameObject);
        }
        x = 0;
        deckSize = 30;
        for (int i = 0; i < deckSize; i++)
        {
            x = Random.Range(1, 4);
            deck[i] = CardDatabase.cardList[x];
        }
        StartCoroutine(drawDeck());
    }
    IEnumerator drawDeck()
    {
        InputSystem.DisableDevice(Mouse.current);
        if (deckSize < 4)
        {
            howMuchToDraw = deckSize;
        }
        else {
            howMuchToDraw = 4;
        }
        for (int i = 0; i < howMuchToDraw; i++)
        {
            yield return new WaitForSeconds(0.5f);

            Instantiate(CardToHand, transform.position, transform.rotation);
        }
        InputSystem.EnableDevice(Mouse.current);
    }
}
