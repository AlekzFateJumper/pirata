using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public List<Sprite> sprites;

    public void Init()
    {
        ShipController shipCtrl = GetComponent<ShipController>();
        shipCtrl.sprites = sprites;
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
