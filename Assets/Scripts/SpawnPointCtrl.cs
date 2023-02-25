using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointCtrl : MonoBehaviour
{
    public bool occupied;

    // Start is called before the first frame update
    void Start()
    {
        occupied = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void OnTriggerEnter2D(Collider2D collider){
        occupied = true;
    }

    void OnTriggerStay2D(Collider2D collider){
        occupied = true;
    }

    void OnTriggerLeave2D(Collider2D collider){
        occupied = false;
    }
}
