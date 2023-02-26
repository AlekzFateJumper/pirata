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
    private int lastSpawn = -1;
    private int totalSpawnShooter;
    private int totalSpawnChaser;

    void Start()
    {
        timeLeft = PlayerPrefs.GetInt("rTime");
        int spawn = PlayerPrefs.GetInt("sTime");
        PlayerPrefs.SetInt("score", 0);
        enemyTags = new String[2] {"Shooter", "Chaser"};
        totalSpawnShooter = 0;
        totalSpawnChaser = 0;

        UpdateTime();
        UpdateScore();
        InvokeRepeating("spawnEnemy", 0f, (float) spawn );
        // Invoke("spawnEnemy", 0f);
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

    int getRandomSP(){
        int point = UnityEngine.Random.Range(0, spawnPoints.Count);
        if(point == lastSpawn) return getRandomSP();
        if(spawnPoints[point].GetComponent<SpawnPointCtrl>().occupied) return getRandomSP();
        return lastSpawn = point;
    }

    void spawnEnemy() {
        int point = getRandomSP();
        GameObject enemy = Instantiate(shipPrefab, spawnPoints[point].position, Quaternion.identity);
        EnemyController enemyScript = enemy.AddComponent<EnemyController>();
        ShipController shipScript = enemy.GetComponent<ShipController>();
        shipScript.ship.transform.rotation = spawnPoints[point].rotation;
        enemy.tag = enemyTags[UnityEngine.Random.Range(0, enemyTags.Length)];
        if(enemy.tag == "Shooter"){
            enemyScript.Init(shooterSprites);
            enemy.name = "Shooter" + totalSpawnShooter++;
        }else if(enemy.tag == "Chaser"){
            enemyScript.Init(chaserSprites);
            enemy.name = "Chaser" + totalSpawnChaser++;
        }
    }
}
