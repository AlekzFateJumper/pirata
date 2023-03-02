using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavTrigger : MonoBehaviour
{
    public Collider2D selfCollider;
    public List<Collider2D> colliders;
    public int pos;

    void Start(){
        colliders = new List<Collider2D>();
        selfCollider = GetComponent<Collider2D>();
        SendMessageUpwards("TriggerUpdate", this);
    }

    void OnTriggerEnter2D(Collider2D collider){
        if( collider.tag == "Respawn" ||
            collider.tag == "Player" ||
            collider.tag == "Trigger" ||
            collider.tag == "CannonBall" ||
            collider.isTrigger
        ) return;

        colliders.Add(collider);

        SendMessageUpwards("TriggerUpdate", this);
    }

    void OnTriggerExit2D(Collider2D collider){
        if( collider.tag == "Respawn" ||
            collider.tag == "Player" ||
            collider.tag == "Trigger" ||
            collider.tag == "CannonBall" ||
            collider.isTrigger
        ) return;

        colliders.Remove(collider);

        SendMessageUpwards("TriggerUpdate", this);
    }
}
