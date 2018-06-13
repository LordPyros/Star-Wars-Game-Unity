using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBursts : MonoBehaviour {
    public bool isQuadFireShip;
    public float shootingSpeed;
    public float initialShootingDelay;
    // Use this for initialization
    void Start () {
        InvokeRepeating("ShootInBursts", initialShootingDelay, shootingSpeed);
    }

    void ShootInBursts()
    {
        Invoke("ShootLasers", 0f);
        Invoke("ShootLasers", .4f);
        Invoke("ShootLasers", .8f);
        if (isQuadFireShip)
        {
            Invoke("ShootLasers", 0f);
            Invoke("ShootLasers", .4f);
            Invoke("ShootLasers", .8f);
        }
    }

    void ShootLasers()
    {
        EnemyCannons cannons = GetComponentInChildren<EnemyCannons>();
        cannons.FireStraightForward();
    }
}
