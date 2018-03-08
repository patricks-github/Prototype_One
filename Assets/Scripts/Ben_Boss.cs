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
    public Transform target;
    protected Vector3 ChargeDestinationVector;
    protected Vector3 targetDir;
    public float angle;
    public bool StartCharge = false;
    public float Distance;
    public uint HitPoints = 5;
    private Boss_Movement ThisBossMovement;

    public bool IsDown = false;
    public float TimeIsDown = 8f;


    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }


    void Start()
    {
        ThisBossMovement = this.GetComponent<Boss_Movement>();
        GamePlayManager.Instance.EnemiesRemaining++;
        nav.autoBraking = false;
    }


    public void Update()
    {
        if (!IsDown)
        {
            targetDir = target.position - this.gameObject.transform.position;
            angle = Vector3.Angle(targetDir, this.gameObject.transform.forward);
            Distance = targetDir.magnitude;

            if (Combat)
            {
                CombatTimer += Time.deltaTime;
            }

            if (angle < 45.0f && Combat == false && Distance <= 50.0f)
            {
                Debug.Log("Boss In Combat");
                Combat = true;
            }

            if (angle < 35.0f && Combat == false && Distance <= 35.0f)
            {
                Debug.Log("Boss In Combat");
                Combat = true;
            }

            if (Combat && (CombatTimer < 21.0f))
            {
                if (Combat && (CombatTimer > 1.5f) && StartCharge == false)
                {
                    StartCharge = true;
                    Charge(target.position);
                }
            }
            else if (Combat && (CombatTimer > 21.0f))
            {
                Debug.Log("Boss Left Combat");

                StartCharge = false;
                Combat = false;
                CombatTimer = 0.0f;

                nav.speed = 4.0f;
                nav.speed = 3.0f;

                GotoNextPoint();
            }
            else
            {
                if (!nav.pathPending && nav.remainingDistance < 1.0f)
                {
                    nav.speed = 3.0f;
                    nav.isStopped = false;

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
        }
    }


    public void SetIsDown() {
        IsDown = true;
        nav.isStopped = true;
        nav.enabled = false;
        this.transform.position += new Vector3(0.0f, 0.6f, 0.0f);
        this.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        Invoke("Recover", TimeIsDown);

    }

    private void Recover() {
        IsDown = false;
        nav.enabled = true;
        nav.isStopped = false;
        this.transform.rotation = Quaternion.identity;
        this.transform.position += new Vector3(0.0f, -0.6f, 0.0f);
    }


    void OnCollisionEnter(Collision _collision)
    {
        if (_collision.gameObject.tag == "Player")
        {
            //end game here
        }
        if (_collision.gameObject.tag == "BossWall")
        {
            StartCharge = false;
            CombatTimer = 0.0f;
            nav.speed = 3.0f;
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
            CombatTimer = 0.0f;

            Debug.Log("CHARGE");

            nav.speed = 15.0f;

            //ChargeDestinationVector = (target.position - this.gameObject.transform.position);
            //ChargeDestinationVector = ChargeDestinationVector.normalized * 10.0f;
            //nav.SetDestination(ChargeDestinationVector);
            nav.SetDestination(target.position);

            //if (ChargeTimer > 1.0f)
            //{
            //    ChargeTimer = 0.0f;
            //    //ChargeDestinationVector = this.gameObject.transform.position;
            //    StartCharge = false;
            //}
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(this.transform.position, nav.destination);
    }
}