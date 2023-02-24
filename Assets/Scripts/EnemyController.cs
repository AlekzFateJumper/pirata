using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Enemy spawn");
        ShipController shipCtrl = GetComponent<ShipController>();
        switch(tag){
            case "Shooter":
                shipCtrl.sprites[0] = (Resources.Load<Sprite>("ship (22)"));
                shipCtrl.sprites[1] = (Resources.Load<Sprite>("ship (16)"));
                shipCtrl.sprites[2] = (Resources.Load<Sprite>("ship (10)"));
                shipCtrl.sprites[3] = (Resources.Load<Sprite>("ship (4)"));
            break;
            case "Chaser":
                shipCtrl.sprites[0] = (Resources.Load<Sprite>("ship (23)"));
                shipCtrl.sprites[1] = (Resources.Load<Sprite>("ship (17)"));
                shipCtrl.sprites[2] = (Resources.Load<Sprite>("ship (11)"));
                shipCtrl.sprites[3] = (Resources.Load<Sprite>("ship (5)"));
            break;

        }
        shipCtrl.shipSprite.sprite = shipCtrl.sprites[3];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
