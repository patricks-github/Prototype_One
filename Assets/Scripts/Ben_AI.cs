using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ben_AI : MonoBehaviour
{
    // Use this for initialization
    protected NavMeshAgent nav;               // Reference to the nav mesh agent.
    public bool Combat = false;
    public Transform[] points;
    protected int destPoint = 0;
    protected float WaitTimer = 20.0f;
    protected float CombatTimer = 0.0f;
    public Transform target;
    protected Vector3 targetDir;
    protected float angle;
    protected float RandomWait;
    public float Distance;
    public uint HitPoints = 1;
    private Roller_Movement ThisRollerMovement;


    void Awake()
    {
        RandomWait = Random.Range(15.0f, 20.0f);
        nav = GetComponent<NavMeshAgent>();
    }


    void Start()
    {
        ThisRollerMovement = this.GetComponent<Roller_Movement>();
        GamePlayManager.Instance.EnemiesRemaining++;
        nav.autoBraking = false;
    }


    public void Update()
    {
        targetDir = target.position - this.gameObject.transform.position;
        angle = Vector3.Angle(targetDir, this.gameObject.transform.forward);
        Distance = targetDir.magnitude;
        if (nav.isStopped)
        {
            WaitTimer += Time.deltaTime;
        }

        if (Combat)
        {
            CombatTimer += Time.deltaTime;
        }

        if (angle < 30.0f && Combat == false && Distance <= 30.0f)
        {
           // Debug.Log(angle);
            Debug.Log("AI In Combat");
            Combat = true;
        }

        if (Combat && (CombatTimer < 5.0f))
        {
            nav.SetDestination(target.position);
            /*this code here is for if the AI runs into the player. it can also be run in the collide, but thats been a bit janky
             * if (target.position == this.gameObject.transform.position)
            {
                this.gameObject.transform.position = this.gameObject.transform.position.normalized * 2.5f;
            }*/
        }
        else if (Combat && (CombatTimer > 5.0f))
        {
            Debug.Log("AI Left Combat");
            Combat = false;
            WaitTimer = 0.0f;
            CombatTimer = 0.0f;

            GotoNextPoint();
        }
        else
        {
            if (!nav.pathPending && nav.remainingDistance < 2.0f)
            {
                if (nav.remainingDistance < 1.5f)
                {
                    ThisRollerMovement.speedDisplayModifier = 0.0f;
                    nav.isStopped = true;
                }
                if (WaitTimer >= 5)
                {
                    ThisRollerMovement.speedDisplayModifier = 1.0f;
                    nav.isStopped = false;
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
            CombatTimer = 0.0f;
            nav.SetDestination(this.gameObject.transform.position.normalized * 2.5f);
        }

        if (_collision.gameObject.tag == "BossWall")
        {
            nav.SetDestination(this.gameObject.transform.position.normalized * -1.0f);
            GotoNextPoint();
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

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(this.transform.position, nav.destination);
    }
}