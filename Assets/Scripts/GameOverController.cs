using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    public TMP_Text scoreTxt;

    void Start()
    {
        UpdateScore();
    }

    void UpdateScore(){
        var score = PlayerPrefs.GetInt("score");
        scoreTxt.text = "Pontos: " + score.ToString();
    }

    public void Play(){
        PlayerPrefs.SetInt("score", 0);
        SceneManager.LoadScene("Game");
    }

    public void MainMenu(){
        PlayerPrefs.SetInt("score", 0);
        SceneManager.LoadScene("Main Menu");
    }
}
