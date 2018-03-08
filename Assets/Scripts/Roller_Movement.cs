using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Roller_Movement : MonoBehaviour {

    Animator RollerAnimator;
    NavMeshAgent navMeshAgent;
    public float speedDisplayModifier = 0.5f;

    // Use this for initialization
    void Start () {
        speedDisplayModifier = 0.5f;
        RollerAnimator = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        RollerAnimator.SetFloat("speed", speedDisplayModifier);
    }
}
