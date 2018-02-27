using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Ben_AI : MonoBehaviour
{
    // Use this for initialization
    protected Transform Player;               // Reference to the player's position.
                                              // PlayerHealth playerHealth;      // Reference to the player's health.
                                              // EnemyHealth enemyHealth;        // Reference to this enemy's health.
    protected NavMeshAgent nav;               // Reference to the nav mesh agent.
    protected bool Aggro = false;
    protected bool Combat = false;
    public bool isBoss = false;
    public Transform[] points;
    protected int destPoint = 0;
    protected float WaitTimer = 0.0f;
    protected float SpeedTimer = 0.0f;
    protected float CombatTimer = 0.0f;
    protected float IdleTimer = 0.0f;
    public Transform target;
    protected Vector3 ChargeDestinationVector;
    void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform;   
        nav = GetComponent<NavMeshAgent>();
    }

    void Start()
    {
        nav.autoBraking = false;
    }

    public void Update()
    {
        Vector3 targetDir = target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);
        WaitTimer += Time.deltaTime;
        SpeedTimer += Time.deltaTime;
        CombatTimer += Time.deltaTime;
        IdleTimer += Time.deltaTime;
        if (angle < 10.0f)
        {
            Debug.Log("Called angle");
            if (isBoss)
            {
                Debug.Log("Called Combat");
                Combat = true;
            }
            else
            {
                Debug.Log("Called Aggro");
                Aggro = true;
            }            
        }
        else
        {
            Aggro = false;
        }

        if (Combat && (CombatTimer > 3.0f))
        {
            if (isBoss)
            {
                if (IdleTimer > 8.0f)
                {
                    IdleTimer = 0.0f;
                    Combat = false;
                }
                else
                {
                    IdleTimer = 0.0f;
                    Charge(Player.position);
                }
            }
            else
            {
                CombatTimer = 0.0f;
                nav.speed += 0.5f;
                nav.SetDestination(Player.position);
            }
        }
        else if (Aggro)
        {            
            nav.SetDestination(Player.position);            
        }
        else
        {
            if (!nav.pathPending && nav.remainingDistance < 1.0f)
            {                
                if (nav.remainingDistance < 0.5f)
                {
                   nav.isStopped = true;
                }
                float RandomWait = Random.Range(5.0f, 10.0f);
                if (WaitTimer >= RandomWait)
                {
                    nav.speed = 4.0f;
                    nav.isStopped = false;
                    GotoNextPoint();
                    WaitTimer = 0.0f;
                }
            }                
        }
                
    }
    void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.tag == "Player")
        {
            CombatTimer = 0.0f;
            Combat = true;
            // nav.SetDestination(Player.position - PlayerCollideBounce);
        }

        if (_collision.gameObject.tag == "BossWall")
        {
            CombatTimer = 0.0f;
            nav.speed = 4.0f;
        }
    }

    public void GotoNextPoint()
    {
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
    void Charge(Vector3 _ChargeLoc)
    {
        Debug.Log("CHARGE");
        nav.speed = 16.0f;
        ChargeDestinationVector = (Player.position - this.gameObject.transform.position);
        ChargeDestinationVector = ChargeDestinationVector.normalized * 500.0f;
        nav.SetDestination(ChargeDestinationVector);
    }
}