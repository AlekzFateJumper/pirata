using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private GameObject player;
    private Vector3 playerPos;
    private ShipController shipCtrl;
    private int shipColliderId;
    private bool desviar;
    private float giro;
    private float veloc;

    public void Init(List<Sprite> sprites)
    {
        shipCtrl = GetComponent<ShipController>();
        shipCtrl.setSprites(sprites);
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
        player = GameObject.FindWithTag("Player");
        shipColliderId = shipCtrl.ship.GetComponent<Collider2D>().GetInstanceID();
        desviar = false;
        giro = 0f;
        veloc = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = player.transform.position;

        if(!desviar){
            giro = getGiroToPlayer();
        }

        object[] args = new object[2];
        args[0] = andar;
        args[1] = giro;
        shipCtrl.Mover(args);
    }

    float getAngle(Vector3 target){
        Vector3 vec = new Vector3();
        // rotaciona vetor para ficar igual a posição 0 do barco.
        vec = Quaternion.Euler(0, 0, 90) * (target - transform.position);
        // retorna ângulo final
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }

    float deltaAngle(Vector3 target){
        return Mathf.DeltaAngle(getAngle(target), shipCtrl.ship.transform.eulerAngles.z);
    }

    float getGiroToPlayer(){
        // Calcula giro para olhar player.
        float angle = deltaAngle(playerPos);
        float abs = Mathf.Abs(angle);

        float giro = angle / abs;
        if(abs < 25f) giro = giro / ((25f - abs)/10f + 1f);
        // Debug.Log("angle: " + angle);
        // Debug.Log("abs: " + abs);
        // Debug.Log("giro: " + giro);
        return giro;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if(collider.GetInstanceID() == shipColliderId) return;
        Debug.Log("Enter: " + collider.name);
    }

    void OnTriggerStay2D(Collider2D collider){
        if( collider.GetInstanceID() == shipColliderId ||
            collider.name == "Main Camera" ||
            collider.tag == "Respawn"
        ) return;
        var angle = deltaAngle(collider.transform.position);
        var dist = collider.Distance(shipCtrl.ship.GetComponent<Collider2D>());

        if(collider.tag != "Player"){
            if ( Mathf.Abs(angle) < 20 ){
                desviar = true;
                
            }else if( Mathf.Abs(angle) > 85 ){
                desviar = false;
            }
        }else{
        }

        Debug.Log("Stay: " + collider.name);
        Debug.Log("angle: " + angle);

    }

    void OnTriggerLeave2D(Collider2D collider){
        if(collider.GetInstanceID() == shipColliderId) return;
        Debug.Log("Leave: " + collider.name);
    }
}
