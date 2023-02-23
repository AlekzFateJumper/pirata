using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public List<Sprite> sprites;
    public GameObject ship;
    public Transform lifeMask;
    public GameObject explosion;
    public float speed = 50f;

    private Rigidbody2D shipBody;
    private int Health;
    private float Andar;
    private float Girar;

    // Start is called before the first frame update
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
    // Update is called once per frame
    void Update()
    {
        Debug.Log("Multi: " + (Vector3.up * Time.deltaTime * Girar));
        Debug.Log("Multi: " + (Time.deltaTime * Girar));

        shipBody.transform.Rotate(Vector3.back * Time.deltaTime * Girar * speed);

        if(Andar >= 0){
            Debug.Log("Andar: " + Andar);
            Debug.Log("Direção: " + shipBody.transform.up);
            GetComponent<Rigidbody2D>().AddForce(shipBody.transform.up * Andar * speed);
        }
    }

    void Hit(){
        ship.GetComponent<SpriteRenderer>().sprite = sprites[--Health];
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

    void Collide (Collision2D collision) {
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
}
