using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Roller_Movement : MonoBehaviour {

    Animator RollerAnimator;
    NavMeshAgent navMeshAgent;
    public float speedDisplayModifier = 1.0f;

    // Use this for initialization
    void Start () {
        RollerAnimator = gameObject.GetComponent<Animator>();
        navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        RollerAnimator.SetFloat("speed", speedDisplayModifier);
    }
}
