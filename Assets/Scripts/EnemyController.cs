using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private GameObject player;
    private Vector3 playerPos;
    private ShipController shipCtrl;

    public void Init(List<Sprite> sprites)
    {
        shipCtrl = GetComponent<ShipController>();
        shipCtrl.setSprites(sprites);
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = player.transform.position;

        float angle = getAngle(playerPos);
        float abs = Mathf.Abs(angle);

        float giro = angle / abs;
        if(abs < 25) giro = giro / (2.5f - (abs/10));

        Debug.Log("giro: " + giro);

        object[] args = new object[2];
        args[0] = 0f;
        args[1] = giro;
        shipCtrl.Mover(args);
    }

    float getAngle(Vector3 target){
        Vector3 vec = new Vector3();
        vec = Quaternion.Euler(0, 0, -90) * (target - transform.position);

        return -(Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg);
    }

    void OnTriggerEnter2D(Collider2D collider){
        
    }

    void OnTriggerStay2D(Collider2D collider){
        
    }

    void OnTriggerLeave2D(Collider2D collider){
        
    }
}
