using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldGenerator : MonoBehaviour {
    public GameObject ExplosionSmallPrefab;
    int health;
    public Sprite noShieldSprite;
    public GameObject shieldAnimPrefab;


    // Use this for initialization
    void Start () {
        health = Rules.GetShieldGeneratorHealth();
	}

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            // player ship collision
            case "PlayerShipTag":
                health -= Rules.GetShipCollisionDamage();
                break;
        }
        if (health <= 0)
            ShieldDestroyed();
    }

    public void TakeDamage(int damage)
    {
        if (health > 0)
        {
            // play shield hit anim on star destroyer
            PlayShieldAnim();

            // take damage
            health -= damage;
            if (health <= 0)
                ShieldDestroyed();
        }
    }

    private void PlayShieldAnim()
    {
        shieldAnimPrefab.gameObject.SetActive(false);
        shieldAnimPrefab.gameObject.SetActive(true);
    }

    void ShieldDestroyed()
    {
        PlayExplosion();

        // disable this collider 
        var col = GetComponent<Collider2D>();
        col.enabled = false;

        // destroy the missile target child gameobject (so missiles can't target dead generator)
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        

        // ** need to find a destroyed generator pic or anim
        // need to enable pic


        // check if other generator is dead
        // find this objects tag
        string tag = gameObject.tag;
        // find what the other shield generators tag is
        if (tag == "RightShieldGenTag")
        {
            // check if collider is enabled (check if shields are up)
            var leftShield = GameObject.FindGameObjectWithTag("LeftShieldGenTag").GetComponent<Collider2D>();
            if (leftShield.enabled == false)
            {
                //both shield gen are down, enable main collider (star destoyer can take damage now)
                EnableMainCollider();
                Destroy(shieldAnimPrefab);
            }
        }
        else if (tag == "LeftShieldGenTag" )
        {
            // check if collider is enabled (check if shields are up)
            var rightShield = GameObject.FindGameObjectWithTag("RightShieldGenTag").GetComponent<Collider2D>();
            if (rightShield.enabled == false)
            {
                //both shield gen are down, enable main collider (star destoyer can take damage now)
                EnableMainCollider();
                Destroy(shieldAnimPrefab);
            }
        }        
    }
    

    void EnableMainCollider()
    {
        // enable main star destoyer collider and swap its sprite to shieldless sprite
        var c = transform.root.gameObject.GetComponent<Collider2D>();
        c.enabled = true;
        c.SendMessage("TurnOnMissileTarget");
        var s = transform.root.gameObject.GetComponent<SpriteRenderer>();
        s.sprite = noShieldSprite;
    }

    void PlayExplosion()
    {
        GameObject explosion = Instantiate(ExplosionSmallPrefab);

        explosion.transform.position = transform.position;
    }
    
}
