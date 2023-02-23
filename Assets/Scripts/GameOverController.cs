using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    public TMP_Text scoreTxt;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScore();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateScore(){
        var score = PlayerPrefs.GetInt("score");
        scoreTxt.text = "Pontos: " + score.ToString();
    }

    public void Play(){
        var score = PlayerPrefs.SetInt("score", 0);
        SceneManager.LoadScene("Game");
    }

    public void MainMenu(){
        var score = PlayerPrefs.SetInt("score", 0);
        SceneManager.LoadScene("MainMenu");
    }
}
