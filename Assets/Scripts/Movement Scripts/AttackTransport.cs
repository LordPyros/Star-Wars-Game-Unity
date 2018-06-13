using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackTransport : MonoBehaviour {

    private bool hasEnteredScreen;
    
    private float turnAwayDistance = 1.4f;
    public float speed;
    public float rotatingSpeed;
    private Rigidbody2D rb;
    private Vector2 point2Target;

    private Vector2 min;
    private Vector2 max;

    private bool turnAwayDistanceChanged;

    private bool turnedThisFrame;
    
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // bottm left screen corner
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        // top right screen corner
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        
        Invoke("SetEnteredScreen", 1.5f);
    }
    
    void FixedUpdate()
    {
        if (hasEnteredScreen)
        {
            turnedThisFrame = false;
            // check if too close to the edge of the screen, if so turn away
            // check how far away a point on the left hand side of the screen is on the same y axis as this ship
            var edge = new Vector3(min.x, transform.position.y, 0f);
            var dist = Vector3.Distance(transform.position, edge);
            if (dist < turnAwayDistance)
            {
                // check the current rotation
                
                // turn away from screen edge 
                TurnAwayFromEdge(edge);
            }
            if (!turnedThisFrame)
            {
                // check how far away a point on the right hand side of the screen is on the same y axis as this ship
                edge = new Vector3(max.x, transform.position.y, 0f);
                dist = Vector3.Distance(transform.position, edge);
                if (dist < turnAwayDistance)
                {
                    // turn away from screen edge 
                    TurnAwayFromEdge(edge);
                }
            }
            if (!turnedThisFrame)
            {
                // check how far away a point on the bottom of the screen is on the same x axis as this ship
                edge = new Vector3(transform.position.x, min.y, 0f);
                dist = Vector3.Distance(transform.position, edge);
                if (dist < turnAwayDistance)
                {
                    // turn away from screen edge 
                    TurnAwayFromEdge(edge);
                }
            }
            if (!turnedThisFrame)
            {
                // check how far away a point on the top of the screen is on the same x axis as this ship
                edge = new Vector3(transform.position.x, max.y, 0f);
                dist = Vector3.Distance(transform.position, edge);
                if (dist < turnAwayDistance)
                {
                    // turn away from screen edge 
                    TurnAwayFromEdge(edge);
                }
            }
            
            //randomise how close ship can get to edge before turning
            // if the ship doesn't turn this frame and turnawaydist hasn't been changed since last turn, randomise the turnawaydist
            // It is expected this should happen on the first frame after a completed turn
            if (!turnedThisFrame && !turnAwayDistanceChanged)
            {
                // set a new turning exit angle here

                turnAwayDistance = Random.Range(.7f, 2.5f);
                turnAwayDistanceChanged = true;
            }
            
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
            rb.velocity = transform.right * speed;
        }
    }

    void TurnAwayFromEdge(Vector3 edge)
    {
            // turn away from screen edge 
            point2Target = (Vector2)transform.position + (Vector2)edge;
            turnedThisFrame = true;
            turnAwayDistanceChanged = false;
    }

    void SetEnteredScreen()
    {
        hasEnteredScreen = true;
    }
}
