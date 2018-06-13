using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDestroyerImp1Mover : MonoBehaviour {
    float movementSpeed = 1.2f;
    float movementDistance; //3.05f
    private float time;
    // Use this for initialization
    void Start () {
        time = Time.time;
        movementDistance = GetComponent<SpriteRenderer>().bounds.size.y - (GetComponent<SpriteRenderer>().bounds.size.y /10);
    }
	
	// Update is called once per frame
	void Update () {
        if (Time.time - time < movementDistance)
        {
            transform.position += transform.right * Time.deltaTime * movementSpeed;
        }
        else
        {
            this.enabled = false;
        }
    }
}
