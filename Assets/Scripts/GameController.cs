using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public TMP_Text roundTime;
    public TMP_Text scoreTxt;

    private float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = PlayerPrefs.GetInt("rTime");
        PlayerPrefs.SetInt("score", 0);

        UpdateTime();
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        UpdateTime();
        UpdateScore();
        if(timeLeft <= 0){
            GameOver();
        }
    }

    void UpdateTime(){
        roundTime.text = TimeSpan.FromSeconds(timeLeft).ToString(@"m\:ss");
    }

    void UpdateScore(){
        var score = PlayerPrefs.GetInt("score");
        scoreTxt.text = "Pontos: " + score.ToString();
    }

    void ExplodeShip(String type){
        if(type == "player"){
            GameOver();
        }
    }

    void GameOver(){
        SceneManager.LoadScene("GameOver");
    }
}
