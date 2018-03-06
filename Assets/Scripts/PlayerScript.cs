using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    void OnTriggerEnter(Collider _Collider)
    {
        Debug.Log("ENTERTED TRIGGER AREA");
        if (_Collider.gameObject.transform.root.gameObject.tag == "Enemy" || (_Collider.gameObject.transform.root.gameObject.tag == "Ben_Boss" && _Collider.gameObject.transform.root.gameObject.GetComponent<Ben_Boss>().IsDown == false))
        {
            //die
            Debug.Log("LOST HEALTH");
            GamePlayManager.Instance.HitPointsLost(1);
        }
    }

}
