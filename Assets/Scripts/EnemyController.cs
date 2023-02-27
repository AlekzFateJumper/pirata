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
    private float giro;
    private float veloc;
    private float angleToPlayer;
    private float deltaToPlayer;
    private Dictionary<string, NavTrigger?> angulos;

    public void Init(List<Sprite> sprites)
    {
        shipCtrl = GetComponent<ShipController>();
        shipCtrl.setSprites(sprites);
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
        player = GameObject.FindWithTag("Player");
        shipColliderId = shipCtrl.ship.GetComponent<Collider2D>().GetInstanceID();
        angulos = new Dictionary<string, NavTrigger?>() {
            ["Collider (0,0)"   ] = null,
            ["Collider (15,0)"  ] = null,
            ["Collider (-15,0)" ] = null,
            ["Collider (30,0)"  ] = null,
            ["Collider (-30,0)" ] = null,
            ["Collider (45,0)"  ] = null,
            ["Collider (-45,0)" ] = null,
            ["Collider (60,0)"  ] = null,
            ["Collider (-60,0)" ] = null,
            ["Collider (90,0)"  ] = null,
            ["Collider (-90,0)" ] = null,
            ["Collider (105,0)" ] = null,
            ["Collider (-105,0)"] = null,
            ["Collider (120,0)" ] = null,
            ["Collider (-120,0)"] = null,
            ["Collider (135,0)" ] = null,
            ["Collider (-135,0)"] = null,
            ["Collider (150,0)" ] = null,
            ["Collider (-150,0)"] = null,
            ["Collider (165,0)" ] = null,
            ["Collider (-165,0)"] = null,
            ["Collider (180,0)" ] = null,
            ["Collider (0,1)"   ] = null,
            ["Collider (15,1)"  ] = null,
            ["Collider (-15,1)" ] = null,
            ["Collider (30,1)"  ] = null,
            ["Collider (-30,1)" ] = null,
            ["Collider (45,1)"  ] = null,
            ["Collider (-45,1)" ] = null,
            ["Collider (60,1)"  ] = null,
            ["Collider (-60,1)" ] = null,
            ["Collider (90,1)"  ] = null,
            ["Collider (-90,1)" ] = null,
            ["Collider (105,1)" ] = null,
            ["Collider (-105,1)"] = null,
            ["Collider (120,1)" ] = null,
            ["Collider (-120,1)"] = null,
            ["Collider (135,1)" ] = null,
            ["Collider (-135,1)"] = null,
            ["Collider (150,1)" ] = null,
            ["Collider (-150,1)"] = null,
            ["Collider (165,1)" ] = null,
            ["Collider (-165,1)"] = null,
            ["Collider (180,1)" ] = null,
            ["Collider (0,2)"   ] = null,
            ["Collider (15,2)"  ] = null,
            ["Collider (-15,2)" ] = null,
            ["Collider (30,2)"  ] = null,
            ["Collider (-30,2)" ] = null,
            ["Collider (45,2)"  ] = null,
            ["Collider (-45,2)" ] = null,
            ["Collider (60,2)"  ] = null,
            ["Collider (-60,2)" ] = null,
            ["Collider (90,2)"  ] = null,
            ["Collider (-90,2)" ] = null,
            ["Collider (105,2)" ] = null,
            ["Collider (-105,2)"] = null,
            ["Collider (120,2)" ] = null,
            ["Collider (-120,2)"] = null,
            ["Collider (135,2)" ] = null,
            ["Collider (-135,2)"] = null,
            ["Collider (150,2)" ] = null,
            ["Collider (-150,2)"] = null,
            ["Collider (165,2)" ] = null,
            ["Collider (-165,2)"] = null,
            ["Collider (180,2)" ] = null
        };
        deltaToPlayer = 0f;
        angleToPlayer = 0f;
        giro = 0f;
        veloc = 0f;
    }

    void Update()
    {
        if(shipCtrl.getHealth() <= 0) return;

        playerPos = player.transform.position;
        angleToPlayer = getAngle(playerPos);
        deltaToPlayer = deltaAngle(angleToPlayer);
        if( shipCtrl.isBlocked() ) Unblock();
        else if( angulos["Collider (0,0)"].colliders.Count > 0 ||
            angulos["Collider (0,1)"].colliders.Count > 0 ) Desviar();
        else MoveControl();

        object[] args = new object[2];
        args[0] = veloc;
        args[1] = giro;
        shipCtrl.Mover(args);
    }

    void MoveControl(){
        float playerDist = Vector3.Distance(playerPos, transform.position);

        if(tag == "Shooter" && playerDist <= 2.5f){
            veloc = 0f;
            if ( InRange( deltaToPlayer, -45f, 45f, true ) ) {
                if ( InRange( deltaToPlayer, -2.5f, 2.5f, true ) ) {
                    giro = deltaToPlayer / 100f;
                    SendMessage("TiroFrontal");
                }else{
                    giro = getGiroToPlayer();
                }
            } else
            if ( InRange( deltaToPlayer, -180f, -45f, false ) ) {
                float sideDelta = deltaToPlayer + 90f;
                if ( InRange( sideDelta, -2.5f, 2.5f, true ) ) {
                    giro = sideDelta / 100;
                    SendMessage("TiroLateral", true);
                }else{
                    giro = Mathf.Clamp(Mathf.Sign(sideDelta) / (sideDelta >= 20f ? 1 : 20 - Mathf.FloorToInt(sideDelta)), 0f, 1f);
                }
            } else
            if ( InRange( deltaToPlayer, 45f, 180f, false ) ) {
                float sideDelta = deltaToPlayer - 90f;
                if ( InRange( sideDelta, -2.5f, 2.5f, true ) ) {
                    giro = sideDelta / 100f;
                    SendMessage("TiroLateral", false);
                }else{
                    giro = Mathf.Clamp(Mathf.Sign(sideDelta) / (sideDelta >= 20f ? 1f: 20f - Mathf.FloorToInt(sideDelta)), 0f, 1f);
                }
            }
        } else {
            veloc = angulos["Collider (0,2)"].colliders.Count > 0 ? .6f :
                    angulos["Collider (0,1)"].colliders.Count > 0 ? .3f :
                    angulos["Collider (0,0)"].colliders.Count > 0 ?  0f : 1f;
            giro = getGiroToPlayer();
        }
    }

    void Desviar(){
        int seqPos = 0;
        int seqNeg = 0;
        for ( int i = 15; i <= 180; i+=15 ) {
            seqPos = 0;
            seqNeg = 0;
            for ( int j = 0; j < 3; j++ ){
                if (i == 0 || i == 180) {
                    if(angulos["Collider (" + i + "," + j + ")"].colliders.Count == 0) {
                        seqPos++;
                        seqNeg++;
                    }else{
                        seqPos = 0;
                        seqNeg = 0;
                    }
                } else {
                    if(angulos["Collider (" + i + "," + j + ")"].colliders.Count == 0) {
                        seqPos++;
                    }else{
                        seqPos = 0;
                    }
                    if(angulos["Collider (-" + i + "," + j + ")"].colliders.Count == 0) {
                        seqNeg++;
                    }else{
                        seqNeg = 0;
                    }
                }
            }

            if(deltaToPlayer >= 0){
                if(seqPos > 1){
                    giro = 1f;
                    veloc = ((float)seqPos) / 3f;
                    return;
                }else if(seqNeg > 1){
                    giro = -1f;
                    veloc = ((float)seqNeg) / 3f;
                    return;
                }
            }else{
                if(seqNeg > 1){
                    giro = -1f;
                    veloc = ((float)seqNeg) / 3f;
                    return;
                }else if(seqPos > 1){
                    giro = 1f;
                    veloc = ((float)seqPos) / 3f;
                    return;
                }
            }
        }
    }

    void Unblock(){
        var sign = Mathf.Sign(deltaToPlayer);
        giro = sign;
        veloc = 1f;
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
        float abs = Mathf.Abs(deltaToPlayer);
        float g = abs > 1 ? Mathf.Sign(deltaToPlayer) : deltaToPlayer;
        return g;
    }

    void TriggerUpdate(NavTrigger trigger){
        angulos[trigger.name] = trigger;
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
