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
    public AudioSource explosionAudio;


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
        if(Health == 0) return;

        shipBody.transform.Rotate(Vector3.back * Time.fixedDeltaTime * Girar * rot_speed);

        if(Andar >= 0 && !blocked) {
            var vetor = (ship.transform.up * Andar * (-speed) * Time.fixedDeltaTime);
            GetComponent<Rigidbody2D>().AddForce(vetor);
        }

        for(var i = 0; i < 3; i++) {
            if(cannonWait[i] > 0f) cannonWait[i] -= Time.fixedDeltaTime;
            if(cannonWait[i] < 0f) cannonWait[i] = 0f;
        }
    }

    void Hit() {
        if(Health == 0) return;
        if(--Health < 0) Health = 0;
        shipSprite.sprite = sprites[Health];
        updateLifeBar();
        Explode();
    }

    void Explode(float t = .25f) {
        if(exploding == null) exploding = Instantiate(explosion, transform.position, transform.rotation) as GameObject;
        if(Health == 0) playExplosion();
        if(!IsInvoking("ApagaFogo"))
            Invoke("Exploded", t);
    }

    void Exploded() {
        if(Health == 0) {
            StartCoroutine(FadeTo(.0f, 1.0f));
            Invoke("ApagaFogo", .8f);
        }else{
            ApagaFogo();
        }
    }

    void ApagaFogo(){
        Destroy(exploding);
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
            cBallCtrl cBC = collision.gameObject.GetComponent<cBallCtrl>();
            if(cBC.origin == GetInstanceID()) return;
            if(cBC.originTag == "Player"){
                // Aumenta o score caso acertar e tripica o valor se for o tiro final.
                int score = PlayerPrefs.GetInt("score") + (( tag == "Chaser" ? 15 : 25 ) * ( Health > 1 ? 1 : 3 ));
                PlayerPrefs.SetInt("score", score);
            }
            // Destrói a bola de canhão:
            Destroy(collision.gameObject);
            // Toma dano:
            Hit();
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
        if (collision.gameObject.CompareTag("CannonBall")) return;
        ContactPoint2D[] contactPoints = new ContactPoint2D[collision.contactCount];
        collision.GetContacts(contactPoints);

        foreach (ContactPoint2D contactPoint in contactPoints)
        {
            if (Math.Abs(contactPoint.normal.x + ship.transform.up.x) < 0.2
            && Math.Abs(contactPoint.normal.y + ship.transform.up.y) < .2 ) {
                blocked = false;
            } else {
                blocked = true;
                break;
            }
        }

        Debug.Log("Source: " + name + "/" + tag + " Name: " + collision.gameObject.name + " Tag: " + collision.gameObject.tag +
            "\r\nBlocked: " + blocked + " contactCount: " + collision.contactCount);

        float ang = getAngle(collision.transform.position);
        float dang = deltaAngle(ang);
        Debug.Log("ang: " + ang + " dang: " + dang);
        if (dang > 150 || dang < -150){
            blocked = false;
        }
    }

    float getAngle(Vector3 target){
        Vector3 vec = new Vector3();
        // rotaciona vetor para ficar igual a posição 0 do barco.
        vec = Quaternion.Euler(0, 0, 90) * (target - transform.position);
        // retorna ângulo final
        return Mathf.Atan2(vec.y, vec.x) * Mathf.Rad2Deg;
    }

    float deltaAngle(float target){
        return Mathf.DeltaAngle(target, ship.transform.eulerAngles.z);
    }

    public void TiroFrontal () {
        if(cannonWait[0] > 0) return;
        GameObject cBall = Instantiate(cannonBall, new Vector3(canhaoFrontal.position.x, canhaoFrontal.position.y, canhaoFrontal.position.z),canhaoFrontal.rotation) as GameObject;
        cBall.GetComponent<cBallCtrl>().setOrigin(GetInstanceID(), tag);
        cBall.GetComponent<Rigidbody2D>().AddForce(canhaoFrontal.right * 1000);
        cannonWait[0] = .5f;
        AudioSource audio = canhaoFrontal.GetComponent<AudioSource>();
        if (audio != null) audio.Play();
    }

    public void TiroLateral (bool direita) {
        var cannons = canhaoEsq;
        if (direita) {
            if (cannonWait[1] > 0) return;
            cannons = canhaoDir;
            cannonWait[1] = .5f;
        } else if (cannonWait[2] > 0) return;
        else cannonWait[2] = .5f;
        
        foreach (var cannon in cannons) {
            GameObject cBall = Instantiate(cannonBall, new Vector3(cannon.position.x, cannon.position.y, cannon.position.z),cannon.rotation) as GameObject;
            cBall.GetComponent<cBallCtrl>().setOrigin(GetInstanceID(), tag);
            cBall.GetComponent<Rigidbody2D>().AddForce(cannon.right * (direita?-1000:1000));
        }

        AudioSource audio = cannons[0].GetComponent<AudioSource>();
        if (audio != null) audio.Play();
    }

    public void setSprites(List<Sprite> s){
        sprites = new List<Sprite>(s);
    }

    public void playExplosion(){
        if( explosionAudio != null ) explosionAudio.Play();
    }

    public int getHealth(){
        return Health;
    }

    public bool isBlocked(){
        return blocked;
    }
    public float getCannonWait(int i){
        return cannonWait[i];
    }

    IEnumerator FadeTo(float aValue, float aTime)
    {
        Renderer rend = transform.GetComponentInChildren<Renderer>();
        float alpha = rend.material.color.a;
        for (float t = .0f; t < 1.0f; t += Time.deltaTime / aTime)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
            rend.material.color = newColor;
            yield return null;
        }
        Naufragio();
    }
}
