using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameWinTrigger : MonoBehaviour {

    public string SceneToGoToo = "";

    void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.name == "Trophy") {
            //You have brought the trophy back, you win
            
            if (SceneToGoToo != "")
                EnterNewScene(SceneToGoToo);
            else
                GamePlayManager.Instance.IsTrophyReturned = true;
        }
    }


    private void EnterNewScene(string _String) {
        GamePlayManager.Instance.GoToScene(_String);
    }

}
