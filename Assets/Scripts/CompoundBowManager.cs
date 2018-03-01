using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompoundBowManager : MonoBehaviour
{

    public SteamVR_TrackedObject trackedHandLeft;
    public SteamVR_TrackedObject trackedHandRight;
    public GameObject VRBodyObject;

    Animator BowAnimator;
    public bool isBowBeingHeld = false;

    bool bowPulled = false;
    bool bowAttachedToBody = false;

    float bowDraw;

    Vector3 OriginalScale;

    void Awake() {
        OriginalScale = this.gameObject.transform.lossyScale;
    }

    // Use this for initialization
    void Start()
    {
        BowAnimator = gameObject.GetComponent<Animator>();
        SetBowSpeed(0);
        
    }

    // Update is called once per frame
    void Update()
    {
        //are you holding the bow?
        if (this.gameObject.GetComponent<Rigidbody>().isKinematic == true)
        {
            isBowBeingHeld = true;
            bowAttachedToBody = false;
            this.GetComponent<Rigidbody>().useGravity = true;
            this.gameObject.transform.localScale = OriginalScale;
        }
        else {
            isBowBeingHeld = false;
        }

        if (isBowBeingHeld == false && bowAttachedToBody == false) {
            bowAttachedToBody = true;
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            VRBodyObject.GetComponent<VRBody>().AttatchObjectToBody(this.gameObject);
        }

    }

    void SetBowSpeed(float speed)
    {
        BowAnimator.SetFloat("speed", speed);
    }


    public void UpdateAnimation(float DrawPercentage)
    {
        BowAnimator.Play(0, 0, DrawPercentage);
    }

    public void ReleaseBowAnimation()
    {
        SetBowSpeed(-25);
    }


}
