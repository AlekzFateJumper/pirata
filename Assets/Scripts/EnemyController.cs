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
                shipCtrl.sprites.Add("ship (22).png");
                shipCtrl.sprites.Add("ship (16).png");
                shipCtrl.sprites.Add("ship (10).png");
                shipCtrl.sprites.Add("ship (4).png");
            break;
            case "Chaser":
                shipCtrl.sprites.Add("ship (23).png");
                shipCtrl.sprites.Add("ship (17).png");
                shipCtrl.sprites.Add("ship (11).png");
                shipCtrl.sprites.Add("ship (5).png");
            break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
