using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyLaser : MonoBehaviour {

    public float speed; // laser speed
    Vector2 _direction; // direction of laser
    bool isReady; // to know when laser direction is set

    public GameObject ExplosionSmallAnim; // small explosion prefab

    // bottom left of screen
    Vector2 min;
    // top right of screen
    Vector2 max;

    private void Awake()
    {
        isReady = false;
    }

    // Use this for initialization
    void Start()
    {
        // set screen bounds
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }

    public void SetDirection(Vector2 direction)
    {
        _direction = direction.normalized;

        isReady = true;
    }
    public void SetForwardDirection(Vector2 direction)
    {
        _direction = direction;
        isReady = true;

    }

    // Update is called once per frame
    void Update()
    {
        if (isReady)
        {
            // get the laser's current position
            Vector2 position = transform.position;

            // compute the lasers new position
            position += _direction * speed * Time.deltaTime;

            // update the laser's position
            transform.position = position;

            // destroy laser if out of screen
            if ((transform.position.x < min.x) || (transform.position.x > max.x) || (transform.position.y < min.y) || (transform.position.y > max.y))
            {
                Destroy(gameObject);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        // detect collision between friendly laser and enemy ships
        if (col.tag == "EnemyShipTag" || col.tag == "EnemyCapitalShipTag")
        {
            col.SendMessage("TakeDamage", Rules.GetLaserDamage());
            GameObject explosion = Instantiate(ExplosionSmallAnim);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
