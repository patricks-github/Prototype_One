using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour {

    public static GamePlayManager Instance;

    [SerializeField] GameObject BossCreatureReference;

    public int PlayerHitPoints = 1;
    public int EnemiesRemaining = 1;
    public bool IsBossDead = false;


    private bool GameStarted;
    private float Timer = 0.0f;

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
    void Start () { 
        GameStarted = true;

    }
	
	// Update is called once per frame
	void Update () {

        Timer += Time.deltaTime;
        if(Timer >= 5.0f && GameStarted == true)
        {
            EnemiesRemaining--;
            GameStarted = false;
        }

        if (PlayerHitPoints <= 0) {
            //Game Over, Change Scenes
            SceneManager.LoadScene("GameOver");
        }

        if (EnemiesRemaining <= 0) {
            //No Enemies Remaining
            SceneManager.LoadScene("WinGame");
        }

        //if (BossCreatureReference == null) {
        //    //Win Game, Chance Scenes
        //    SceneManager.LoadScene("WinGame");
        //}
	}


    public void HitPointsLost(int _HitPointsLost){
        PlayerHitPoints -= _HitPointsLost;
    }

    

}
