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
    private Dictionary<int, int> angulos;

    public void Init(List<Sprite> sprites)
    {
        shipCtrl = GetComponent<ShipController>();
        shipCtrl.setSprites(sprites);
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
        player = GameObject.FindWithTag("Player");
        shipColliderId = shipCtrl.ship.GetComponent<Collider2D>().GetInstanceID();
        deltaToPlayer = 0f;
        angleToPlayer = 0f;
        giro = 0f;
        veloc = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(shipCtrl.getHealth() <= 0) return;

        playerPos = player.transform.position;
        angleToPlayer = getAngle(playerPos);
        deltaToPlayer = deltaAngle(angleToPlayer);

        if(angulos[0] > 0 || angulos[15] > 0 || angulos[-15] > 0) Desviar();
        else MoveControl();

        object[] args = new object[2];
        args[0] = veloc;
        args[1] = giro;
        shipCtrl.Mover(args);
        Debug.Log("Source: " + name + "/" + tag + " Blocked: " + shipCtrl.isBlocked() + " Mover: " + veloc + " / " + giro );
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
    
        for(int i = 0; i <= 180; i+=15){
            if (i == 0 || i == 180) {
                if(!angulos.ContainsKey(i)) angulos[i] = 0;
                qtdPos += angulos[i];
                qtdNeg += angulos[i];
                if(angulos[i] == 0) {
                    seqPos++;
                    seqNeg++;
                }
            } else {
                if(!angulos.ContainsKey(i)) angulos[i] = 0;
                qtdPos += angulos[i];
                if(angulos[i] == 0) {
                    seqPos++;
                }
                if(!angulos.ContainsKey(-i)) angulos[-i] = 0;
                qtdNeg += angulos[-i];
                if(angulos[-i] == 0) {
                    seqNeg++;
                }
            }
            if(seqPos >= 3){
                giro = 1;
                break;
            }else if(seqNeg >= 3){
                giro = -1;
                break;
            }else if(qtdPos >= qtdNeg && i == 180){
                giro = 1;
                break;
            }else if(qtdNeg > qtdPos && i == 180){
                giro = -1;
                break;
            }
        }
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
        float g = Mathf.Sign(deltaToPlayer);
        if(abs < 25f) g = g / ((25f - abs)/10f + 1f);
        return g;
    }

    void TriggerEnter(Collider2D collider){
        int z = Mathf.RoundToInt(collider.transform.localEulerAngles.z);
        int qtd = 0;
        if(angulos.ContainsKey(z)) qtd = angulos[z];
        angulos[z] = ++qtd;
    }

    void TriggerStay(Collider2D collider){

    }

    void TriggerExit(Collider2D collider){
        int z = Mathf.RoundToInt(collider.transform.localEulerAngles.z);
        angulos[z] = angulos[z] - 1;
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
