using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private GameObject player;
    private Vector3 playerPos;
    private ShipController shipCtrl;
    private Vector3 auxVec;

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

        float giro = getGiroToPlayer();

        float distPlayer = (target - transform.position)

        object[] args = new object[2];
        args[0] = 0f;
        args[1] = giro;
        shipCtrl.Mover(args);
    }

    float getAngle(Vector3 target){
        Vector3 vec = new Vector3();
        // grava vetor de target para usar em outras contas.
        auxVec = (target - transform.position);
        // rotaciona vetor para ficar igual a posição 0 do barco.
        vec = Quaternion.Euler(0, 0, 90) * auxVec;
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
        
    }

    void OnTriggerStay2D(Collider2D collider){
        
    }

    void OnTriggerLeave2D(Collider2D collider){
        
    }
}
