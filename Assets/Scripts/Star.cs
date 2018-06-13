using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Star : MonoBehaviour
{

    public float speed; //star speed


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Get current pos of star
        Vector2 position = transform.position;

        // compute the star's new pos
        position = new Vector2(position.x, position.y + speed * Time.deltaTime);

        // update star's pos
        transform.position = position;

        // bottom left of screen
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        // top right
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        //If the star goes outside the screen on the bottm
        // then position the star on the top of the screen
        // and randomly between left and right side of screen
        if (transform.position.y < min.y)
        {
            transform.position = new Vector2(Random.Range(min.x, max.x), max.y);
        }
    }
}
