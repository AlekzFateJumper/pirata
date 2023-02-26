using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    void Update()
    {
        Move();
        Fire();
    }

    void Move(){
        object[] args = new object[2];
        args[0] = Input.GetAxis("Vertical");
        args[1] = Input.GetAxis("Horizontal");

        SendMessage("Mover", args);
    }

    void Fire(){
        if(Input.GetButtonDown("Fire1")) SendMessage("TiroFrontal");
        if(Input.GetButtonDown("Fire2")) SendMessage("TiroLateral", true);
        if(Input.GetButtonDown("Fire3")) SendMessage("TiroLateral", false);
    }

    void TriggerUpdate(NavTrigger trigger){
    }

}
