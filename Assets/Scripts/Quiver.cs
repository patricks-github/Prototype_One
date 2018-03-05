using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quiver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider collider) {

        Debug.Log("Entered Quiver");

        if (collider.gameObject.transform.parent.transform.parent.gameObject.tag == "GameController" && CompoundBowManager.Instance.isBowBeingHeld)
        {
            Debug.Log("Conditions being met");
            // var device = SteamVR_Controller.Input((int)collider.gameObject.GetComponent<SteamVR_TrackedObject>().index);
            var device = SteamVR_Controller.Input((int)ArrowManager.Instance.trackedObj.index);
            if (device.GetTouch(SteamVR_Controller.ButtonMask.Trigger))
            {
                Debug.Log("Attached Arrow");
                ArrowManager.Instance.AttachArrow();
            }
            
        }
    }
}
