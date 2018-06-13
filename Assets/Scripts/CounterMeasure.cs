using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CounterMeasure : MonoBehaviour {
    private Rigidbody2D rb;

    public float speed = 5f;
    public float rotatingSpeed = 200;

    private Vector2 direction;

    public GameObject explosionPrefab;
    private bool missileFound;
    private GameObject targetMissile;

    // Use this for initialization
    void Start () {
        // notify level manager a counter measure is live
        LevelManager.activeCounterMeasures++;
        rb = GetComponent<Rigidbody2D>();

        // search for a torepdos 10 times a second
        CheckForTorpedos();
        GetDirection();
        InvokeRepeating("CheckForTorpedos", .1f, .1f);
        Invoke("Explode", 1.4f);
    }

    private void Update()
    {
        // Move
        if (!missileFound)
        {
            // get the CMs current position
            Vector2 position = transform.position;
            // compute the CMs new position
            position += direction * speed * Time.deltaTime;

            // update the CMs position
            transform.position = position;
        }
    }

    private void FixedUpdate()
    {
        if (missileFound)
        {
            // check target missile is still alive, otherwise continue on current path
            if (targetMissile != null)
            {
                // get direction 
                Vector2 point2Target = (Vector2)transform.position - (Vector2)targetMissile.transform.position;

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
    }


    private void GetDirection()
    {
        // if theres a missile, rotate to the closest horizontal axis facing the target
        if (missileFound)
        {
            if (transform.position.x < targetMissile.transform.position.x)
            {
                transform.rotation *= Quaternion.Euler(0, 0, Random.Range(-10, 20));
            }
            else
            {
                transform.rotation *= Quaternion.Euler(0, 0, Random.Range(160, 190));
            }
        }
        // if no target, launch in a random horizontal direction
        else
        {
            int leftOrRight = Random.Range(0, 2);
            if (leftOrRight == 0)
            {
                leftOrRight = Random.Range(-10, 20);
            }
            else
            {
                leftOrRight = Random.Range(160, 190);
            }
            transform.rotation *= Quaternion.Euler(0, 0, leftOrRight);
        }
        
        

        direction = transform.right;
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "EnemyProtonTorpedoTag" || col.tag == "EnemyConcussionMissileTag")
        {
            Explode();
        }
    }
    private void Explode()
    {
        GameObject go = Instantiate(explosionPrefab);
        go.transform.position = transform.position;
        // notify level manager that counter measure is dead
        LevelManager.activeCounterMeasures--;
        if (LevelManager.activeCounterMeasures < 0)
            LevelManager.activeCounterMeasures = 0;
        Destroy(gameObject);
    }

    private void CheckForTorpedos()
    {
        if (!missileFound)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector3(0, 0, 0), 12f);
            if (colliders.Length > 0)
            {
                float dist = 100f;
                // add all missiles to an array
                foreach (Collider2D col in colliders)
                {
                    if (col.tag == "EnemyProtonTorpedoTag" || col.tag == "EnemyConcussionMissileTag")
                    {
                        // find closest counter measure
                        if (Vector2.Distance(transform.position, col.transform.position) < dist)
                        {
                            dist = Vector2.Distance(transform.position, col.transform.position);
                            targetMissile = col.transform.gameObject;
                            missileFound = true;
                        }
                    }
                }
            }
        }
        else
        {
            CancelInvoke("CheckForTorpedos");
        }
    }
    
}
