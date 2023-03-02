using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Dictionary<int,bool> navAngs;
    private bool desviando;
    private float z;

    public void Init(List<Sprite> sprites)
    {
        shipCtrl = GetComponent<ShipController>();
        shipCtrl.setSprites(sprites);
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
        player = GameObject.FindWithTag("Player");
        shipColliderId = shipCtrl.ship.GetComponent<Collider2D>().GetInstanceID();
        navAngs = new Dictionary<int, bool>() {
            [   0] = true,
            [  30] = true,
            [ -30] = true,
            [  60] = true,
            [ -60] = true,
            [  90] = true,
            [ -90] = true,
            [ 120] = true,
            [-120] = true,
            [ 150] = true,
            [-150] = true,
            [ 180] = true
        };
        deltaToPlayer = 0f;
        angleToPlayer = 0f;
        giro = 0f;
        veloc = 0f;
        desviando = false;
        z = shipCtrl.ship.transform.eulerAngles.z;
    }

    void Update()
    {
        if(shipCtrl.getHealth() <= 0) return;

        z = correctAngle(shipCtrl.ship.transform.eulerAngles.z);
        playerPos = player.transform.position;
        angleToPlayer = getAngle(playerPos);
        deltaToPlayer = deltaAngle(angleToPlayer);

        int front = (int)correctAngle(Mathf.RoundToInt(z/30f)*30);

        if( desviando || !navAngs[front] || shipCtrl.isBlocked() ) Desviar();
        else MoveToPlayer();

        object[] args = new object[2];
        args[0] = veloc;
        args[1] = giro;
        shipCtrl.Mover(args);
    }

    void MoveToPlayer(){
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
            int front = (int)correctAngle(Mathf.RoundToInt(z/30f)*30);
            veloc = navAngs[front] ?  1f : 0f;
            giro = getGiroToPlayer();
        }
    }

    void Desviar(){
        if(!desviando){
            desviando = true;
        }

        giro = getGiroTo(closestWayByKey(z));
        veloc = giro < 1f ? (1f - giro) / 2 : 0f;

        if(giro == 0f){
            int rndAng = (int)correctAngle(Mathf.RoundToInt(angleToPlayer/30f)*30);
            if( navAngs[rndAng] )
            {
                desviando = false;
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
        return Mathf.DeltaAngle(target, z);
    }

    float invertAngle(float a){
        return a > 0 ? a - 180 : a + 180;
    }

    float correctAngle(float a){
        return a > 180f ? a - 360f : (a <= -180f ? a + 360f : a );
    }

    float getGiroToPlayer(){
        // Calcula giro para olhar player.
        float abs = Mathf.Abs(deltaToPlayer);
        float g = abs > 1 ? Mathf.Sign(deltaToPlayer) : deltaToPlayer;
        return g;
    }

    float getGiroTo(float target){
        // Calcula giro para olhar para o target.
        float abs = Mathf.Abs(target);
        float g = abs > 1 ? Mathf.Sign(target) : target;
        return g;
    }

    void TriggerUpdate(NavTrigger trigger){
        navAngs[trigger.pos] = (trigger.colliders.Count == 0);
    }

    bool InRange(float n, float min, float max, bool eq = false){
        if(min > max && min >= 0 && max <= 0){
            if(eq) return n >= min || n <= max;
            return n > min || n < max;
        }
        if(eq) return n >= min && n <= max;
        return n > min && n < max;
    }

    int closestWayByKey(float number){
        return navAngs.Where(p => p.Value == true).Aggregate((x,y) => Math.Abs(x.Key-number) < Math.Abs(y.Key-number) ? x : y).Key;
    }
}
