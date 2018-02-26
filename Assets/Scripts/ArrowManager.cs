﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour {
    //Singleton
    public static ArrowManager Instance;

    //privates
    private GameObject currentArrow;
    private bool isAttached = false;
    private float FinalDrawDistance = 0.0f;

    //Public objects that can be referenced and seen by other objects.
    public SteamVR_TrackedObject trackedObj;
    public GameObject arrowPrefab;
    public GameObject stringAttachPoint;
    public GameObject arrowStartPoint;
    public GameObject stringStartPoint;

    //Private objects that can be assigned through the editor like Publics.
    [SerializeField] float PullPowerCompensation = 10.0f;
    [SerializeField] float ArrowPositionXCompensation = 1.0f;
    [SerializeField] float ReleaseStrength = 10.0f;
    [SerializeField] float MaxDrawDistance = 0.55f;

    void Awake() {
        if (Instance == null)
            Instance = this;
    }

    void Destroy()
    {
        if (Instance == this)
            Instance = null;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        AttachArrow();
        PullString();
	}

    private void PullString() {
        if (isAttached) {
            //Gets distance between bow and pulling hand 
            float distance = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;
            if (distance > MaxDrawDistance)
            {
                distance = MaxDrawDistance;
            }
            //Debug.Log(distance.ToString());
            stringAttachPoint.transform.localPosition = new Vector3((stringStartPoint.transform.localPosition.x + distance + ArrowPositionXCompensation) * PullPowerCompensation, 0f, 0f);

            //releases the arrow if the trigger is released
            var device = SteamVR_Controller.Input((int)trackedObj.index);
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger)){
                FinalDrawDistance = distance;
                ReleaseArrow();
            }
        }
    }


    private void ReleaseArrow() {
        //Release the arrow and give it a velocity
        currentArrow.transform.parent = null;
        currentArrow.GetComponent<Arrow>().Fired();
        //currentArrow.GetComponent<BoxCollider>().isTrigger = false;
        currentArrow.GetComponent<Arrow>().ThisArrowTip.GetComponent<BoxCollider>().isTrigger = false;
        Rigidbody r = currentArrow.GetComponent<Rigidbody>();
        r.velocity = currentArrow.transform.forward * (FinalDrawDistance /0.6f) * ReleaseStrength;
        r.useGravity = true;

        stringAttachPoint.transform.position = stringStartPoint.transform.position;

        //Reset bools and checks
        currentArrow = null;
        isAttached = false;
    }

    //Attaching an Arrow to your hand if your hand currently is empty
    private void AttachArrow() {
        if (currentArrow == null) {
            //create a copy of the arrowPrefab 
           currentArrow = Instantiate(arrowPrefab);
            currentArrow.transform.parent = trackedObj.transform;
            currentArrow.transform.localPosition = new Vector3(0f, 0f, 0.34f);
          //currentArrow.transform.localRotation = Quaternion.identity;
            currentArrow.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void AttachBowToArrow() {
        currentArrow.transform.parent = stringAttachPoint.transform;

        //Attach the arrow to a set position and rotation each time so it aligns with the bow and the string
        currentArrow.transform.localPosition = arrowStartPoint.transform.localPosition;
        currentArrow.transform.localRotation = arrowStartPoint.transform.localRotation;
        isAttached = true;
    }
}
