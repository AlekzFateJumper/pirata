using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ShipController shipCtrl = GetComponent<ShipController>();
        shipCtrl.sprites.Clear();
        switch(tag){
            case "Shooter":
                shipCtrl.sprites.Add(Resources.Load<Sprite>("ship (22)"));
                shipCtrl.sprites.Add(Resources.Load<Sprite>("ship (16)"));
                shipCtrl.sprites.Add(Resources.Load<Sprite>("ship (10)"));
                shipCtrl.sprites.Add(Resources.Load<Sprite>("ship (4)"));
            break;
            case "Chaser":
                shipCtrl.sprites.Add(Resources.Load<Sprite>("ship (23)"));
                shipCtrl.sprites.Add(Resources.Load<Sprite>("ship (17)"));
                shipCtrl.sprites.Add(Resources.Load<Sprite>("ship (11)"));
                shipCtrl.sprites.Add(Resources.Load<Sprite>("ship (5)"));
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
