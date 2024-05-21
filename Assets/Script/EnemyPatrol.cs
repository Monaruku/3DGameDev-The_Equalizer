using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyPatrol : MonoBehaviour
{

    public BattleScript _battleSystem;
    public GameObject fadeToBlackScreen;
    public AnimationClip fadeIn;
    public AnimationClip fadeOut;
    public NavMeshAgent _agent;
    public Animator _animator;

    private string battleTag = "BattleSystem";
    private string playerTag = "Player";

    // Start is called before the first frame update
    void Start()
    {
        _battleSystem = GameObject.FindGameObjectWithTag(battleTag).GetComponent<BattleScript>();
        _agent = this.GetComponent<NavMeshAgent>();
        _animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Collider[] hitCollider = Physics.OverlapSphere(this.transform.position, 5);
        foreach (var item in hitCollider)
        {
            if (item.gameObject.tag == playerTag)
            {
                _agent.SetDestination(item.gameObject.transform.position);
            }
        }
        if (_agent.velocity.magnitude > 0)
        {
            _animator.SetBool("Walk", true);
        }
        else
        {
            _animator.SetBool("Walk", false);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == playerTag)
        {
            _battleSystem.callStartBattle(true);
            Destroy(this.gameObject);
        }
    }
}
