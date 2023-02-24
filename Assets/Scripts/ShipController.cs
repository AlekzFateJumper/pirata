using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public List<Sprite> sprites;
    public GameObject ship;
    public SpriteRenderer shipSprite;
    public Transform lifeMask;
    public GameObject explosion;
    public float rot_speed;
    public float speed;

    private Rigidbody2D shipBody;
    private int Health;
    private float Andar;
    private float Girar;

    void Start()
    {
        Health = 3;
        shipBody = ship.GetComponent<Rigidbody2D>();
    }

    public void Mover(object args)
    {
        object[] a = (object[]) args;
        Andar = (float) a[0];
        Girar = (float) a[1];
    }

    void FixedUpdate()
    {
        shipBody.transform.Rotate(Vector3.back * Time.fixedDeltaTime * Girar * rot_speed);

        if(Andar >= 0){
            var vetor = (ship.transform.up * Andar * (-speed) * Time.fixedDeltaTime);
            GetComponent<Rigidbody2D>().AddForce(vetor);
        }
    }

    void Hit(){
        shipSprite.sprite = sprites[--Health];
        updateLifeBar();
        if(Health <= 0){
            Explode();
        }
    }

    void Explode(){
        //var exploding = Instantiate(explosion, transform.position, transform.rotation);
        //exploding.Sink();
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

    void CollideEnter (Collision2D collision) {
        if (collision.gameObject.CompareTag("CannonBall")) {
            //var exploding = Instantiate(explosion, transform.position, transform.rotation);
            //exploding.Weak();
            Hit();
        }else if((CompareTag("Chaser") || CompareTag("Player")) && (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Chaser"))){
            Health = 0;
            updateLifeBar();
            Explode();
        }
    }

    void CollideExit (Collision2D collision) {
    }

    void CollideStay (Collision2D collision) {
    }
}
