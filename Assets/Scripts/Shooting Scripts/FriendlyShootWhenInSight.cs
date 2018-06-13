using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyShootWhenInSight : MonoBehaviour {

    public float laserShootingSpeed;
    public float torpedoShootingSpeed;
    public float missileShootingSpeed;
    public bool isQuadFireShip;
    public bool canFireTorpedos;
    public bool canFireMissiles;
    bool canShootLaser;
    bool canShootTorpedo;
    bool canShootMissile;
    int torpedosRemaining;
    int missilesRemaining;
    // Use this for initialization
    void Start()
    {
        // allow lasers to begin after period of time
        Invoke("CanShoot", 1.5f);
        // torpedos may fire immediately
        if (canFireTorpedos)
        {
            canShootTorpedo = true;
            torpedosRemaining = 8;
        }
        if (canFireMissiles)
        {
            canShootMissile = true;
            missilesRemaining = 12;
        }
           
    }

    // Update is called once per frame
    void Update()
    {
        // check if can shoot
        if (canShootLaser || canShootTorpedo || canShootMissile)
        {
            Vector2 pos = new Vector2(transform.position.x + 0.5f, transform.position.y);
            // raycast to player ship
            RaycastHit2D hit = Physics2D.Raycast(pos, transform.right);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "EnemyShipTag" || hit.transform.tag == "EnemyCapitalShip")
                {
                    // if successful, shoot
                    FriendlyCannons cannons = GetComponentInChildren<FriendlyCannons>();
                    if (canShootTorpedo)
                    {
                        if (torpedosRemaining > 0)
                        {
                            cannons.FireTorepdo();
                            torpedosRemaining--;
                            canShootTorpedo = false;
                            Invoke("CanShootTorpedo", torpedoShootingSpeed);
                        }
                    }
                    else if (canShootMissile)
                    {
                        if (missilesRemaining > 0)
                        {
                            cannons.FireConcussionMissile();
                            missilesRemaining--;
                            canShootMissile = false;
                            Invoke("CanShootMissile", missileShootingSpeed);
                        }
                    }
                    else
                    {
                        ShootLaser(cannons);
                        canShootLaser = false;
                        Invoke("CanShoot", laserShootingSpeed);

                    }
                }
            }
        }
    }

    void CanShootMissile()
    {
        canShootMissile = true;
    }

    void CanShootTorpedo()
    {
        canShootTorpedo = true;
    }
    void CanShoot()
    {
        canShootLaser = true;
    }

    void ShootLaser(FriendlyCannons cannons)
    {
        cannons.FireStraightForward();
        if (isQuadFireShip)
        {
            cannons.FireStraightForward();
        }
    }
}
