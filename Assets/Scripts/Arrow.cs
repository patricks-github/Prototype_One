using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private bool isFired = false;
    public bool isTeleportArrow = false;
    public GameObject ThisArrowTip;

    [SerializeField] AudioSource ArrowHitSound;
    [SerializeField] AudioSource ArrowBreakSound;


    [SerializeField] float MaxTimeAlive = 5.0f;

    private float TimeAlive;

    void OnTriggerStay(Collider _other)
    {
        if (_other.gameObject.tag == "Bow")
            AttachArrow();
    }

    void OnTriggerEnter(Collider _other)
    {
        //Arrow is in knocking Range
        if (_other.gameObject.tag == "Bow")
            AttachArrow();

        //An enemy has been hit
        if (_other.gameObject.tag == "ValidHitPoint" && isTeleportArrow == false && isFired)
        {
            Debug.Log("HIT! :)");
            ArrowHitSound.PlayOneShot(ArrowHitSound.clip);
            if (_other.gameObject.transform.root.gameObject.tag == "Ben_Boss" && _other.transform.root.gameObject.GetComponent<Ben_Boss>().IsDown == false)
            {
                if (_other.transform.root.gameObject.GetComponent<Ben_Boss>().HitPoints - 1 == 0)
                {
                    _other.transform.root.gameObject.GetComponent<Ben_Boss>().SetIsDown();
                    //Destroy(_other.transform.root.gameObject);
                    //GamePlayManager.Instance.EnemiesRemaining--;
                    //GamePlayManager.Instance.AnEnemyHasBeenKilled = true;

                }
                else
                {
                    _other.transform.root.gameObject.GetComponent<Ben_Boss>().HitPoints -= 1;

                }

            }
            else
            {
                if (_other.transform.parent.parent.parent.gameObject.GetComponent<Ben_AI>().HitPoints - 1 == 0)
                {
                    Destroy(_other.transform.root.gameObject);
                    GamePlayManager.Instance.EnemiesRemaining--;
                    GamePlayManager.Instance.AnEnemyHasBeenKilled = true;
                }
                else
                {
                    _other.transform.parent.parent.parent.gameObject.GetComponent<Ben_AI>().HitPoints -= 1;
                }

            }
            Destroy(this);
        }

        //Arrow hit solid surface, play break animations / effects
        if (_other.gameObject.tag == "ArrowBreak" && isFired == true)
        {
            ArrowBreakSound.PlayOneShot(ArrowBreakSound.clip);
            isFired = false;
            Debug.Log("Arrow break");
            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<Rigidbody>().Sleep();
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<ParticleSystem>().Play();
            Invoke("KillMeNow", 1.0f);
        }

        //Ben - Arrow hit an interactable, adding arrows to your total
        if (_other.gameObject.tag == "ArrowInteractable" && isFired)
        {
            //ArrowManager.Instance.ArrowsLeft += Ben_Interactable.Instance.ArrowsAdded;
            ArrowManager.Instance.ArrowsLeft += 15;
            Destroy(_other.transform.root.gameObject);
        }
    }


    void Update()
    {
        if (isFired)
        {
            transform.LookAt(transform.position + transform.GetComponent<Rigidbody>().velocity);

            //Arrow Clean Up
            TimeAlive += Time.deltaTime;
            if (TimeAlive >= MaxTimeAlive)
            {
                Destroy(this.gameObject);

            }
        }

    }

    public void Fired()
    {
        isFired = true;
    }

    private void AttachArrow()
    {
        var device = SteamVR_Controller.Input((int)ArrowManager.Instance.OffHand.index);
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            ArrowManager.Instance.AttachBowToArrow();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        //destroys arrow if Tip hits a valid surface 
        // Debug.Log("Teleport");
        if (collision.gameObject.tag == "Floor" && isTeleportArrow)
        {
            ArrowBreakSound.PlayOneShot(ArrowBreakSound.clip);
            Vector3 PositionToMoveToo = this.GetComponentInChildren<Transform>().position;
            PositionToMoveToo.y = this.GetComponentInChildren<Transform>().position.y;
            TeleportManager.Instance.TeleportToLocation(PositionToMoveToo);
            Destroy(this.gameObject);
        }
        else if (collision.gameObject.tag == "Floor" && !isTeleportArrow)
        {
            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<Rigidbody>().Sleep();
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<ParticleSystem>().Play();
            Invoke("KillMeNow", 1.0f);
        }

        if (collision.gameObject.tag == "BossWall")
        {
            ArrowBreakSound.PlayOneShot(ArrowBreakSound.clip);
            this.GetComponent<Collider>().enabled = false;
            this.GetComponent<Rigidbody>().Sleep();
            this.GetComponent<MeshRenderer>().enabled = false;
            this.GetComponent<ParticleSystem>().Play();
            Invoke("KillMeNow", 1.0f);
        }
    }

    private void KillMeNow()
    {
        Destroy(this);
    }
}