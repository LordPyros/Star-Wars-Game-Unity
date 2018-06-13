using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProtonTorpedo : MonoBehaviour {

    public float speed;
    public float rotatingSpeed;
    public GameObject explosionPrefab;
    private GameObject target;
    private Rigidbody2D rb;
    private bool hasChangedTarget;
    public bool isConcussionMissile;
    private Vector2 min;
    private Vector2 max;
    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        // automatically blow up after 8 seconds
        Invoke("BlowUp", 8f);
        // bottom left of screen
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        // top right of screen
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }
	
	
    void FixedUpdate()
    {
        // check torpedo is not too far out of screen or destroy it
        if (transform.position.x < min.x + min.x / 4 || transform.position.x > max.x + max.x / 4 || transform.position.y < min.y + min.y / 2 || transform.position.y > max.y + max.y / 2)
            Destroy(gameObject);

        if (!hasChangedTarget && LevelManager.activeCounterMeasures > 0)
        {
            // get all colliders
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector3(0, 0, 0), 12f);
            if (colliders.Length > 0)
            {
                float dist = 100f;
                // add all counter measures to an array
                foreach (Collider2D col in colliders)
                {
                    if (col.tag == "CounterMeasureTag")
                    {
                        // find closest counter measure
                        if (Vector2.Distance(transform.position, col.transform.position) < dist)
                        {
                            dist = Vector2.Distance(transform.position, col.transform.position);
                            target = col.transform.gameObject;
                            hasChangedTarget = true;
                        }
                    }
                }
            }
        }

        // check target is still alive
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
        else if (hasChangedTarget)
            BlowUp();
        else
        {
            // continue in current direction
            rb.angularVelocity = 0;
            rb.velocity = transform.right * speed;
        }
    }

    private void BlowUp()
    {
        GameObject explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        Destroy(gameObject);
    }

    void ExplosionDamage(Vector3 center, float radius, Collider2D col)
    {
        // get all colliders in range of explosion - does not hit player ship
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {

            // send message to owner of collider to take damage
            if (hitColliders[i].tag == "PlayerShipTag" || hitColliders[i].tag == "FriendlyShipTag")
            {
                // this if statement stops null exceptions when 2 item do damage hit in the same frame and the first one kills the ship and its game over
                if (col.transform.gameObject != null && col.transform.gameObject.activeInHierarchy)
                {
                    if (isConcussionMissile)
                    {
                        hitColliders[i].SendMessage("TakeDamage", Rules.GetConcussionMissileDamage());
                    }
                    else
                    {
                        hitColliders[i].SendMessage("TakeDamage", Rules.GetTorpedoDamage());
                    }
                }
            }

            i++;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // detect collision between torpedo and friendly ship, player ship, player laser or counter measure
        if (col.tag == "PlayerShipTag" || col.tag == "FriendlyShipTag" || col.tag == "CounterMeasureTag" || col.tag == "PlayerLaserTag")
        {
            ExplosionDamage(transform.position, .7f, col);
            BlowUp();
        }
    }

    public void GetTarget(GameObject tar)
    {
        target = tar;
    }
}
