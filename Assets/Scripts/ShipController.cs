using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public int Health;

    private float Rotation;

    public List<Sprite> sprites;
    public 
    // Start is called before the first frame update
    void Start()
    {
        Health = 4;
        Rotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    int Hit(float x, float y){
        return --Health;
    }
}
