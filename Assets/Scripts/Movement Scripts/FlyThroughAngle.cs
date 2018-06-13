using UnityEngine;

public class FlyThroughAngle : MonoBehaviour {

    public float movementSpeed;
    private Vector2 min;
    private Vector2 max;
    private bool hasEnteredScreen;
    private SpriteRenderer shipSprite;
    
    void Start()
    {
        // bottom left of screen
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        // top right of screen
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        //
        shipSprite = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        // move the ship
        transform.position += transform.right * Time.deltaTime * movementSpeed;

        if (hasEnteredScreen)
        {
            // check if the ship has left the botttom, left or right sides of the screen and destroy it
            if (min.y - shipSprite.bounds.size.y / 2 > transform.position.y 
                || min.x - shipSprite.bounds.size.x / 2 > transform.position.x 
                || max.x + shipSprite.bounds.size.x / 2 < transform.position.x)
                Destroy(gameObject);
        }
        else
        {
            // check if the ship has entered the screen from the top, left or right sides of screen
            if (max.y + shipSprite.bounds.size.y / 2 > transform.position.y 
                && max.x + shipSprite.bounds.size.x / 2 > transform.position.x
                && min.x - shipSprite.bounds.size.x / 2 < transform.position.x)
                hasEnteredScreen = true;
        }
    }
}
