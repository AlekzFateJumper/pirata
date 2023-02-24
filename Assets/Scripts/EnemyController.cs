using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public void Init(List<Sprite> sprites)
    {
        ShipController shipCtrl = GetComponent<ShipController>();
        shipCtrl.setSprites(sprites);
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
