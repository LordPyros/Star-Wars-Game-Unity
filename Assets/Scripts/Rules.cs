using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rules : MonoBehaviour {
    
    // Tween speeds
    // tie fighter = 1.7
    // tie interceptor = 2.0
    // tie bombed = 1.5
    // tie advance = 1.9

    // Starting lives ( 0 counts as a life)
    public static int GetPlayerStartingLives()
    {
        return 3;
    }
    // Starting CounterMeasures
    public static int GetPlayerStartingCounterMeasures()
    {
        return 20;
    }

    // ship collision damage
    public static int GetShipCollisionDamage()
    {
        return 2;
    }
    public static int GetNearbyShipExplosionDamage()
    {
        return 2;
    }

    // weapons damage
    public static int GetTorpedoDamage()
    {
        return 8;
    }
    public static int GetUpgradedTorpedoDamage()
    {
        return 12;
    }
    public static int GetConcussionMissileDamage()
    {
        return 6;
    }
    public static int GetUpgradedConcussionMissileDamage()
    {
        return 9;
    }
    public static int GetLaserDamage()
    {
        return 1;
    }
    public static int GetUpgradedLaserDamage()
    {
        return 2;
    }
    public static int GetHeavyLaserDamage()
    {
        return 3;
    }

    // Friendly Fighter stats
    //Xwing
    public static int GetXwingStartingHealth()
    {
        return 4;
    }
    public static int GetXwingStartingShields()
    {
        return 4;
    }
    public static int GetXwingMaxShields()
    {
        return 8;
    }
    public static int GetXwingStartingTorpedos()
    {
        return 6;
    }
    public static float GetXwingSpeed()
    {
        return 5f;
    }
    //Awing
    public static int GetAwingStartingHealth()
    {
        return 3;
    }
    public static int GetAwingStartingShields()
    {
        return 3;
    }
    public static int GetAwingMaxShields()
    {
        return 6;
    }
    public static int GetAwingStartingMissiles()
    {
        return 8;
    }
    // Millenium Falcon
    public static int GetFalconStartingHealth()
    {
        return 12;
    }
    public static int GetFalconStartingShields()
    {
        return 12;
    }


    // Mine health
    public static int GetMineHealth()
    {
        return 1;
    }

    //  Enemy Fighter health
    public static int GetTieFighterHealth()
    {
        return 3;
    }
    public static int GetTieInterceptorHealth()
    {
        return 5;
    }
    public static int GetTieBomberHealth()
    {
        return 7;
    }

    // Tie advanced
    public static int GetTieAdvancedHealth()
    {
        return 6;
    }
    public static int GetTieAdvancedShields()
    {
        return 6;
    }

    // Enemy Transports
    public static int GetATR6Health()
    {
        return 12;
    }
    public static int GetATR6Shields()
    {
        return 15;
    }


    // Capital Ship health
    public static int GetCorvetteHealth()
    {
        return 20;
    }
    public static int GetCorvetteShield()
    {
        return 22;
    }
    public static int GetStarDestroyerHealth()
    {
        return 400;
    }
    public static int GetShieldGeneratorHealth()
    {
        return 60;
    }
}
