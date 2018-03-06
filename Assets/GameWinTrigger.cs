using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinTrigger : MonoBehaviour {

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.name == "Trophy") {
            //You have brought the trophy back, you win
            GamePlayManager.Instance.IsTrophyReturned = true;
        }
    }

}
