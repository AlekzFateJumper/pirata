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

    private float timeLeft;
    private bool init;
    private String[] enemyTags;

    void Start()
    {
        timeLeft = PlayerPrefs.GetInt("rTime");
        PlayerPrefs.SetInt("score", 0);
        init = true;
        enemyTags = new String[2] {"Shooter", "Chaser"};

        UpdateTime();
        UpdateScore();
        spawnEnemy();
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

    IEnumerator spawnEnemy() {
        int interval = PlayerPrefs.GetInt("sTime");
        if(!init) yield return new WaitForSeconds(interval);
        else init = false;
        GameObject enemy = Instantiate(shipPrefab, spawnPoints[System.Random(0, spawnPoints.Count)].position, Quaternion.identity);
        EnemyController enemyScript = enemy.AddComponent<EnemyController>();
        enemy.tag = enemyTags[System.Random(0, enemyTags.Length)];
    }
}
