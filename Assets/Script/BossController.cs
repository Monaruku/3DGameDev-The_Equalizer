using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public GameObject target;
    public int moveSpeed;
    public int rotationSpeed;
    public Animator _animator;
    public Vector3 myPosition;
    public bool isMoving;
    public GameObject _turn;
    public GameObject _shop;

    private int id;
    private NavMeshAgent _agent;
    private GameObject minionPos;

    public BattleScript _battleSystem;
    public GameObject _healthbar;

    //health system
    public int health;
    public int maxHealth;

    private string playerTag = "Player";
    private string turnName = "TurnSystem";
    private string battleTag = "BattleSystem";
    private string healthbarTag = "BossHealthbar";

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag(playerTag);
        _animator = GetComponent<Animator>();
        maxHealth = 400;
        health = maxHealth;
        _turn = GameObject.Find(turnName);
        _battleSystem = GameObject.FindGameObjectWithTag(battleTag).GetComponent<BattleScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_agent.velocity.magnitude > 0)
        {
            _animator.SetBool("Walk", true);
            isMoving = true;
        }
        else
        {
            _animator.SetBool("Walk", false);
            isMoving = false;
        }
        if (health <= 0)
        {
            _animator.SetBool("Death", true);
            _battleSystem.onEndOnce = true;
            StartCoroutine(finishAnim());
        }
    }

    public void moveCharacter(Vector3 destination)
    {
        _agent.SetDestination(destination);
        transform.eulerAngles = new Vector3(
        transform.eulerAngles.x,
        transform.eulerAngles.y + 180,
        transform.eulerAngles.y
        );
    }

    public void changeHealth(int amt)
    {
        health += amt;
    }

    public void onTurn(Vector3 myPos, int id)
    {
        myPosition = myPos;
        this.id = id;
        _agent.SetDestination(target.transform.position + Vector3.forward * 1f);
        StartCoroutine(attack());

    }
    public void attackPlayer()
    {
        if (BattleScript.totalEncounter <= 1)
        {
            target.GetComponent<StarterAssets.ThirdPersonController>().takeDamage(-250);
        }
        else
        {
            target.GetComponent<StarterAssets.ThirdPersonController>().takeDamage(-50);
        }
    }

    IEnumerator finishAnim()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
        _battleSystem.startVictoryScreen();
    }

    IEnumerator attack()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => isMoving == false);
        transform.eulerAngles = new Vector3(0, -180, 0);
        _animator.Play("Base Layer.Attack", 0, 0f);
        yield return new WaitWhile(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f);
        yield return new WaitForSeconds(2.8f);
        moveCharacter(myPosition);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => isMoving == false);
        transform.eulerAngles = new Vector3(0, -180, 0);
        BattleScript.hasRun = false;
        BattleScript.readyToEnd = true;
    }
}
