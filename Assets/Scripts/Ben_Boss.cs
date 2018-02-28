using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ben_Boss : MonoBehaviour
{   // Use this for initialization
    protected NavMeshAgent nav;               // Reference to the nav mesh agent.
    protected bool Combat = false;
    public Transform[] points;
    protected int destPoint = 0;
    protected float CombatTimer = 0.0f;
    protected float ChargeTimer = 0.0f;
    public Transform target;
    protected Vector3 ChargeDestinationVector;
    protected Vector3 targetDir;
    protected float angle;
    public bool StartCharge = false;
    

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }


    void Start()
    {
        nav.autoBraking = false;
    }


    public void Update()
    {
        targetDir = target.position - this.gameObject.transform.position;
        angle = Vector3.Angle(targetDir, this.gameObject.transform.forward);

        CombatTimer += Time.deltaTime;

        if (angle < 20.0f)
        {
            Debug.Log("Called Combat");
            Combat = true;
        }

        if (Combat && (CombatTimer < 8.0f))
        {
            if (Combat && (CombatTimer > 3.0f))
            {
                StartCharge = true;
                Charge(target.position);
            }
        }
        else if (Combat && (CombatTimer > 8.0f))
        {
            Combat = false;
            CombatTimer = 0.0f;
            ChargeTimer = 0.0f;

            GotoNextPoint();
        }
        else
        {
            if (!nav.pathPending && nav.remainingDistance < 1.0f)
            {
                nav.speed = 4.0f;
                nav.isStopped = false;

                GotoNextPoint();
            }
        }
    }


    void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.tag == "Player")
        {
            //end game here
        }
        if (_collision.gameObject.tag == "BossWall")
        {
            CombatTimer = 0.0f;
            nav.speed = 4.0f;
        }
    }


    public void GotoNextPoint()
    {   // Returns if no points have been set up
        if (points.Length == 0)
        {
            return;
        }
        // Set the agent to go to the currently selected destination.
        nav.destination = points[destPoint].position;

        // Choose the next point in the array as the destination,
        // cycling to the start if necessary.
        destPoint = (destPoint + 1) % points.Length;
    }

    //yeet
    void Charge(Vector3 _ChargeLoc)
    {
        if (StartCharge)
        {
            ChargeTimer += Time.deltaTime;
            CombatTimer = 0.0f;

            Debug.Log("CHARGE");

            nav.speed = 16.0f;

            ChargeDestinationVector = (target.position - this.gameObject.transform.position);
            ChargeDestinationVector = ChargeDestinationVector.normalized * 500.0f;
            nav.SetDestination(ChargeDestinationVector);

            if (ChargeTimer > 1.0f)
            {
                ChargeTimer = 0.0f;
                //ChargeDestinationVector = this.gameObject.transform.position;
                StartCharge = false;
            }
        }
    }
}