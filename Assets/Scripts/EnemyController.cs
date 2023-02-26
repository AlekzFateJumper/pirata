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
    private List<Collider2D> nearObjs;

    public void Init(List<Sprite> sprites)
    {
        shipCtrl = GetComponent<ShipController>();
        shipCtrl.setSprites(sprites);
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
        player = GameObject.FindWithTag("Player");
        shipColliderId = shipCtrl.ship.GetComponent<Collider2D>().GetInstanceID();
        desviar = false;
        deltaToPlayer = 0f;
        giro = 0f;
        veloc = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        playerPos = player.transform.position;

        if(nearObjs.Count > 0) CheckForWalls();
        else desviar = false;

        if(!desviar) MoveControl();

        object[] args = new object[2];
        args[0] = veloc;
        args[1] = giro;
        shipCtrl.Mover(args);
        // Debug.Log("Desviar: " + desviar + "\r\nMover: " + veloc + " / " + giro);
    }

    void MoveControl(){
        float playerDist = Vector3.Distance(playerPos, shipCtrl.ship.transform.position);

        if(playerDist <= 2f && shipCtrl.tag == "Shooter"){
            veloc = 0f;
            if ( InRange( deltaToPlayer, -45, 45, true ) ) {
                if ( InRange( deltaToPlayer, -1, 1, true ) ) {
                    giro = deltaToPlayer / 100;
                    if(shipCtrl.cannonWait[0] <= 0f) shipCtrl.TiroFrontal();
                }else{
                    giro = getGiroToPlayer();
                }
            } else
            if ( InRange( deltaToPlayer, -180, -45, false ) ) {
                float sideDelta = deltaToPlayer + 90;
                if ( InRange( sideDelta, -1, 1, true ) ) {
                    giro = sideDelta / 100;
                    if(shipCtrl.cannonWait[0] <= 0f) shipCtrl.TiroLateral(true);
                }else{
                    giro = Mathf.Sign(sideDelta);
                }
            } else
            if ( InRange( deltaToPlayer, 45, 180, false ) ) {
                float sideDelta = deltaToPlayer - 90;
                if ( InRange( sideDelta, -1, 1, true ) ) {
                    giro = sideDelta / 100;
                    if(shipCtrl.cannonWait[0] <= 0f) shipCtrl.TiroLateral(false);
                }else{
                    giro = Mathf.Sign(sideDelta);
                }
            }
        } else {
            veloc = 1f;
            giro = getGiroToPlayer();
        }
    }

    void CheckForWalls(){
        Collider2D collider = getNearestObj();

        var angle = deltaAngle(collider.transform.position);
        var dist = collider.Distance(shipCtrl.ship.GetComponent<Collider2D>());
        var abs = Mathf.Abs(angle);

        if ( abs <= 75 ){
            desviar = true;
            if(abs > 40){
                giro = - Mathf.Sign(angle);
                veloc = dist.distance > 1.2f ? 1f : 0f;
            }else if(abs > 20){
                giro = - Mathf.Sign(angle);
                veloc = dist.distance > 1.2f ? 0.5f : 0f;
            }
        }else{
            desviar = false;
        }
    }

    Collider2D? getNearestObj(){
        float minDist = Mathf.Infinity;
        Collider2D? collider = null;
        foreach(var obj in nearObjs) {
            var dist = obj.Distance(shipCtrl.ship.GetComponent<Collider2D>()).distance;
            if(dist < minDist){
                minDist = dist;
                collider = obj;
            }
        }
        return collider;
    }

    float getAngle(Vector3 target){
        Vector3 vec = new Vector3();
        // rotaciona vetor para ficar igual a posição 0 do barco.
        vec = Quaternion.Euler(0, 0, 90) * (target - shipCtrl.ship.transform.position);
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
        float g = Mathf.Sign(deltaToPlayer);
        if(abs < 25f) g = g / ((25f - abs)/10f + 1f);
        return g;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if( collider.GetInstanceID() == shipColliderId ||
            collider.name == "Main Camera" ||
            collider.tag == "Respawn" ||
            collider.tag == "Player"
        ) return;

        // Debug.Log("Enter: " + collider.name + "\r\nTag: " + collider.tag);

        nearObjs.Add(collider);
    }

    void OnTriggerExit2D(Collider2D collider){
        if( collider.GetInstanceID() == shipColliderId ||
            collider.name == "Main Camera" ||
            collider.tag == "Respawn" ||
            collider.tag == "Player"
        ) return;

        // Debug.Log("Exit: " + collider.name + "\r\nTag: " + collider.tag);

        nearObjs.Remove(collider);
    }

    bool InRange(float n, float min, float max, bool eq = false){
        if(eq) return n >= min && n <= max;
        return n > min && n < max;
    }
}
