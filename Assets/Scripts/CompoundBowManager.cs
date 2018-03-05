using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompoundBowManager : MonoBehaviour
{
    public static CompoundBowManager Instance;


    public SteamVR_TrackedObject trackedHandLeft;
    public SteamVR_TrackedObject trackedHandRight;
    public GameObject VRBodyObject;

    [SerializeField] Canvas ArrowDisplay;

    Animator BowAnimator;
    public bool isBowBeingHeld = false;

    bool bowPulled = false;
    bool bowAttachedToBody = false;

    float bowDraw;

    Vector3 OriginalScale;
    Vector3 ArrowDisplayScale;

    public bool IsBowInLeftHand = false;




    void Destroy()
    {
        if (Instance == this)
            Instance = null;
    }

    void Awake() {
        OriginalScale = this.gameObject.transform.lossyScale;
        ArrowDisplayScale = ArrowDisplay.gameObject.GetComponent<RectTransform>().localScale;

        if (Instance == null)
            Instance = this;
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

            ArrowDisplay.enabled = true;
            ArrowDisplay.GetComponentInChildren<Text>().text = ArrowManager.Instance.ArrowsLeft.ToString() + "/" + ArrowManager.Instance.TotalArrows.ToString();

           

            //Bow In the left hand
            if (this.gameObject.transform.parent.parent.parent.parent.gameObject.name == "Controller (left)")
            {
                ArrowManager.Instance.BoWHand = trackedHandLeft;
                ArrowManager.Instance.OffHand = trackedHandRight;
                this.gameObject.transform.localScale = OriginalScale;
                ArrowDisplay.gameObject.GetComponent<RectTransform>().localScale = ArrowDisplayScale;
                IsBowInLeftHand = true;
            }
            //Bow In the right hand
            else {
                ArrowManager.Instance.BoWHand = trackedHandRight;
                ArrowManager.Instance.OffHand = trackedHandLeft;
                this.gameObject.transform.localScale = new Vector3(-OriginalScale.x, OriginalScale.y, OriginalScale.z);
                ArrowDisplay.gameObject.GetComponent<RectTransform>().localScale = new Vector3(-ArrowDisplayScale.x, ArrowDisplayScale.y, ArrowDisplayScale.z);
                IsBowInLeftHand = false;
            }
        }
        else {
            isBowBeingHeld = false;
        }

        if (isBowBeingHeld == false && bowAttachedToBody == false) {
            bowAttachedToBody = true;
            this.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<Rigidbody>().velocity = Vector3.zero;
            VRBodyObject.GetComponent<VRBody>().AttatchObjectToBody(this.gameObject);
            UpdateAnimation(0f);
            ArrowDisplay.enabled = false;
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
