using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    //Singleton
    public static ArrowManager Instance;

    //privates
    private GameObject currentArrow;
    private bool isAttached = false;
    private float FinalDrawDistance = 0.0f;

    //Public objects that can be referenced and seen by other objects.
    public SteamVR_TrackedObject trackedObj;
    public GameObject Bow;
    public GameObject arrowPrefab;
    public GameObject teleArrowPrefab;
    public GameObject stringAttachPoint;
    public GameObject arrowStartPoint;
    public GameObject stringStartPoint;
    public uint TotalArrows = 99;
    public uint ArrowsLeft = 99;

    //Private objects that can be assigned through the editor like Publics.
    [SerializeField] float PullPowerCompensation = 10.0f;
    [SerializeField] float ArrowPositionZCompensation = 1.0f;
    [SerializeField] float ReleaseStrength = 10.0f;
    [SerializeField] float MaxDrawDistance = 0.55f;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    void Destroy()
    {
        if (Instance == this)
            Instance = null;
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (ArrowsLeft > TotalArrows)
            ArrowsLeft = TotalArrows;


        

        //AttachArrow();
        if (ArrowsLeft > 0)
        {
            if (isAttached)
                PullString();

            var device = SteamVR_Controller.Input((int)trackedObj.index);
            if (device.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad) && isAttached == false && currentArrow != null)
            {
                ToggleArrow();
            }
        }
    }


    private void ToggleArrow()
    {
        bool IsTeleArrow = currentArrow.GetComponent<Arrow>().isTeleportArrow;
        Destroy(currentArrow);

        if (IsTeleArrow == false)
            currentArrow = Instantiate(teleArrowPrefab);
        else
            currentArrow = Instantiate(arrowPrefab);

        currentArrow.transform.parent = trackedObj.transform;
        currentArrow.transform.localPosition = new Vector3(0f, 0f, 0.42f);
        currentArrow.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void PullString()
    {
            //Gets distance between bow and pulling hand 
            float distance = (stringStartPoint.transform.position - trackedObj.transform.position).magnitude;
            if (distance > MaxDrawDistance)
            {
                distance = MaxDrawDistance;
            }
            //Debug.Log(distance.ToString());

            Bow.GetComponent<CompoundBowManager>().UpdateAnimation(distance / MaxDrawDistance);
            stringAttachPoint.transform.localPosition = new Vector3(0f, 0f, (stringStartPoint.transform.localPosition.z + distance + ArrowPositionZCompensation) * PullPowerCompensation);

            //Rotate bow

            //Vector3 FowardVector = Vector3.Normalize(Bow.transform.position - trackedObj.transform.position);
            //Bow.transform.LookAt(this.gameObject.transform.position + FowardVector);

            //releases the arrow if the trigger is released
            var device = SteamVR_Controller.Input((int)trackedObj.index);
            if (device.GetTouchUp(SteamVR_Controller.ButtonMask.Trigger))
            {
                FinalDrawDistance = distance;
                ReleaseArrow();
            }
    }


    private void ReleaseArrow()
    {
        //Ben - reduce ArrowsLeft by 1
        ArrowsLeft -= 1;
        //Release the arrow and give it a velocity
        currentArrow.transform.parent = null;
        currentArrow.GetComponent<Arrow>().Fired();
        //currentArrow.GetComponent<BoxCollider>().isTrigger = false;
        currentArrow.GetComponent<Arrow>().ThisArrowTip.GetComponent<BoxCollider>().isTrigger = false;
        Rigidbody r = currentArrow.GetComponent<Rigidbody>();
        r.velocity = currentArrow.transform.forward * (FinalDrawDistance / MaxDrawDistance) * ReleaseStrength;
        r.useGravity = true;
        r.isKinematic = false;

       // Bow.transform.localRotation = Quaternion.identity;
        Bow.GetComponent<CompoundBowManager>().ReleaseBowAnimation();

        stringAttachPoint.transform.position = stringStartPoint.transform.position;

        //Reset bools and checks
        currentArrow = null;
        isAttached = false;
    }

    //Attaching an Arrow to your hand if your hand currently is empty
    public void AttachArrow()
    {
        if (currentArrow == null)
        {
            //create a copy of the arrowPrefab 
            currentArrow = Instantiate(arrowPrefab);
            currentArrow.transform.parent = trackedObj.transform;
            currentArrow.transform.localPosition = new Vector3(0f, 0f, 0.45f);
            currentArrow.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
    }

    public void AttachBowToArrow()
    {
        currentArrow.transform.parent = stringAttachPoint.transform;

        //Attach the arrow to a set position and rotation each time so it aligns with the bow and the string
        currentArrow.transform.localPosition = arrowStartPoint.transform.localPosition;
        currentArrow.transform.localRotation = arrowStartPoint.transform.localRotation;
        isAttached = true;
    }
}
