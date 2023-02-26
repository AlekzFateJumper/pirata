using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBallCtrl : MonoBehaviour
{
    public int origin;
    public string originTag;
    public Collider2D ccl;

    private Rigidbody2D rbd;
    private float startTime;

    // Start is called before the first frame update
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(rbd.velocity.magnitude < .5f && Time.time - startTime > .2f) Destroy(gameObject);
    }

    public void setOrigin(int id, string tag){
        origin = id;
        originTag = tag;
        ccl.enabled = true;
    }
}
