using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionCtrl : MonoBehaviour
{
    protected Action callback;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setCallback(Action cb){
        callback = cb;
    }

    public void runCallback(){
        callback();
    }
}
