using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHeavyLaser : MonoBehaviour {

    float speed; // laser speed
    Vector2 _direction; // direction of laser
    bool isReady; // to know when laser direction is set

    public GameObject ExplosionMediumSmallAnim; // small explosion prefab

    // bottom left of screen
    Vector2 min;
    // top right of screen
    Vector2 max;

    private void Awake()
    {
        speed = 2f;
        isReady = false;
    }

    // Use this for initialization
    void Start()
    {
        // get screen bounds
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
            // compute the bullets new position
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
        // detect collision between enemy laser and player or friendly ships
        if (col.tag == "PlayerShipTag" || col.tag == "FriendlyShipTag")
        {
            // this if statement stops null exceptions when 2 lasers hit in the same frame and the first one kills the ship
            if (col.transform.gameObject != null && col.transform.gameObject.activeInHierarchy)
                col.SendMessage("TakeDamage", Rules.GetHeavyLaserDamage());
            GameObject explosion = Instantiate(ExplosionMediumSmallAnim);
            explosion.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
