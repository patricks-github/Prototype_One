using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonChangeScene : MonoBehaviour {

    [SerializeField] string SceneToLoad = "";

    private void LoadThisScene(string _SceneToLoad) {
        if (SceneToLoad != "")
            if (SceneToLoad == "Quit")
            {
                //Quit
                Debug.Log("Quitting Game");
                Application.Quit();
            }
            else
                SceneManager.LoadScene(_SceneToLoad);
        else
            Debug.LogWarning("Button Change Scene - No Scene has been Selected");
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Button Has Been Hit by arrow, Change Scene

        Debug.Log("Collision Occured WIth Button");

        if (collision.gameObject.tag == "ArrowTip")
        {
            LoadThisScene(SceneToLoad);
        }
    }

}
