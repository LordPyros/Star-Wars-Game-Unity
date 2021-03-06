﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyProtonTorpedo : MonoBehaviour {

    public float speed = 6f;
    public float rotatingSpeed = 250;
    public GameObject explosionPrefab;
    private GameObject target;
    private Vector2 min;
    private Vector2 max;

    Rigidbody2D rb;
    // Use this for initialization
    void Start()
    {
        // if friendly missile is fired, an enemy ship should exist, select one
        target = GameObject.FindGameObjectWithTag("EnemyShipTag");

        rb = GetComponent<Rigidbody2D>();
        Invoke("BlowUp", 8f);

        // bottom left of screen
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        // top right of screen
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // check torpedo is not too far out of screen or destroy it
        if (transform.position.x < min.x + min.x / 4 || transform.position.x > max.x + max.x / 4 || transform.position.y < min.y + min.y / 2 || transform.position.y > max.y + max.y / 2)
            Destroy(gameObject);

        // check target is still alive or self destruct
        if (target != null)
        {
            // get direction 
            Vector2 point2Target = (Vector2)transform.position - (Vector2)target.transform.position;

            point2Target.Normalize();
            // set rotation
            float value = Vector3.Cross(point2Target, transform.right).z;

            if (value > 0)
            {
                rb.angularVelocity = rotatingSpeed;
            }
            else if (value < 0)
            {
                rb.angularVelocity = -rotatingSpeed;
            }
            else
            {
                rotatingSpeed = 0;
            }
            // set speed
            rb.velocity = transform.right * speed;
        }
        else
        {
            rb.angularVelocity = 0;
            rb.velocity = transform.right * speed;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // detect collision between torpedo and enemy ship
        if (col.tag == "EnemyShipTag" || col.tag == "EnemyCapitalShip")
        {
            col.SendMessage("TakeDamage", Rules.GetTorpedoDamage());
            BlowUp();
        }
    }
    void BlowUp()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
}
