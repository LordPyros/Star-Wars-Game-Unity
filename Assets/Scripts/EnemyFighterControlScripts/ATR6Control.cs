using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATR6Control : MonoBehaviour {

    public GameObject ExplosionAnim; // explosion prefab
    private int health;
    private int shields;
    public GameObject ShieldPrefab;
    // Use this for initialization
    void Start()
    {
        health = Rules.GetATR6Health();
        shields = Rules.GetATR6Shields();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // detect collision of enemy ship with player ship or players laser/missile
        switch (col.tag)
        {
            case "PlayerShipTag":
                ShipDestroyed();
                break;
        }
    }

    void ShipDestroyed()
    {
        PlayExplosion();
        
        // get current position
        var currentPos = transform.position;
        
        // add 1000 points to score
        LevelManager.Instance.Score += 1000;
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
    
    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            // if no shields, take health damage
            if (shields <= 0)
            {
                health -= damage;
            }
            // if shields, take shield damage, if there is excess damage, take that as health damage
            else
            {
                shields -= damage;

                UpdateShield();
                if (shields < 0)
                {
                    health += shields;
                }
            }
            // check if ship is dead
            if (health <= 0)
                ShipDestroyed();
        }
    }

    private void UpdateShield()
    {
        ShieldPrefab.gameObject.SetActive(false);
        ShieldPrefab.gameObject.SetActive(true);
        if (shields <= 0)
        {
            Invoke("TurnOffShield", .4f);
        }

    }
    void TurnOffShield()
    {
        ShieldPrefab.gameObject.SetActive(false);
    }
}
