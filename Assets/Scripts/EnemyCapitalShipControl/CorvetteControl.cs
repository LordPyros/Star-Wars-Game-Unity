using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorvetteControl : MonoBehaviour {
    public GameObject ExplosionAnim;
    public GameObject ShieldAnimPrefab;
    private int health;
    private int shields;
    
    // Use this for initialization
	void Start () {
        health = Rules.GetCorvetteHealth();
        shields = Rules.GetCorvetteShield();
	}
	

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "PlayerShipTag":
                TakeDamage(Rules.GetShipCollisionDamage());
                break;
        }
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

    void ShipDestroyed()
    {
        PlayExplosion();

        // add 1400 points to score
        LevelManager.Instance.Score += 1400;

        // destroy enemy ship
        Destroy(gameObject);
    }
    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExplosionAnim);

        explosion.transform.position = transform.position;
    }

    private void UpdateShield()
    {
        ShieldAnimPrefab.gameObject.SetActive(false);
        ShieldAnimPrefab.gameObject.SetActive(true);
        if (shields <= 0)
        {
            Invoke("TurnOffShield", .4f);
        }

    }
    void TurnOffShield()
    {
        ShieldAnimPrefab.gameObject.SetActive(false);
    }
}
