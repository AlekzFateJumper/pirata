using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBallCtrl : MonoBehaviour
{
    public int origin;

    private Rigidbody2D rbd;

    // Start is called before the first frame update
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.time);
        if(rbd.velocity == Vector2.zero) Destroy(gameObject);
    }

    public void setOrigin(int id){
        origin = id;
        Debug.Log(origin);
    }
}
