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
    private int score;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = PlayerPrefs.GetInt("rTime");
        score = 0;

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
        scoreTxt.text = "Pontos: " + score.ToString();
    }

    void GameOver(){
        PlayerPrefs.SetInt("score", score);
        SceneManager.LoadScene("Game");
    }
}
