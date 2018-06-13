using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtonTorpedo : MonoBehaviour {
    //public GameObject ProtonTorpedoPrefab;
    public float speed = 5f;
    public float rotatingSpeed = 200;
    public GameObject explosionPrefab;
    private GameObject target;
    private Rigidbody2D rb;
    private Vector2 min;
    private Vector2 max;

    // Use this for initialization
    void Start() {
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
        // detect collision between torpedo and any enemy 
        if (GameMechanics.CheckColliderAgainstAllEnemyTags(col))
        {
            ExplosionDamage(transform.position, .7f, col);
            BlowUp();
        }
    }
    void BlowUp()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
    }
    void ExplosionDamage(Vector3 center, float radius, Collider2D col)
    {
        // get all colliders in range of explosion
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
                // send message to owner of collider to take damage
                if (GameMechanics.CheckColliderAgainstAllEnemyTags(hitColliders[i]))
                {
                    hitColliders[i].SendMessage("TakeDamage", Rules.GetTorpedoDamage());
                }
            i++;
        }
    }

    public void SetTarget(GameObject tar)
    {
        target = tar;
    }

}
