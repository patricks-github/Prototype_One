using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Movement : MonoBehaviour {

    Animator BossAnimator;
    NavMeshAgent navMeshAgent;
    public float speedDisplayModifier = 1.0f;

    // Use this for initialization
    void Start()
    {

        BossAnimator = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        BossAnimator.SetFloat("speed", navMeshAgent.speed * speedDisplayModifier);       
    }

    public void AttackAnimation()
    {
        BossAnimator.SetTrigger("attack");
    }
}
