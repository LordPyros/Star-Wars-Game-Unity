using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendlyCannons : MonoBehaviour {

    public GameObject friendlyLaserPrefab;
    public GameObject laserPos01;
    public GameObject laserPos02;

    public GameObject concussionMissilePrefab;
    public GameObject torpedoPrefab;
    public GameObject torpedoPos01;
    public GameObject torpedoPos02;



    
    public void FireStraightForward()
    {
        // instantiate enemy lasers
        GameObject laser01 = Instantiate(friendlyLaserPrefab);
        GameObject laser02 = Instantiate(friendlyLaserPrefab);
        // set the laser's initial position
        laser01.transform.position = laserPos01.transform.position;
        laser02.transform.position = laserPos02.transform.position;
        // set the rotation
        laser01.transform.rotation = transform.parent.rotation;
        laser02.transform.rotation = transform.parent.rotation;
        // set the laser's direction
        laser01.GetComponent<FriendlyLaser>().SetForwardDirection(laser01.transform.right);
        laser02.GetComponent<FriendlyLaser>().SetForwardDirection(laser02.transform.right);

    }

    public void FireTorepdo()
    {
        GameObject torpedo = Instantiate(torpedoPrefab);
        torpedo.transform.position = torpedoPos01.transform.position;
        torpedo.transform.rotation = transform.parent.rotation;
    }

    public void FireConcussionMissile()
    {
        GameObject missile = Instantiate(concussionMissilePrefab);
        missile.transform.position = torpedoPos01.transform.position;
        missile.transform.rotation = transform.parent.rotation;
    }
}
