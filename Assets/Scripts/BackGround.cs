using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour {
    private Rigidbody2D rb2d;
    private SpriteRenderer sr;
    private float spriteHeight;
    private Vector2 min;
    private Vector2 max;
    // Use this for initialization
    void Start () {
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(0, -0.5f);

        // can stop with  - rb2d.velocity = Vector2.zero;

        spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;

        // bottom left of screen
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        // top right of screen
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
    }
	
	void Update ()
    {
        // check if sprite is out of screen
        if (transform.position.y < min.y - spriteHeight / 2)
        {
            RepositionBackground();
        }
	}

    private void RepositionBackground()
    {
        // move the background image above the other
        transform.position = new Vector2(0, max.y + spriteHeight / 2);
    }
}
