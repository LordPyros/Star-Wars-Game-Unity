using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootConstantly : MonoBehaviour {

    public bool isQuadFireShip;
    public float shootingSpeed;
    public float initialShootingDelay;
	
    // Use this for initialization
	void Start () {
        InvokeRepeating("ShootNonStop", initialShootingDelay, shootingSpeed);
	}
	
    void ShootNonStop()
    {
        EnemyCannons cannons = GetComponentInChildren<EnemyCannons>();
        cannons.FireStraightForward();
        if (isQuadFireShip)
        {
            cannons.FireStraightForward();
        }
    }
}
