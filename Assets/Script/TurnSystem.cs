using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnSystem : MonoBehaviour
{
    public bool isYourTurn;
    public bool isEnemyTurn;
    public int yourTurn;
    public int opponentTurn;

    public int maxMana;
    public static int currentMana;
    public int leftoverMana;
    public TextMeshProUGUI manaText;
    public GameObject BattleUI;

    public static bool startTurn;
    // Start is called before the first frame update
    void Start()
    {
        isYourTurn = true;
        yourTurn = 1;
        opponentTurn = 0;


        leftoverMana = 0;

        startTurn = false;
    }

    // Update is called once per frame
    void Update()
    {
        manaText.text = currentMana.ToString();
    }

    public void EndYourTurn()
    {
        isYourTurn = false;
        isEnemyTurn = true;
        leftoverMana = currentMana;
        BattleScript.hasRun = false;
        BattleScript.endRun = false;
        opponentTurn += 1;
        BattleUI.SetActive(false);
    }

    public void EndOpponentTurn()
    {
        isEnemyTurn = false;
        isYourTurn = true;
        yourTurn += 1;
        currentMana = maxMana;
        currentMana += leftoverMana;
        startTurn = true;
        BattleUI.SetActive(true);
    }
}
