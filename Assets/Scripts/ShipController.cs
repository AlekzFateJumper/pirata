using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private int Health;
    public float Rotation;

    public List<Sprite> sprites;
    public GameObject ship;

    public Transform lifeMask;

    // Start is called before the first frame update
    void Start()
    {
        Health = 3;
        Rotation = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Hit(){
        ship.GetComponent<SpriteRenderer>.sprite = sprites[--Health];
        updateLifeBar();
        if(Health <= 0){
            Explode();
        }
    }

    void Explode(){
        
    }

    void Exploded(){
        if(gameObject.CompareTag("Player")){
            gameObject.SendMessageUpwards("GameOver");
        }
    }

    void updateLifeBar(){
        Vector3 pos = new Vector3(0,0,0);
        switch (Health)
        {
            case 2:
                pos = new Vector3(-.4f,0,0);
            break;
            case 1:
                pos = new Vector3(-.6f,0,0);
            break;
            case 0:
                pos = new Vector3(-1f,0,0);
            break;
        }
        lifeMask.position = pos;
    }
}
