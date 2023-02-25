using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cBallCtrl : MonoBehaviour
{
    public int origin;
    public string originTag;

    private Rigidbody2D rbd;

    // Start is called before the first frame update
    void Start()
    {
        rbd = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if(rbd.velocity.magnitude < 0.5f) Destroy(gameObject);
    }

    public void setOrigin(int id, string tag){
        origin = id;
        originTag = tag;
    }
}
