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
    private float deltaToPlayer;
    private bool pInRange;

    public void Init(List<Sprite> sprites)
    {
        shipCtrl = GetComponent<ShipController>();
        shipCtrl.setSprites(sprites);
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
        player = GameObject.FindWithTag("Player");
        shipColliderId = shipCtrl.ship.GetComponent<Collider2D>().GetInstanceID();
        desviar = false;
        deltaToPlayer = 0f;
        pInRange = false;
        giro = 0f;
        veloc = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = player.transform.position;

        if(!desviar){
            giro = getGiroToPlayer();
            if(pInRange && shipCtrl.tag == "Shooter") veloc = 0f;
            else veloc = 1f;
        }

        object[] args = new object[2];
        args[0] = veloc;
        args[1] = giro;
        shipCtrl.Mover(args);
        Debug.Log("Mover: " + args);
        Debug.Log("Desviar: " + desviar);
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
        deltaToPlayer = deltaAngle(playerPos);
        float abs = Mathf.Abs(deltaToPlayer);

        float giro = Mathf.Sign(deltaToPlayer);
        if(abs < 25f) giro = giro / ((25f - abs)/10f + 1f);

        return giro;
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
                veloc = 0f;
            }else if( Mathf.Abs(angle) > 85 ){
                desviar = false;
            }
            if(desviar){
                if(Mathf.Abs(angle) > 40){
                    giro = -Mathf.Sign(angle);
                    veloc = 1f;
                }else if(Mathf.Abs(angle) > 20){
                    giro = -Mathf.Sign(angle);
                    veloc = dist.distance > 1f ? 0.5f : 0f;
                }
            }
        }else{
            pInRange = true;
        }

        Debug.Log("Stay: " + collider.name);
        Debug.Log("angle: " + angle);
    }
}
