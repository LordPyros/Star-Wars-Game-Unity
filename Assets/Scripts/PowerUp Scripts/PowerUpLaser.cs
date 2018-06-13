using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpLaser : MonoBehaviour {
    public GameObject[] Message;
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
            // inform ship of laser power up
            col.SendMessage("LaserPowerUpReceived");
        }
    }
    public void SpawnLaserPowerLevelMessage(int num)
    {
        // display laser power up message
        GameObject msg = Instantiate(Message[num - 2]);
        msg.transform.position = transform.position;
        Destroy(gameObject);
    }

    
}
