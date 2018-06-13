using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorvetteMover : MonoBehaviour {
    private float movementSpeed = 1.4f;
    private float randomMovement;
    private float time;
    // Use this for initialization
    void Start () {
        // move ship onto screen then move it a semi random distance
        randomMovement = GetComponent<SpriteRenderer>().bounds.size.y - (GetComponent<SpriteRenderer>().bounds.size.y / 5) + Random.Range(0f, 1f);
        
        time = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        
            if (Time.time - time < randomMovement)
            {
                transform.position += transform.right * Time.deltaTime * movementSpeed;
            }
            else
            {
                enabled = false;
            }
        
    }
}
