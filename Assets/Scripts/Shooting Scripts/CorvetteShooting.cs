using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorvetteShooting : MonoBehaviour {

    public GameObject HeavyLaserPos1;
    public GameObject HeavyLaserPos2;
    public GameObject LaserPos1;
    public GameObject LaserPos2;
    public GameObject LaserPos3;
    public GameObject LaserPos4;
    public GameObject LaserPrefab;
    public GameObject HeavyLaserPrefab;
    private GameObject playerShip;
    // Use this for initialization
    void Start () {
        Invoke("FireEnemyHeavyLaser", 2f);
        Invoke("FireLeftSideLasers", 2.5f);
        Invoke("FireRightSideLasers", 2.5f);
        // Get a reference to the player's ship
        ChooseATarget();
    }
	
	// Update is called once per frame
	void Update () {
        // rotate lasers to direction of target
        Vector3 vectorToTarget = playerShip.transform.position - transform.position;
        float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
        Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = q;
    }
    // front cannons fire at player 
    
    private void FireEnemyHeavyLaser()
    {
        if (playerShip == null || !playerShip.activeInHierarchy)
        {
            playerShip = GameMechanics.SelectPlayerAsTarget();
        }
            // instantiate enemy lasers
            GameObject laser01 = Instantiate(HeavyLaserPrefab);
            GameObject laser02 = Instantiate(HeavyLaserPrefab);
            // set the laser's initial position
            laser01.transform.position = HeavyLaserPos1.transform.position;
            laser02.transform.position = HeavyLaserPos2.transform.position;
            // compute the laser's direction towards the player's ship
            Vector2 direction01 = playerShip.transform.position - laser01.transform.position;
            Vector2 direction02 = playerShip.transform.position - laser02.transform.position;
            // set the laser's direction
            laser01.GetComponent<EnemyHeavyLaser>().SetDirection(direction01);
            laser02.GetComponent<EnemyHeavyLaser>().SetDirection(direction02);
            // rotate lasers to direction of target
            Vector3 vectorToTarget = playerShip.transform.position - transform.position;
            float angle = Mathf.Atan2(vectorToTarget.y, vectorToTarget.x) * Mathf.Rad2Deg;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);
            laser01.transform.rotation = q;
            laser02.transform.rotation = q;

            float rand = Random.Range(1f, 2f);
            Invoke("FireEnemyHeavyLaser", rand);
        
    }
    // side lasers fire in a random but still some what sideways direction
    public void FireLeftSideLasers()
    {
        // instantiate enemy lasers
        GameObject laser01 = Instantiate(LaserPrefab);
        GameObject laser02 = Instantiate(LaserPrefab);
        
        // set the laser's initial position
        laser01.transform.position = LaserPos1.transform.position;
        laser02.transform.position = LaserPos2.transform.position;
        
        // get the direction
        float ran = Random.Range(50f, 100f);
        laser01.transform.rotation = transform.parent.rotation;
        laser02.transform.rotation = transform.parent.rotation;
        // set the rotation
        laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
        laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
        
        // set the laser's direction
        laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
        laser02.GetComponent<EnemyLaser>().SetForwardDirection(laser02.transform.right);

        float rand = Random.Range(1f, 2f);
        Invoke("FireLeftSideLasers", rand);
    }
    // side lasers fire in a random but sideways direction
    public void FireRightSideLasers()
    {
        // instantiate enemy lasers
        GameObject laser03 = Instantiate(LaserPrefab);
        GameObject laser04 = Instantiate(LaserPrefab);
        // set the laser's initial position
        laser03.transform.position = LaserPos3.transform.position;
        laser04.transform.position = LaserPos4.transform.position;
        // get the direction
        float ran = Random.Range(260f, 310f);
        laser03.transform.rotation = transform.parent.rotation;
        laser04.transform.rotation = transform.parent.rotation;
        laser03.transform.rotation *= Quaternion.Euler(0, 0, ran);
        laser04.transform.rotation *= Quaternion.Euler(0, 0, ran);
        // set the laser's direction
        laser03.GetComponent<EnemyLaser>().SetForwardDirection(laser03.transform.right);
        laser04.GetComponent<EnemyLaser>().SetForwardDirection(laser04.transform.right);

        float rand = Random.Range(1f, 2f);
        Invoke("FireRightSideLasers", rand);
    }

    private void ChooseATarget()
    {
        playerShip = GameMechanics.SelectPlayerAsTarget();
    }
}


