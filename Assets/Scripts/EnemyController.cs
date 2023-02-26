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

    // Update is called once per frame
    void Update()
    {
        if(shipCtrl.getHealth() <= 0) return;

        playerPos = shipCtrl.ship.transform.position;
        angleToPlayer = getAngle(playerPos);
        deltaToPlayer = deltaAngle(angleToPlayer);

        if( angulos["Collider (0,0)"].colliders.Count > 0 ||
            angulos["Collider (0,1)"].colliders.Count > 0 ||
            angulos["Collider (0,2)"].colliders.Count > 0 ) Desviar();
        else MoveControl();

        object[] args = new object[2];
        args[0] = veloc;
        args[1] = giro;
        shipCtrl.Mover(args);
        Debug.Log("Source: " + name + "/" + tag + " Blocked: " + shipCtrl.isBlocked() + " Mover: " + veloc + " / " + giro +
            "\r\nAngulo 0: "   + angulos["Collider (0,0)"  ].colliders.Count + "/" + angulos["Collider (0,1)"  ].colliders.Count + "/" + angulos["Collider (0,2)"  ].colliders.Count +
            "\r\nAngulo 15: "  + angulos["Collider (15,0)" ].colliders.Count + "/" + angulos["Collider (15,1)" ].colliders.Count + "/" + angulos["Collider (15,2)" ].colliders.Count +
            "\r\nAngulo -15: " + angulos["Collider (-15,0)"].colliders.Count + "/" + angulos["Collider (-15,1)"].colliders.Count + "/" + angulos["Collider (-15,2)"].colliders.Count );
    }

    void MoveControl(){
        float playerDist = Vector3.Distance(playerPos, transform.position);

        if(tag == "Shooter" && playerDist <= 2f){
            veloc = 0f;
            if ( InRange( deltaToPlayer, -45, 45, true ) ) {
                if ( InRange( deltaToPlayer, -1, 1, true ) ) {
                    giro = deltaToPlayer / 100;
                    if(shipCtrl.getCannonWait(0) <= 0f) shipCtrl.TiroFrontal();
                }else{
                    giro = getGiroToPlayer();
                }
            } else
            if ( InRange( deltaToPlayer, -180, -45, false ) ) {
                float sideDelta = deltaToPlayer + 90;
                if ( InRange( sideDelta, -1, 1, true ) ) {
                    giro = sideDelta / 100;
                    if(shipCtrl.getCannonWait(1) <= 0f) shipCtrl.TiroLateral(true);
                }else{
                    giro = sideDelta == 0f ? 0 : Mathf.Sign(sideDelta);
                }
            } else
            if ( InRange( deltaToPlayer, 45, 180, false ) ) {
                float sideDelta = deltaToPlayer - 90;
                if ( InRange( sideDelta, -1, 1, true ) ) {
                    giro = sideDelta / 100;
                    if(shipCtrl.getCannonWait(2) <= 0f) shipCtrl.TiroLateral(false);
                }else{
                    giro = Math.Abs(sideDelta) == 0f ? 0 : Mathf.Sign(sideDelta);
                }
            }
        } else {
            veloc = 1f;
            giro = getGiroToPlayer();
        }
    }

    void Desviar(){
        int qtdPos = 0;
        int qtdNeg = 0;
        int seqPos = 0;
        int seqNeg = 0;
    
        for ( int i = 0; i <= 180; i+=15 ) {
            for ( int j = 0; j < 3; j++ ){
                if (i == 0 || i == 180) {
                    qtdPos += (3-j) * angulos["Collider (" + i + "," + j + ")"].colliders.Count;
                    qtdNeg += (3-j) * angulos["Collider (" + i + "," + j + ")"].colliders.Count;
                    if(angulos["Collider (" + i + "," + j + ")"].colliders.Count == 0) {
                        seqPos++;
                        seqNeg++;
                    }else{
                        seqPos = 0;
                        seqNeg = 0;
                    }
                } else {
                    qtdPos += (3-j) * angulos["Collider (" + i + "," + j + ")"].colliders.Count;
                    if(angulos["Collider (" + i + "," + j + ")"].colliders.Count == 0) {
                        seqPos++;
                    }else{
                        if(j < 2) seqPos = 0;
                    }
                    qtdNeg += (3-j) * angulos["Collider (-" + i + "," + j + ")"].colliders.Count;
                    if(angulos["Collider (-" + i + "," + j + ")"].colliders.Count == 0) {
                        seqNeg++;
                    }else{
                        if(j < 2) seqNeg = 0;
                    }
                }
            }
            Debug.Log("Source: " + name + "/" + tag + " Blocked: " + shipCtrl.isBlocked() + " Mover: " + veloc + " / " + giro +
                "\r\nAngulo 0: "   + angulos["Collider (0,0)"  ].colliders.Count + "/" + angulos["Collider (0,1)"  ].colliders.Count + "/" + angulos["Collider (0,2)"  ].colliders.Count +
                "\r\nAngulo 15: "  + angulos["Collider (15,0)" ].colliders.Count + "/" + angulos["Collider (15,1)" ].colliders.Count + "/" + angulos["Collider (15,2)" ].colliders.Count +
                "\r\nAngulo -15: " + angulos["Collider (-15,0)"].colliders.Count + "/" + angulos["Collider (-15,1)"].colliders.Count + "/" + angulos["Collider (-15,2)"].colliders.Count
            );

            if(Mathf.Sign(deltaToPlayer) > 0){
                if(seqPos > 3){
                    giro = 1;
                    break;
                }else if(seqNeg > 3){
                    giro = -1;
                    break;
                }
            }else{
                if(seqNeg > 3){
                    giro = -1;
                    break;
                }else if(seqPos > 3){
                    giro = 1;
                    break;
                }
            }
        }
        if(Mathf.Sign(deltaToPlayer) > 0){
            if(qtdPos >= qtdNeg && giro == 0){
                giro = 1;
            }else if(qtdNeg > qtdPos && giro == 0){
                giro = -1;
            }
        }else{
            if(qtdNeg >= qtdPos && giro == 0){
                giro = -1;
            }else if(qtdPos > qtdNeg && giro == 0){
                giro = 1;
            }
        }
        veloc = ( ( angulos["Collider (0,0)"].colliders.Count > 0 || angulos["Collider (" + (giro * 15) + ",0)"].colliders.Count > 0 ) ? 0f : 
                ( ( angulos["Collider (0,1)"].colliders.Count > 0 || angulos["Collider (" + (giro * 15) + ",1)"].colliders.Count > 0 ) ? .2f : 
                ( ( angulos["Collider (0,2)"].colliders.Count > 0 || angulos["Collider (" + (giro * 15) + ",2)"].colliders.Count > 0 ) ? .5f : .75f ) ) );
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
        Debug.Log("GiroToPlayer: " + g + " deltaToPlayer: " + deltaToPlayer);
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
