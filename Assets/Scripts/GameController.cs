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

    public GameObject shipPrefab;
    public List<Transform> spawnPoints;
    public List<Sprite> shooterSprites;
    public List<Sprite> chaserSprites;

    private float timeLeft;
    private String[] enemyTags;

    void Start()
    {
        timeLeft = PlayerPrefs.GetInt("rTime");
        int spawn = PlayerPrefs.GetInt("sTime");
        PlayerPrefs.SetInt("score", 0);
        enemyTags = new String[2] {"Shooter", "Chaser"};

        UpdateTime();
        UpdateScore();
        InvokeRepeating("spawnEnemy", 0f, (float) spawn );
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        UpdateTime();
        UpdateScore();
        if(timeLeft <= 0){
            GameOver();
        }
    }

    void UpdateTime() {
        roundTime.text = TimeSpan.FromSeconds(timeLeft).ToString(@"m\:ss");
    }

    void UpdateScore() {
        var score = PlayerPrefs.GetInt("score");
        scoreTxt.text = "Pontos: " + score.ToString();
    }

    void GameOver() {
        SceneManager.LoadScene("GameOver");
    }

    void spawnEnemy() {
        GameObject enemy = Instantiate(shipPrefab, spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Count-1)].position, Quaternion.identity);
        EnemyController enemyScript = enemy.AddComponent<EnemyController>();
        enemy.tag = enemyTags[UnityEngine.Random.Range(0, enemyTags.Length-1)];
        if(enemy.tag == "Shooter"){
            enemyScript.Init(shooterSprites);
        }else if(enemy.tag == "Chaser"){
            enemyScript.Init(chaserSprites);
        }
    }
}
