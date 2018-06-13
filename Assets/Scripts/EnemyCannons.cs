using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCannons : MonoBehaviour {

    public GameObject enemyLaser; // enemy laser cannon position prefab
    public GameObject laserPosition01;
    public GameObject laserPosition02;
    public GameObject torpedoPosition01;
    public GameObject torpedoPosition02; 
    public GameObject torpedoPrefab;
    public GameObject missilePosition01;
    public GameObject missilePosition02;
    public GameObject missilePrefab;

    // Use this for initialization
    void Start () {
        // fire a laser after 1 second
        //InvokeRepeating("FireEnemyLaser", 1f, 1f);
	}
    
    public void FireStraightForward()
    {

        // instantiate enemy lasers
        GameObject laser01 = Instantiate(enemyLaser);
        GameObject laser02 = Instantiate(enemyLaser);
        // set the laser's initial position
        laser01.transform.position = laserPosition01.transform.position;
        laser02.transform.position = laserPosition02.transform.position;
        // set the rotation
        laser01.transform.rotation = transform.parent.rotation;
        laser02.transform.rotation = transform.parent.rotation;
        // set the laser's direction
        laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
        laser02.GetComponent<EnemyLaser>().SetForwardDirection(laser02.transform.right);
        
    }

    public void FireTorepdo(GameObject target)
    {
        GameObject torpedo = Instantiate(torpedoPrefab);
        torpedo.transform.position = torpedoPosition01.transform.position;
        torpedo.transform.rotation = transform.parent.rotation;
        torpedo.GetComponent<EnemyProtonTorpedo>().GetTarget(target);
    }

    public void FireMissile(GameObject target)
    {
        GameObject missile = Instantiate(missilePrefab);
        missile.transform.position = missilePosition01.transform.position;
        missile.transform.rotation = transform.parent.rotation;
        missile.GetComponent<EnemyProtonTorpedo>().GetTarget(target);
    }
}
