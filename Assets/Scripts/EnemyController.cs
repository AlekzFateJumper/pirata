using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{

    private GameObject player;
    private Vector2 playerPos;
    private ShipController shipCtrl;

    public void Init(List<Sprite> sprites)
    {
        shipCtrl = GetComponent<ShipController>();
        shipCtrl.setSprites(sprites);
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = player.transform.position;
        Debug.Log("Target rotation:" + (playerPos - transform.position));
        Debug.Log("Ship rotation:" + .transform.back));

        object[] args = new object[2];
        args[0] = 0f;
        args[1] = .5f;
        shipCtrl.Mover(args);
    }

    void OnTriggerEnter(Collision2D collision){
        
    }

    void OnTriggerStay(Collision2D collision){
        
    }

    void OnTriggerLeave(Collision2D collision){
        
    }
}
