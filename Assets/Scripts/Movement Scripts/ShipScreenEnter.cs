using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipScreenEnter : MonoBehaviour {

    // This script is used to bring ships with secondary movement scripts, into the screen from the top

    public float movementSpeed = 1.4f;
    private float time;
    // Use this for initialization
    void Start()
    {
        time = Time.time;
        // have ship enter on a slight random angle (looks a bit unnatural when they come in straight)
        transform.Rotate(Vector3.forward * Random.Range(-10,11));
    }

    // Update is called once per frame
    void Update()
    {
        // move ship in its forward direction for a short period of time (1.5s)
        if (Time.time - time < 1.5f)
        {
            transform.position += transform.right * Time.deltaTime * movementSpeed;
        }
        // once time expires, disable script
        else
        {
            this.enabled = false;
        }
    }
}
