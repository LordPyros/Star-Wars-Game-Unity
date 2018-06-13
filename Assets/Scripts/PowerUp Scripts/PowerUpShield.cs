using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpShield : MonoBehaviour {
    public GameObject Message;
    private Vector2 min;
    public float movementSpeed;
    private void Start()
    {
        //bottm left screen corner
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
    }

    private void Update()
    {
        // move power up down the screen
        transform.position += transform.right * Time.deltaTime * movementSpeed;
        // destroy when out of the screen on the bottom
        if (transform.position.y < min.y - (GetComponent<SpriteRenderer>().bounds.size.y / 2))
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        // check for collision with player ship
        if (col.tag == "PlayerShipTag")
        {
            // Shield power up is dealt with on player control script collision2d
            GameObject msg = Instantiate(Message);
            msg.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
    
}
