using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
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
    public ParticleSystem hitBlood;
    public ParticleSystem deathPuff;
    public GameObject goblinBody;
    public GameObject goblinPart;
    private bool runOnce;

    //health system
    public int health;

    public int damage;

    private string playerTag = "Player";
    private string turnName = "TurnSystem";
    private string shopTag = "Shop";

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>(); 
    }
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag(playerTag);
        _animator = GetComponent<Animator>();
        health = 100;
        damage = -10;
        runOnce = false;
        _turn = GameObject.Find(turnName);
        _shop = GameObject.FindGameObjectWithTag(shopTag);
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
        if (health <= 0 && !runOnce)
        {
            _animator.Play("Base Layer.Death", 0, 0);
            StartCoroutine(finishAnim());
            runOnce = true;
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
        hitBlood.Play();
    }

    public void onTurn(Vector3 myPos, int id)
    {
        myPosition = myPos;
        this.id = id;
        _agent.SetDestination(target.transform.position + Vector3.forward);
        StartCoroutine(attack());

    }
    public void attackPlayer()
    {
        if (BattleScript.totalEncounter == 1)
        {
            target.GetComponent<StarterAssets.ThirdPersonController>().takeDamage(-5);
        }
        else if (BattleScript.totalEncounter == 2)
        {
            target.GetComponent<StarterAssets.ThirdPersonController>().takeDamage(-10);
        }
        else if (BattleScript.totalEncounter >= 2)
        {
            target.GetComponent<StarterAssets.ThirdPersonController>().takeDamage(-15);
        }
    }

    IEnumerator finishAnim()
    {
        yield return new WaitForSeconds(0.6f);
        goblinBody.SetActive(false);
        goblinPart.SetActive(false);
        deathPuff.Play();
        yield return new WaitForSeconds(0.5f);
        if (BattleScript.attackPos == 1)
        {
            BattleScript.enemyPos1 = false;
        }
        if (BattleScript.attackPos == 2)
        {
            BattleScript.enemyPos2 = false;
        }
        Destroy(this.gameObject);
    }

    IEnumerator attack()
    {
        yield return new WaitForSeconds(0.2f);
        yield return new WaitUntil(() => isMoving == false);
        transform.eulerAngles = new Vector3(0, -180, 0);
        _animator.Play("Base Layer.Attack", 0, 0f);
        yield return new WaitWhile(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.5f);
        yield return new WaitForSeconds(1f);
        moveCharacter(myPosition);
        yield return new WaitForSeconds(0.5f);
        yield return new WaitUntil(() => isMoving == false);
        transform.eulerAngles = new Vector3(0, -180, 0);
        if (id == 1)
        {
            BattleScript.goblinFirst = true;
            BattleScript.goblinNext = true;
            BattleScript.hasRun = false;
        }
        else if (id == 2)
        {
            BattleScript.hasRun = false;
            BattleScript.readyToEnd = true;
        }
    }
}
