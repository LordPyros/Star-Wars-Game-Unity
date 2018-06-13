using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideDogFighter : MonoBehaviour {

    public float speed;
    public float rotatingSpeed = 150;
    private GameObject target;
    private Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {
        // Select a target
        InvokeRepeating("SwitchTargets", 0f, 8f);
        rb = GetComponent<Rigidbody2D>();
    }


    void FixedUpdate()
    {
        if (target == null || !target.activeInHierarchy)
            SwitchTargets();

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

    private void SwitchTargets()
    {
        target = GameMechanics.SelectPlayerAsTarget();
    }
}
