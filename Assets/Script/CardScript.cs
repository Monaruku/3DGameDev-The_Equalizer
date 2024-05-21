using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    public GameObject Hand;
    public GameObject HandCard;

    public DisplayCard _display;
    public GameObject castUI;
    public GameObject castCard;
    public GameObject battleUI;
    public BattleScript battleSystem;

    //Find GameObject String
    private string playerHandString = "PlayerHand";
    private string battleSystemString = "BattleCollections";
    // Start is called before the first frame update
    void Start()
    {
        _display = this.GetComponent<DisplayCard>();
        Hand = GameObject.Find(playerHandString);
    }

    // Update is called once per frame
    void Update()
    {
        HandCard.transform.SetParent(Hand.transform, false);
    }

    public void attack()
    {
        battleSystem = GameObject.Find(battleSystemString).GetComponent<BattleScript>();
        switch (_display.id)
        {
            case 1:
                if (TurnSystem.currentMana >= 2)
                {
                    battleSystem.enterCastUI(_display.id, this.gameObject);
                }
                else
                {
                    battleSystem.showNoMana();
                }
                break;
            case 2:
                if (TurnSystem.currentMana >= 1)
                {
                    battleSystem.enterGuardUI(_display.id, this.gameObject);
                }
                else
                {
                    battleSystem.showNoMana();
                }
                break;
            case 3:
                if (TurnSystem.currentMana >= 3)
                {
                    battleSystem.enterCastUI(_display.id, this.gameObject);
                }
                else
                {
                    battleSystem.showNoMana();
                }
                break;
            default:
                break;
        }
    }
}
