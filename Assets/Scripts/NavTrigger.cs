using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        void OnTriggerEnter2D(Collider2D collider){
        if( collider.tag == "Respawn" ||
            collider.tag == "Player"
        ) return;

        SendMessageUpwards("TriggerEnter", collider);
    }

    // void OnTriggerStay2D(Collider2D collider){
    //     if( collider.tag == "Respawn" ||
    //         collider.tag == "Player"
    //     ) return;

    //     SendMessageUpwards("TriggerStay", collider);
    // }

    void OnTriggerExit2D(Collider2D collider){
        if( collider.tag == "Respawn" ||
            collider.tag == "Player"
        ) return;

        SendMessageUpwards("TriggerExit", collider);
    }
}
