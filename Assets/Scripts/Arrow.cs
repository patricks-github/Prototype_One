using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private bool isFired = false;
    public bool isTeleportArrow = false;
    public GameObject ThisArrowTip;

    [SerializeField] float MaxTimeAlive = 5.0f;

    private float TimeAlive;

    void OnTriggerStay(Collider _other)
    {
        if (_other.gameObject.tag == "Bow")
            AttachArrow();
    }

    void OnTriggerEnter(Collider _other)
    {
        if (_other.gameObject.tag == "Bow")
            AttachArrow();
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
                ;
            }
        }

    }

    public void Fired()
    {
        isFired = true;
    }

    private void AttachArrow()
    {
        var device = SteamVR_Controller.Input((int)ArrowManager.Instance.trackedObj.index);
        if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
        {
            ArrowManager.Instance.AttachBowToArrow();
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        //destroys arrow if Tip hits a valid surface 
        // Debug.Log("Teleport");
        if (collision.gameObject.tag == "ValidSurface" && isTeleportArrow)
        {
            TeleportManager.Instance.TeleportToLocation(this.GetComponentInChildren<Transform>().position);
            Destroy(this.gameObject);
        }
    }
}













