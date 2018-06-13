using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TieBomberControl : MonoBehaviour
{


    //public GameObject EnemyCannonsPrefab; // enemy laser cannon position prefab
    public GameObject ExplosionAnim; // explosion prefab
    public int health { get; set; }
    // Use this for initialization
    void Start()
    {
        health = Rules.GetTieBomberHealth();
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

    void ShipDestroyed()
    {
        PlayExplosion();
        // get current position
        var currentPos = transform.position;

        // add 180 points to score
       LevelManager.Instance.Score += 180;

        // destroy enemy ship
        Destroy(gameObject);
        // Check if any enemies are close enough to receive explosion damage
        GameMechanics.NearbyShipExplosionDamage(currentPos, .5f);
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
    
}
