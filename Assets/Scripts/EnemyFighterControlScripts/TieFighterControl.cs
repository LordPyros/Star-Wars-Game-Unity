using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TieFighterControl : MonoBehaviour {
    
    
    public GameObject ExplosionAnim; // explosion prefab
    public int health { get; set; }
    // Use this for initialization
    void Start () {
        
        health = Rules.GetTieFighterHealth();
	}
	
    private void OnTriggerEnter2D(Collider2D col)
    {
        // detect collision of enemy ship with player ship or players laser/missile
        switch (col.tag)
        {
            case "PlayerShipTag":
            case "FriendlyShipTag":
                ShipDestroyed();
                break;
        }
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            if (health <= 0)
                ShipDestroyed();
        }
        
    }

    void ShipDestroyed()
    {
        PlayExplosion();
        // get current position
        var currentPos = transform.position;
        
        // add 100 points to score
        LevelManager.Instance.Score += 100;
        // destroy enemy ship
        Destroy(gameObject);
        // Check if any enemies are close enough to receive explosion damage
        GameMechanics.NearbyShipExplosionDamage(currentPos, .5f);
    }

    void PlayExplosion()
    {
        GameObject explosion = Instantiate(ExplosionAnim);
        explosion.transform.position = transform.position;
    }
    
}
