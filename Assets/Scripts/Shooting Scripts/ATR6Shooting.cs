using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATR6Shooting : MonoBehaviour {

    public GameObject laserPrefab;
    public GameObject[] laserPositions; // 0 = center, 1 = left, 2 = right
    public GameObject[] gunPositions; // 0 = center, 1 = left, 2 = right
    public GameObject torpedoPrefab;
    public GameObject[] torpedoPositions;
    private GameObject target;
    public bool hasTorpedos;
    private bool canShootTorpedos;
    public float minTorpedoFiringSpeed;
    private void Start()
    {
        InvokeRepeating("SelectTarget", 0f, 8f);
        //InvokeRepeating("FireTopLaserAtPlayer", 1f, 1f);

        FireLeftLaser();
        FireRightLaser();
        FireTopLaser();
        FireTopLaserAtPlayer();
        if (hasTorpedos)
            Invoke("CanShootTorpedos", 0.5f);
    }

    private void FixedUpdate()
    {
        // fire torpedos when in sight
        if (canShootTorpedos)
        {
            // raycast to player ship
            RaycastHit2D hit = Physics2D.Raycast(torpedoPositions[0].transform.position, transform.right);
            if (hit.collider != null)
            {
                if (hit.transform.tag == "PlayerShipTag")
                {
                    if (2.5f < Vector2.Distance(torpedoPositions[0].transform.position, hit.transform.position))
                    {
                        ShootTorpedos(hit.transform.gameObject);
                        canShootTorpedos = false;
                        Invoke("CanShootTorpedos", minTorpedoFiringSpeed);
                    }
                }
            }
        }
    }

    // select a player ship, find out where the player ship is in comparison to the current rotation


    // when its time to fire, it checks if target is in its arc, if so shoot target otherwise do a random shot inside the arc
    // "top" laser always shoots at target, "bottom laser always shoots randomly (in arc?)
    private void FireLeftLaser()
    {
        // check if target is in arc



        // if not in arc
        // instantiate enemy lasers
        GameObject laser01 = Instantiate(laserPrefab);
        // get the direction
        float ran = Random.Range(40f, 180f);
        // rotate turret to the direction the laser is being fired
        gunPositions[1].transform.rotation *= Quaternion.Euler(0, 0, ran);
        // set the laser's initial position
        laser01.transform.position = laserPositions[1].transform.position;
        // set the rotation
        laser01.transform.rotation = transform.rotation;
        laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
        // set the laser's direction
        laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
        // return turrets rotation back to ships rotation
        gunPositions[1].transform.rotation = transform.rotation;

        float rand = Random.Range(1f, 2f);
        Invoke("FireLeftLaser", rand);
    }
    private void FireRightLaser()
    {
        // check if target is in arc



        // if not in arc
        // instantiate enemy lasers
        GameObject laser01 = Instantiate(laserPrefab);
        // get the direction
        float ran = Random.Range(180f, 320f);
        // rotate turret to the direction the laser is being fired
        gunPositions[2].transform.rotation *= Quaternion.Euler(0, 0, ran);
        // set the laser's initial position
        laser01.transform.position = laserPositions[2].transform.position;
        // set the rotation
        laser01.transform.rotation = transform.rotation;
        laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
        // set the laser's direction
        laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
        // return turrets rotation back to ships rotation
        gunPositions[2].transform.rotation = transform.rotation;

        float rand = Random.Range(1f, 2f);
        Invoke("FireRightLaser", rand);
    }
    private void FireTopLaser()
    {
        // check if target is in arc



        // if not in arc
        // instantiate enemy lasers
        GameObject laser01 = Instantiate(laserPrefab);
        // get the direction
        float ran = Random.Range(-40f, 40f);
        // rotate turret to the direction the laser is being fired
        gunPositions[0].transform.rotation *= Quaternion.Euler(0, 0, ran);
        // set the laser's initial position
        laser01.transform.position = laserPositions[0].transform.position;
        // set the rotation
        laser01.transform.rotation = transform.rotation;
        laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
        // set the laser's direction
        laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
        // return turrets rotation back to ships rotation
        gunPositions[0].transform.rotation = transform.rotation;

        float rand = Random.Range(1f, 2f);
        Invoke("FireTopLaser", rand);
    }
    private void FireTopLaserAtPlayer()
    {
        if (target == null || !target.activeInHierarchy)
            target = GameMechanics.SelectPlayerAsTarget();
        // instantiate enemy lasers
        GameObject laser01 = Instantiate(laserPrefab);
        // rotate turret to point at target
        Vector3 vectorToTarget = target.transform.position - gunPositions[0].transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        gunPositions[0].transform.rotation = q;
        // set the laser's initial position
        laser01.transform.position = laserPositions[0].transform.position;
        // set lasers direction
        vectorToTarget = target.transform.position - laser01.transform.position;
        // rotate lasers to direction of target
        angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        q = Quaternion.AngleAxis(angle, Vector3.forward);
        laser01.transform.rotation = q;
        // set the laser's direction
        laser01.GetComponent<EnemyLaser>().SetDirection(vectorToTarget);
        
        // return turrets rotation back to the ships rotation
        gunPositions[0].transform.rotation = transform.rotation; 

        float rand = Random.Range(.8f, 1.2f);
        Invoke("FireTopLaserAtPlayer", rand);
    }

    private void SelectTarget()
    {
        target = GameMechanics.SelectPlayerAsTarget();
    }

    private void CanShootTorpedos()
    {
        canShootTorpedos = true;
    }

    private void ShootTorpedos(GameObject tar)
    {
        GameObject torpedo = Instantiate(torpedoPrefab);
        torpedo.transform.position = torpedoPositions[0].transform.position;
        torpedo.transform.rotation = transform.rotation;
        torpedo.GetComponent<EnemyProtonTorpedo>().GetTarget(tar);
        GameObject torpedo1 = Instantiate(torpedoPrefab);
        torpedo1.transform.position = torpedoPositions[1].transform.position;
        torpedo1.transform.rotation = transform.rotation;
        torpedo1.GetComponent<EnemyProtonTorpedo>().GetTarget(tar);
    }
}
