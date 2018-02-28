using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBody : MonoBehaviour {

    GameObject VRCamera;

    Vector3 cameraRotation;
    Vector3 currentRotation;

    public float headTurnDistance = 50.0f;
    public float turnSpeed = 100.0f;

    //Turn speed modifier based on angle difference
    float turnSpeedModifier;
    float headTurnSpeed = 100.0f;
    float AngleDif;

    bool Rotate = false;

	// Use this for initialization
	void Start ()
    {
        VRCamera = transform.parent.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Setting Variables
        transform.position = VRCamera.transform.position;
        cameraRotation = new Vector3(0, VRCamera.transform.localEulerAngles.y, 0);
        currentRotation = new Vector3(0, transform.localEulerAngles.y, 0);

        //Positive angle difference between body and camera.
        AngleDif = Mathf.Abs(Mathf.DeltaAngle(currentRotation.y, cameraRotation.y));

        //Debug.Log("Rotate Bool: " + Rotate + ", Rotate Difference: " +  AngleDif);

        Rotate = (AngleDif > headTurnDistance);
        
        if (Rotate)
        {
            //Changing turn speed based on angle
            turnSpeedModifier = AngleDif;

            //Deciding which direction to turn towards
            if (Mathf.DeltaAngle(currentRotation.y, cameraRotation.y) > 1)
            {
                headTurnSpeed = turnSpeed * turnSpeedModifier;
            }
            else
            {
                headTurnSpeed = (turnSpeed * turnSpeedModifier) * -1;
            }

            //Actually rotating
            transform.Rotate(Vector3.up * (headTurnSpeed * Time.deltaTime));
        }
    }
}
