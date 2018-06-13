using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDestoyerImp1Control : MonoBehaviour {
    public Collider2D mainCollider;
    public GameObject ExplosionAnim;
    private int health;

    
    // Use this for initialization
    void Start () {        
        health = Rules.GetStarDestroyerHealth();
        // get the main collider and disable it (becomes enabled when shield generators are destroyed)
        mainCollider = GetComponent<Collider2D>();
        mainCollider.enabled = false;
	}
	
    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
                // player ship collision
            case "PlayerShipTag":
                health -= Rules.GetShipCollisionDamage();
                if (health <= 0)
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

    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExplosionAnim);

        explosion.transform.position = transform.position;
    }

    void ShipDestroyed()
    {
        PlayExplosion();
        
        // add 10,000 points to score
        LevelManager.Instance.Score += 10000;

        LevelManager.Instance.EndOfLevel();

        // destroy enemy ship
        Destroy(gameObject);
    }

    public void TurnOnMissileTarget()
    {
        // find the star destroyers main missile target GO and enable it
        foreach (Transform t in transform)
        {
            if (t.name == "MissileTargetGO")
            {
                t.gameObject.SetActive(true);
            }
        }
    }

    
}
