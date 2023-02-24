using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public List<Sprite> sprites;
    public GameObject ship;
    public SpriteRenderer shipSprite;
    public Transform lifeMask;
    public float rot_speed;
    public float speed;
    public GameObject explosion;
    public GameObject cannonBall;
    public Transform canhaoFrontal;
    public List<Transform> canhaoDir;
    public List<Transform> canhaoEsq;

    private Rigidbody2D shipBody;
    private int Health;
    private float Andar;
    private float Girar;
    private bool blocked;
    private float[] cannonWait;
    private GameObject exploding;

    void Start()
    {
        Andar = 0;
        Girar = 0;
        Health = 3;
        shipBody = ship.GetComponent<Rigidbody2D>();
        blocked = false;
        cannonWait = new float[3] { 0f, 0f, 0f };
        if(tag == "Player") shipSprite.sprite = sprites[Health];
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

        if(Andar >= 0 && !blocked) {
            var vetor = (ship.transform.up * Andar * (-speed) * Time.fixedDeltaTime);
            GetComponent<Rigidbody2D>().AddForce(vetor);
        }

        for(var i = 0; i < 3; i++) {
            if(cannonWait[i] > 0) cannonWait[i] -= Time.fixedDeltaTime;
            if(cannonWait[i] < 0) cannonWait[i] = 0;
        }
    }

    void Hit() {
        if(--Health < 0) Health = 0;
        shipSprite.sprite = sprites[Health];
        updateLifeBar();
        Explode();
    }

    void Explode() {
        exploding = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
        Invoke("Exploded", 0.25f);
    }

    void Exploded() {
        if(Health == 0) {
            StartCoroutine(FadeTo(0.0f, 1.0f));
        }else{
            Destroy(exploding);
        }
    }

    void Naufragio() {
        if(gameObject.CompareTag("Player")) {
            gameObject.SendMessageUpwards("GameOver");
        }else{
            Destroy(gameObject);
        }
    }

    void updateLifeBar() {
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
            // Para não ser atingido pela própria bala de canhão:
            if(collision.gameObject.GetComponent<cBallCtrl>().origin == GetInstanceID()) return;
            // Toma dano:
            Hit();
            // Destrói a bola de canhão:
            Destroy(collision.gameObject);
        } else if (collision.gameObject.name == "Ship") {
            // Se um barco colidir com outro barco, ambos explodem.
            Health = 0;
            updateLifeBar();
            Explode();
        } else {
            blocked = true;
            var rb2 = GetComponent<Rigidbody2D>();
            rb2.velocity = Vector2.zero;
            var vetor = (ship.transform.up * speed * Time.deltaTime);
            rb2.AddForce(vetor);
        }
    }

    void CollideExit (Collision2D collision) {
        if(collision.contactCount == 0) blocked = false;
    }

    void CollideStay (Collision2D collision) {
        ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contactPoints);
        
        foreach (ContactPoint2D contactPoint in contactPoints)
        {
            if (Math.Abs(contactPoint.normal.x + ship.transform.up.x) < 0.1
            && Math.Abs(contactPoint.normal.y + ship.transform.up.y) < 0.1 ) {
                blocked = false;
            } else {
                blocked = true;
                break;
            }
        }
    }

    void TiroFrontal () {
        if(cannonWait[0] > 0) return;
        GameObject cBall = Instantiate(cannonBall, new Vector3(canhaoFrontal.position.x, canhaoFrontal.position.y, canhaoFrontal.position.z),canhaoFrontal.rotation) as GameObject;
        cBall.GetComponent<cBallCtrl>().setOrigin(GetInstanceID());
        cBall.GetComponent<Rigidbody2D>().AddForce(canhaoFrontal.right * 1000);
        cannonWait[0] = 0.5f;
    }

    void TiroLateral (bool direita) {
        var cannons = canhaoEsq;
        if (direita) {
            if (cannonWait[1] > 0) return;
            cannons = canhaoDir;
            cannonWait[1] = 0.5f;
        } else if (cannonWait[2] > 0) return;
        else cannonWait[2] = 0.5f;

        foreach (var cannon in cannons) {
            GameObject cBall = Instantiate(cannonBall, new Vector3(cannon.position.x, cannon.position.y, cannon.position.z),cannon.rotation) as GameObject;
            cBall.GetComponent<cBallCtrl>().setOrigin(GetInstanceID());
            cBall.GetComponent<Rigidbody2D>().AddForce(cannon.right * (direita?-1000:1000));
        }
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        float alpha = transform.GetComponent<Renderer>().material.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
            transform.GetComponent<Renderer>().material.color = newColor;
            yield return null;
        }
        Naufragio();
    }
}
