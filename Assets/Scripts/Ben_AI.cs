using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ben_AI : MonoBehaviour
{
    // Use this for initialization
    protected NavMeshAgent nav;               // Reference to the nav mesh agent.
    protected bool Aggro = false;
    protected bool Combat = false;
    public Transform[] points;
    protected int destPoint = 0;
    protected float WaitTimer = 0.0f;
    protected float CombatTimer = 0.0f;
    protected float TargetTimer = 0.0f;
    public Transform target;
    protected Vector3 ChargeDestinationVector;
    protected Vector3 targetDir;
    protected Vector3 targetLocOld;
    protected float angle;
    protected float RandomWait;
    void Awake()
    {
        RandomWait = Random.Range(15.0f, 25.0f);
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
        WaitTimer += Time.deltaTime;
        CombatTimer += Time.deltaTime;
        TargetTimer += Time.deltaTime;

        if (TargetTimer > 2.5f)
        {
            targetLocOld = target.position;
        }

        if (angle < 5.0f)
        {
            Debug.Log("Called Aggro");
            Aggro = true;
        }
        else
        {
            Aggro = false;
        }

        if (targetDir.x < 10.0f && targetDir.z < 10.0f && Aggro)
        {            
            Combat = true;
            Aggro = false;
        }

        if (Combat && (CombatTimer > 3.0f))
        {
            CombatTimer = 0.0f;
            nav.speed += 0.5f;
            nav.SetDestination(targetLocOld);
            /*this code here is for if the AI runs into the player. it can also be run in the collide, but thats been a bit janky
             * if (target.position == this.gameObject.transform.position)
            {
                this.gameObject.transform.position = this.gameObject.transform.position.normalized * 2.5f;
            }*/
        }
        else if (Aggro)
        {
            nav.SetDestination(targetLocOld);
        }
        else
        {
            if (!nav.pathPending && nav.remainingDistance < 1.0f)
            {
                if (nav.remainingDistance < 1.5f)
                {
                    nav.isStopped = true;
                }
                if (WaitTimer >= RandomWait)
                {
                    GotoNextPoint();
                }
            }
        }

    }
    void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.tag == "Player")
        {
            //end game here? or just damage player
            //this.gameObject.transform.position = this.gameObject.transform.position.normalized * 2.5f;
        }
    }


    public void GotoNextPoint()
    {
        nav.isStopped = false;
        WaitTimer = 0.0f;
        // Returns if no points have been set up
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
}