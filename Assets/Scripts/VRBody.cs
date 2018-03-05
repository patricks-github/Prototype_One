using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRBody : MonoBehaviour {

    public GameObject VRCamera;

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
       // VRCamera = transform.parent.GetChild(0).gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Setting Variablesd
        transform.position = VRCamera.transform.position - new Vector3(0f, VRCamera.transform.localPosition.y / 2.0f, 0f);
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

    public void AttatchObjectToBody(GameObject _gameObject) {
       
        switch (_gameObject.tag) {
            case "Bow":
                //Places the bow on roughly the left hip using half the body's width
                _gameObject.transform.parent = this.gameObject.transform;
                _gameObject.transform.localPosition = Vector3.zero + new Vector3(-0.5f, 0.0f, 0.0f);
                _gameObject.transform.rotation = Quaternion.identity;
                break;

            case "Attachable":
                //Regular Game object, attach it to body
                _gameObject.transform.parent = this.gameObject.transform;
                break;

            default:
                break;
        }
    }

}
