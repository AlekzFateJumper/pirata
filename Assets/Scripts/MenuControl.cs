using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MenuControl : MonoBehaviour
{
    public GameObject mainMenuObj;
    public GameObject cfgMenuObj;

    public TMP_Text roundTime;
    public TMP_Text spawnTime;

    private float rTime;
    private float sTime;

    // Start is called before the first frame update
    void Start()
    {
        rTime = 60;
        sTime = 5;
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
        rTime = time;
        roundTime.text = TimeSpan.FromSeconds(rTime).ToString(@"m\:ss");
    }

    public void ChangeSpawnTime(float time){
        sTime = time;
        spawnTime.text = TimeSpan.FromSeconds(sTime).ToString(@"m\:ss");
    }
}
