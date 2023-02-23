using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public TMP_Text roundTime;

    private float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = PlayerPrefs.GetInt("rTime");
        UpdateTime();
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        UpdateTime();
    }

    void UpdateTime(){
        roundTime.text = TimeSpan.FromSeconds(timeLeft).ToString(@"m\:ss");
    }
}
