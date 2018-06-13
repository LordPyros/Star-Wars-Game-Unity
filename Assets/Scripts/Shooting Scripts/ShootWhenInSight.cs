using System.Collections;
using UnityEngine;

public class ShootWhenInSight : MonoBehaviour {
    public float laserShootingSpeed;
    public float torpedoMissileShootingSpeed;
    public bool isQuadFireShip;
    public bool canFireTorpedos;
    public bool canFireMissiles;
    bool canShootLaser;
    bool canShootTorpedo;
    bool canShootMissile;

    public AudioClip missileBeep;

	// Use this for initialization
	void Start () {
        // allow lasers to begin after period of time
        Invoke("CanShoot", 1.5f);
        // torpedos may fire immediately
        if (canFireTorpedos)
            canShootTorpedo = true;
        if (canFireMissiles)
            canShootMissile = true;
	}
	
	// Update is called once per frame
	void Update () {
        // check if can shoot
        if (canShootLaser || canShootTorpedo || canShootMissile)
        {
            Vector2 pos = new Vector2(transform.position.x + 0.5f, transform.position.y);
            // raycast to player ship
            RaycastHit2D hit = Physics2D.Raycast(pos, transform.right);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "PlayerShipTag")
                {
                    // if successful, shoot
                    EnemyCannons cannons = GetComponentInChildren<EnemyCannons>();
                    if (canShootTorpedo)
                    {
                        if (2.5f < Vector2.Distance(transform.position, hit.transform.position))
                        {
                            StartCoroutine(PlayMissileLockBeep());

                            cannons.FireTorepdo(hit.transform.gameObject);
                            canShootTorpedo = false;
                            Invoke("CanShootTorpedo", torpedoMissileShootingSpeed);
                        }
                    }
                    else if (canShootLaser)
                    {
                        ShootLaser(cannons);
                        canShootLaser = false;
                        Invoke("CanShoot", laserShootingSpeed);
                    }
                    else
                    {
                        if (2.5f < Vector2.Distance(transform.position, hit.transform.position))
                        {
                            StartCoroutine(PlayMissileLockBeep());

                            cannons.FireMissile(hit.transform.gameObject);
                            canShootMissile = false;
                            Invoke("CanShootMissile", torpedoMissileShootingSpeed);
                        }
                    }
                }
            }
        }
    }

    void CanShootTorpedo()
    {
        canShootTorpedo = true;
    }
    void CanShootMissile()
    {
        canShootMissile = true;
    }
    void CanShoot()
    {
        canShootLaser = true;
    }

    void ShootLaser(EnemyCannons cannons)
    {
        cannons.FireStraightForward();
        if (isQuadFireShip)
        {
            cannons.FireStraightForward();
        }
    }

    public IEnumerator PlayMissileLockBeep()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(missileBeep);
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<AudioSource>().PlayOneShot(missileBeep);
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<AudioSource>().PlayOneShot(missileBeep);
    }

}
