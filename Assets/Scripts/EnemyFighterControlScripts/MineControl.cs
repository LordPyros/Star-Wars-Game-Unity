using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineControl : MonoBehaviour {
    public GameObject explosionPrefab;
    public bool isProtonTorpedoMine;
    public bool isConcussionMissileMine;
    private int health;
    //private GameObject target;
    private float rotatingSpeed = 15f;
    public float firingRate = 3f;
    private bool canFireNow = false;
    public GameObject laserPrefab;
    public GameObject torpedoPrefab;
    public GameObject concussionPrefab;
    public GameObject weaponPos1GO;
    public GameObject weaponPos2GO;
    private Rigidbody2D rb;
    private Vector2 min;

    public AudioClip missileBeep;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
        health = Rules.GetMineHealth();
        Invoke("CanFireNow", 0.5f);
        // bottom left of screen
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
    }
	
	void Update () {
        // need to rotate slowly
        rb.angularVelocity = rotatingSpeed;

        // need to move slowly down the screen
        transform.position += new Vector3(0f, -0.4f, 0.0f) * Time.deltaTime;

        // die when out of screen
        if (transform.position.y < min.y - GetComponent<SpriteRenderer>().bounds.size.y / 2)
            Destroy(gameObject);

        // check if can shoot
        if (canFireNow)
        {
            // set the rotation of the raycast so it casts on the same line the turret is pointing
            Quaternion spreadAngle = Quaternion.AngleAxis(230, new Vector3(0, 0, 1));
            Vector3 newVector = spreadAngle * transform.right;
            
            // raycast weapon position1 to player ship
            RaycastHit2D hit = Physics2D.Raycast(weaponPos1GO.transform.position, newVector);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "PlayerShipTag" || hit.transform.tag == "FriendlyShipTag")
                {
                    // if successful, shoot
                    if (isProtonTorpedoMine)
                    {
                        // don't shoot if too close
                        if (2.5f < Vector2.Distance(transform.position, hit.transform.position))
                        {
                            FireTorpedo(hit.transform.gameObject, weaponPos1GO.transform.position, newVector);
                        }
                    }
                    else if (isConcussionMissileMine)
                    {
                        if (2.5f < Vector2.Distance(transform.position, hit.transform.position))
                        {
                            FireConcussionMissile(hit.transform.gameObject, weaponPos1GO.transform.position, newVector);
                        }
                    }
                    else 
                    {
                        FireLaser(weaponPos1GO.transform.position, newVector);
                    }
                    canFireNow = false;
                    Invoke("CanFireNow", firingRate);
                    return;
                }
            }
            else
            {
                spreadAngle = Quaternion.AngleAxis(330, new Vector3(0, 0, 1));
                newVector = spreadAngle * transform.right;
                hit = Physics2D.Raycast(weaponPos2GO.transform.position, newVector);
                if (hit.collider != null)
                {
                    if (hit.transform.tag == "PlayerShipTag" || hit.transform.tag == "FriendlyShipTag")
                    {
                        // if successful, shoot
                        if (isProtonTorpedoMine)
                        {
                            if (2.5f < Vector2.Distance(transform.position, hit.transform.position))
                            {
                                FireTorpedo(hit.transform.gameObject, weaponPos2GO.transform.position, newVector);
                            }
                        }
                        else if (isConcussionMissileMine)
                        {
                            if (2.5f < Vector2.Distance(transform.position, hit.transform.position))
                            {
                                FireConcussionMissile(hit.transform.gameObject, weaponPos2GO.transform.position, newVector);
                            }
                        }
                        else
                        {
                            FireLaser(weaponPos2GO.transform.position, newVector);
                        }
                        canFireNow = false;
                        Invoke("CanFireNow", firingRate);
                        return;
                    }
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "PlayerShipTag" || col.tag == "FriendlyShipTag")
        {
            ShipDestroyed();
        }
    }

    public void ShipDestroyed()
    {
        // play explosion
        GameObject go = Instantiate(explosionPrefab);
        go.transform.position = transform.position;

        // destroy mine
        Destroy(gameObject);
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

    private void FireLaser(Vector3 weaponPos, Vector3 direction)
    {
        // instantiate enemy lasers
        GameObject laser01 = Instantiate(laserPrefab);
        // set the laser's initial position
        laser01.transform.position = weaponPos;
        
        // rotate lasers to direction of target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        laser01.transform.rotation = q;
        // set the laser's direction
        laser01.GetComponent<EnemyLaser>().SetForwardDirection(direction);
    }
    private void FireTorpedo(GameObject target, Vector3 weaponPos, Vector3 direction)
    {
        StartCoroutine(PlayMissileLockBeep());

        GameObject torpedo = Instantiate(torpedoPrefab);
        torpedo.transform.position = weaponPos;
        // rotate lasers to direction of target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        torpedo.transform.rotation = q;
        torpedo.GetComponent<EnemyProtonTorpedo>().GetTarget(target);
    }
    private void FireConcussionMissile(GameObject target, Vector3 weaponPos, Vector3 direction)
    {
        StartCoroutine(PlayMissileLockBeep());

        GameObject missile = Instantiate(concussionPrefab);
        missile.transform.position = weaponPos;
        // rotate lasers to direction of target
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        missile.transform.rotation = q;
        missile.GetComponent<EnemyProtonTorpedo>().GetTarget(target);
    }

    private void CanFireNow()
    {
        canFireNow = true;
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
