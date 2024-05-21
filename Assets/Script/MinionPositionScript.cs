using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionPositionScript : MonoBehaviour
{
    public int posNum;
    public BattleScript _battleSystem;
    public GameObject spotLight;
    public bool enableScript;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if (enableScript)
        {
            spotLight.SetActive(true);
            _battleSystem.showEnemyHealthBar(posNum);
        }
    }

    private void OnMouseDown()
    {
        if (enableScript)
        {
            _battleSystem.mouseOverAction(posNum);
            spotLight.SetActive(false);
        }
    }

    private void OnMouseExit()
    {
        if (enableScript)
        {
            spotLight.SetActive(false);
            _battleSystem.unshowEnemyHealthBar(posNum);
        }
    }
}
