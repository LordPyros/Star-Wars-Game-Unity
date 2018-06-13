using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyThrough : MonoBehaviour {

    public float movementSpeed;
    private Vector2 min;

    
    void Start () {
        // bottom left of screen
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
    }
	
	// move the ship, if its out of the screen on the bottom, destroy it
	void Update () {
        transform.position += transform.right * Time.deltaTime * movementSpeed;
        if (min.y - GetComponent<SpriteRenderer>().bounds.size.y / 2 > transform.position.y)
            Destroy(gameObject);
    }
}
