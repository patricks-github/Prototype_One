using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller_Movement : MonoBehaviour {

    Animator RollerAnimator;

	// Use this for initialization
	void Start () {
        RollerAnimator = gameObject.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        RollerAnimator.SetFloat("speed", gameObject.GetComponent<Rigidbody>().velocity.magnitude);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 200.0f);
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            gameObject.transform.Rotate(new Vector3(0, 180, 0));
        }
    }
}
