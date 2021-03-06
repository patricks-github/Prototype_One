﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour {

    public static GamePlayManager Instance;

    [SerializeField] GameObject BossCreatureReference;

    public int PlayerHitPoints = 1;
    public int EnemiesRemaining = 0;
    public bool IsTrophyReturned = false;

    public bool AnEnemyHasBeenKilled = false;


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

    }
	
	// Update is called once per frame
	void Update () {


        if (PlayerHitPoints <= 0) {
            //Game Over, Change Scenes
            SceneManager.LoadScene("GameOver");
        }

        //if (EnemiesRemaining <= 0 && AnEnemyHasBeenKilled) {
        //    //No Enemies Remaining
        //    SceneManager.LoadScene("WinGame");
        //}

        if (IsTrophyReturned == true) {
            //Win Game, Chance Scenes
            SceneManager.LoadScene("WinGame");
        }
    }


    public void HitPointsLost(int _HitPointsLost){
        PlayerHitPoints -= _HitPointsLost;
    }


    public void GoToScene(string _String) {
        SceneManager.LoadScene(_String);
    }
    

}
