using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyDogFighter : MonoBehaviour {

    public float turnAwayDistance = 1.8f;
    public float speed;
    public float rotatingSpeed = 150;
    private GameObject target;
    private Rigidbody2D rb;
    private Vector2 point2Target;
    private int targetsSoFar;
    Vector2 min;
    Vector2 max;
    private bool checkingForNewTarget;
    // Use this for initialization
    void Start()
    {
        // Select a target
        InvokeRepeating("FindTarget", 0f, .3f);
        rb = GetComponent<Rigidbody2D>();

        // set screen bounds
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }


    void FixedUpdate()
    {
        if (targetsSoFar > 5)
        {
            SetRotationAndSpeed();
            CheckIfOutsideScreen();
            return;
        }

        // if there is no target, check if a search already exists
        // if if does, ship continues on currect path
        // if it doesn't, start a new search


        if (target != null)
        {
            
            // target is not dead, move toward or away if too close
            if (target.activeInHierarchy)
            {
                // check if too close, if so turn away from player ship instead
                var distance = Vector3.Distance(transform.position, target.transform.position);
                if (distance > turnAwayDistance)  // distance ships will begin to turn away from player ship 
                {
                    // get direction 
                    point2Target = (Vector2)transform.position - (Vector2)target.transform.position;
                }
                else
                {
                    // turn away - not sure if this will turn completely away 
                    point2Target = (Vector2)transform.position + (Vector2)target.transform.position;
                }

                point2Target.Normalize();
                SetRotationAndSpeed();
            }
            // no target, continue at current speed and same rotation
            else
            {
                rb.angularVelocity = 0;
                // set speed
                rb.velocity = transform.right * speed;
            }
            
        }
        // no target, continue at current speed and same rotation
        else
        {
            if (!checkingForNewTarget)
            {
                checkingForNewTarget = true;
                InvokeRepeating("FindTarget", 0f, .3f);
            }
            rb.angularVelocity = 0;
            // set speed
            rb.velocity = transform.right * speed;
        }
    }

    private void FindTarget()
    {
        // find an enemy ship and set it as target
        target = GameObject.FindGameObjectWithTag("EnemyShipTag");
        if (target != null)
        {
            CancelInvoke();
            checkingForNewTarget = false;
            targetsSoFar++;
        }
    }

    void SetRotationAndSpeed()
    {
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

    void CheckIfOutsideScreen()
    {
        // destroy if outside screen
        if ((transform.position.x < min.x) || (transform.position.x > max.x) || (transform.position.y < min.y) || (transform.position.y > max.y))
        {
            Destroy(gameObject);
        }
    }
}
