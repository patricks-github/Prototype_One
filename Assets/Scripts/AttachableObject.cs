using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachableObject : MonoBehaviour {


    public bool IsBeingHeld;
    public GameObject VRBodyObject;


    bool ObjectHasBeenPickedUp;
    bool IsInBody;
    bool IsAttached;

    // Use this for initialization
    void Start() {
        ObjectHasBeenPickedUp = false;
        IsAttached = false;
    }

    // Update is called once per frame
    void Update() {
        //Object is being held
        if (this.gameObject.GetComponent<Rigidbody>().isKinematic == true)
        {
            ObjectHasBeenPickedUp = true;
            IsBeingHeld = true;
            this.GetComponent<Rigidbody>().useGravity = true;
            this.GetComponent<BoxCollider>().isTrigger = true;
            IsAttached = false;
            this.GetComponent<Rigidbody>().freezeRotation = false;
        }
        else {
            IsBeingHeld = false;

            if (IsAttached == false && ObjectHasBeenPickedUp && IsInBody) {
                //Object Is In Body, Attach to player
                VRBodyObject.GetComponent<VRBody>().AttatchObjectToBody(this.gameObject);
                this.GetComponent<Rigidbody>().useGravity = false;
                this.GetComponent<BoxCollider>().isTrigger = true;
                IsAttached = true;
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.GetComponent<Rigidbody>().freezeRotation = true;

            }

            if (IsAttached == false && !IsInBody)
            {
                this.GetComponent<BoxCollider>().isTrigger = false;
            }
        }
    }


    void OnTriggerStay(Collider collider) {
        IsInBody = true;
    }

    void OnTriggerEnter(Collider collider)
    {
        IsInBody = true;
    }
    void OnTriggerExit(Collider collider)
    {
        IsInBody = false;
    }


    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("Object inside VR Body");
    //    if (IsBeingHeld == false && ObjectHasBeenPickedUp)
    //    {
    //        //Attach To body
    //        VRBodyObject.GetComponent<VRBody>().AttatchObjectToBody(this.gameObject);
    //        this.GetComponent<Rigidbody>().useGravity = false;
    //        this.GetComponent<BoxCollider>().isTrigger = true;
    //        IsAttached = true;
    //        this.GetComponent<Rigidbody>().velocity = Vector3.zero;

    //    }
    //}

    //void OnCollisionStay(Collision collision) {
    //    Debug.Log("Object inside VR Body");
    //    if (IsBeingHeld == false && ObjectHasBeenPickedUp) {
    //        //Attach To body
    //        VRBodyObject.GetComponent<VRBody>().AttatchObjectToBody(this.gameObject);
    //        this.GetComponent<Rigidbody>().useGravity = false;
    //        this.GetComponent<BoxCollider>().isTrigger = true;
    //        IsAttached = true;
    //        this.GetComponent<Rigidbody>().velocity = Vector3.zero;
    //    }
    //}


}

        

    
    



