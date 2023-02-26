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
    private float anguloDesvio;
    private List<Collider2D> nearObjs;

    public void Init(List<Sprite> sprites)
    {
        shipCtrl = GetComponent<ShipController>();
        shipCtrl.setSprites(sprites);
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
        player = GameObject.FindWithTag("Player");
        shipColliderId = shipCtrl.ship.GetComponent<Collider2D>().GetInstanceID();
        desviar = false;
        anguloDesvio = 0f;
        deltaToPlayer = 0f;
        nearObjs = new List<Collider2D>();
        giro = 0f;
        veloc = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;

        if(nearObjs.Count > 0) Desviar();
        else MoveControl();

        object[] args = new object[2];
        args[0] = veloc;
        args[1] = giro;
        shipCtrl.Mover(args);
        Debug.Log("Blocked: " + shipCtrl.blocked + "\r\nMover: " + veloc + " / " + giro + "\r\nObjs: " + nearObjs.Count);
    }

    void MoveControl(){
        float playerDist = Vector3.Distance(playerPos, transform.position);

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
                    giro = sideDelta == 0f ? 0 : Mathf.Sign(sideDelta);
                }
            } else
            if ( InRange( deltaToPlayer, 45, 180, false ) ) {
                float sideDelta = deltaToPlayer - 90;
                if ( InRange( sideDelta, -1, 1, true ) ) {
                    giro = sideDelta / 100;
                    if(shipCtrl.cannonWait[0] <= 0f) shipCtrl.TiroLateral(false);
                }else{
                    giro = sideDelta == 0f ? 0 : Mathf.Sign(sideDelta);
                }
            }
        } else {
            veloc = 1f;
            giro = getGiroToPlayer();
        }
    }

    void Desviar(){
        float angle = freeAngle();
        Debug.Log("Ship: " + tag + " / " + transform.position + "\r\nAngle: " + angle);
        giro = getGiroToAngle(angle);
    }

    float freeAngle(){
        float way = shipCtrl.ship.transform.eulerAngles.z;
        float playerAngle = getAngle(playerPos);
        if(nearObjs.Count == 1){
            float objAng = getAngle(nearObjs[0].transform.position);
            if(Mathf.DeltaAngle(way, objAng) < 50f) desviar = true;
            if(Mathf.DeltaAngle(objAng, playerAngle) >= 80f) return playerAngle;
            return invertAngle(objAng);
        }else if(nearObjs.Count == 2){
            float a0 = getAngle(nearObjs[0].transform.position);
            float a1 = getAngle(nearObjs[1].transform.position);
            if(Mathf.DeltaAngle(way, a0) < 50f ||
               Mathf.DeltaAngle(way, a1) < 50f) desviar = true;
            else return way;
            return invertAngle(Mathf.LerpAngle(a0, a1, .5f));
        }
        float maxRot = 360f;
        float nearAngle = way;
        foreach(var obj in nearObjs) {
            float angle = getAngle(obj.transform.position);
            float inv = invertAngle(angle);
            float near = Mathf.DeltaAngle(inv, playerAngle);
            if( Mathf.DeltaAngle(way, angle) < 50f ) desviar = true;
            if( Mathf.Abs(near) < Mathf.Abs(maxRot) ){
                maxRot = near;
                nearAngle = inv;
            }
        }
        return nearAngle;
    }

    float getAngle(Vector3 target){
        Vector3 vec = new Vector3();
        // rotaciona vetor para ficar igual a posição 0 do barco.
        vec = Quaternion.Euler(0, 0, 90) * (target - shipCtrl.transform.position);
        // retorna ângulo final
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }

    float deltaAngle(float target){
        return Mathf.DeltaAngle(target, shipCtrl.ship.transform.eulerAngles.z);
    }

    float invertAngle(float a){
        return a > 0 ? a - 180 : a + 180;
    }

    float getGiroToPlayer(){
        // Calcula giro para olhar player.
        deltaToPlayer = deltaAngle(getAngle(playerPos));
        float abs = Mathf.Abs(deltaToPlayer);
        float g = Mathf.Sign(deltaToPlayer);
        if(abs < 25f) g = g / ((25f - abs)/10f + 1f);
        return g;
    }

    float getGiroToAngle(float angle){
        // Calcula giro para olhar para o angulo.
        float deltaToAngle = deltaAngle(angle);
        float abs = Mathf.Abs(deltaToAngle);
        float g = Mathf.Sign(deltaToAngle);
        if(abs < 25f) g = g / ((25f - abs)/10f + 1f);
        return g;
    }

    void OnTriggerEnter2D(Collider2D collider){
        if( collider.GetInstanceID() == shipColliderId ||
            collider.tag == "Respawn" ||
            collider.tag == "Player"
        ) return;

        // Debug.Log("Enter: " + collider.name + "\r\nTag: " + collider.tag);

        nearObjs.Add(collider);
    }

    void OnTriggerStay2D(Collider2D collider){
        if( collider.GetInstanceID() == shipColliderId ||
            collider.tag == "Respawn" ||
            collider.tag == "Player"
        ) return;

        var dist = collider.Distance(shipCtrl.ship.GetComponent<Collider2D>()).distance;

        veloc = Mathf.Clamp(dist - 1.5f, 0f, 1f);
    }

    void OnTriggerExit2D(Collider2D collider){
        if( collider.GetInstanceID() == shipColliderId ||
            collider.tag == "Respawn" ||
            collider.tag == "Player"
        ) return;

        // Debug.Log("Exit: " + collider.name + "\r\nTag: " + collider.tag);

        nearObjs.Remove(collider);
    }

    bool InRange(float n, float min, float max, bool eq = false){
        if(min > max && min >= 0 && max <= 0){
            if(eq) return n >= min || n <= max;
            return n > min || n < max;
        }
        if(eq) return n >= min && n <= max;
        return n > min && n < max;
    }
}
