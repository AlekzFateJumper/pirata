using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuControl : MonoBehaviour
{
    public GameObject mainMenuObj;
    public GameObject cfgMenuObj;

    public TMP_Text roundTime;
    public TMP_Text spawnTime;

    // Start is called before the first frame update
    void Start()
    {
        ChangeRoundTime(120);
        ChangeSpawnTime(5);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Config(){
        mainMenuObj.SetActive(false);
        cfgMenuObj.SetActive(true);
    }
    
    public void Top(){
        cfgMenuObj.SetActive(false);
        mainMenuObj.SetActive(true);
    }

    public void ChangeRoundTime(float time){
        if(roundTime != null) roundTime.text = TimeSpan.FromSeconds(time).ToString(@"m\:ss");
        PlayerPrefs.SetInt("rTime", (int) time);

    }

    public void ChangeSpawnTime(float time){
        if(spawnTime != null) spawnTime.text = TimeSpan.FromSeconds(time).ToString(@"m\:ss");
        PlayerPrefs.SetInt("sTime", (int) time);
    }

    public void Play(){
        SceneManager.LoadScene("Game");
    }
}
