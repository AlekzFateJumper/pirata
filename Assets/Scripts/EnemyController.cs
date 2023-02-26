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

        if(nearObjs.Count > 0) CheckForWalls();
        else desviar = false;

        if(!desviar) MoveControl();

        object[] args = new object[2];
        args[0] = veloc;
        args[1] = giro;
        shipCtrl.Mover(args);
        Debug.Log("Desviar: " + desviar + "\r\nMover: " + veloc + " / " + giro + "\r\nObjs: " + nearObjs.Count);
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

    void CheckForWalls(){
        Collider2D collider = getNearestObj();
        Debug.Log("Mais perto: " + collider.name);

        var dist = collider.Distance(shipCtrl.ship.GetComponent<Collider2D>());
        var angle = deltaAngle(collider.transform.position);
        var abs = Mathf.Abs(angle);

        if ( abs <= 85 ){
            desviar = true;
            giro = (abs > 1f ? Mathf.Sign(angle) : angle);
            if(abs < 20){
                veloc = dist.distance > 1.5f ? .2f : 0f;
            } else if(abs < 45){
                veloc = dist.distance > 1.5f ? .5f : 0f;
            } else {
                veloc = dist.distance > 1.5f ? 1f : 0f;
            }
        }else{
            Debug.Log("Mais que 85.");
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
        Debug.Log(collider == null ? "Sem parede" : "Com parede");
        return collider;
    }

    float freeAngle(bool nearPlayer = false){
        float way = shipCtrl.ship.transform.rotation.z;
        float playerAngle = getAngle(playerPos);
        float maxRot = Mathf.Infinity;
        float nearAngle = way;
        List<float> angles = new List<float>();
        foreach(var obj in nearObjs) {
            float angle = getAngle(Vector3 obj.transform.position);
            float inv = invertAngle(angle);
            float near = Mathf.DeltaAngle(inv, playerAngle);
            if( near < maxRot ){
                maxRot = near;
                nearAngle = inv;
            }
            angles.Add(angle);
        }
        angles = angles.OrderBy(item => item.Value).ToList();
        float bigGapSize = Mathf.Infinity;
        float[] bigGap = {0f, 0f};
        float[] nearGap = {0f, 0f};
        float lastAngle = angles[angles.Count - 1];
        foreach(float angle in angles){
            float[] gap = {lastAngle, angle};
            float size = Mathf.DeltaAngle(angle, lastAngle);
            if (size < bigGap){
                bigGap = gap;
                bigGapSize = size
            }
            if(nearPlayer && InRange(player, gap[0], gap[1], true)){
                nearGap = gap;
            }
            
        }
        return way;
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

    float invertAngle(float a){
        return a > 0 ? a - 180 : a + 180;
    }

    float getGiroToPlayer(){
        // Calcula giro para olhar player.
        deltaToPlayer = deltaAngle(playerPos);
        float abs = Mathf.Abs(deltaToPlayer);
        float g = Mathf.Sign(deltaToPlayer);
        if(abs < 25f) g = g / ((25f - abs)/10f + 1f);
        return g;
    }

    float getGiroToAngle(float angle){
        // Calcula giro para olhar para o angulo.
        deltaToAngle = deltaAngle(angle);
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
