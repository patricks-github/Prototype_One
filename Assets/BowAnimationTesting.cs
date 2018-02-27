using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowAnimationTesting : MonoBehaviour {

    Animator BowAnimator;

    bool bowPulled = false;

    float bowDraw;

    // Use this for initialization
    void Start () {
        BowAnimator = gameObject.GetComponent<Animator>();
        SetBowSpeed(0);
	}
	
	// Update is called once per frame
	void Update () {
        if (bowPulled)
        {
            //SetBowSpeed(0);

            BowAnimator.Play(0, 0, bowDraw);
        }	
        else
        {
            SetBowSpeed(-25);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            bowPulled = true;
            if (bowDraw < 1.0f)
            {
                Debug.Log("Draw");
                bowDraw += 0.5f * Time.deltaTime;
            }
        }
        else
        {
            bowDraw = 0.0f;
            if (bowPulled)
            {
                bowPulled = false;
            }
        }
	}

    void SetBowSpeed(float speed)
    {
        BowAnimator.SetFloat("speed", speed);
    }
}
