using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaserX3 : MonoBehaviour {

    public GameObject ExplosionSmallPrefab;
    float speed;
    // Use this for initialization
    void Start()
    {
        speed = 8f;
    }

    // Update is called once per frame
    void Update()
    {
        // Get the laser's current position
        Vector2 position = transform.position;

        // computer the laser's new position
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);

        // update the laser's position
        transform.position = position;

        // this is top right point of screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // if the laser went outside the screen on the top, then destroy the laser
        if (transform.position.y > max.y)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {

        // detect collision of the player laser with enemy ship
        if (col.tag == "EnemyShipTag" || col.tag == "EnemyCapitalShipTag" || col.tag == "LeftShieldGenTag" || col.tag == "RightShieldGenTag")
        {
            col.SendMessage("TakeDamage", Rules.GetLaserDamage() * 3);
            GameObject go = Instantiate(ExplosionSmallPrefab);
            go.transform.position = transform.position;
            Destroy(gameObject);
        }
    }
}
