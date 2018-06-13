using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMechanics : MonoBehaviour {

	
    // when a ship is destroyed, check for nearby enemy ships and deal them explosion damage
    public static void NearbyShipExplosionDamage(Vector3 center, float radius)
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(center, radius);
        int i = 0;
        while (i < hitColliders.Length)
        {
            if (CheckColliderAgainstAllEnemyTags(hitColliders[i]))
            {
                hitColliders[i].SendMessage("TakeDamage", Rules.GetNearbyShipExplosionDamage());
            }
            i++;
        }
    }

    public static bool CheckColliderAgainstAllEnemyTags(Collider2D col)
    {
        if (col.tag == "EnemyShipTag" || col.tag == "EnemyCapitalShipTag" || col.tag == "LeftShieldGenTag" || col.tag == "RightShieldGenTag")
            return true;
        else
            return false;
    }

    public static bool CheckColliderAgainstAllFriendlyTags(Collider2D col)
    {
        if (col.tag == "PlayerShipTag" || col.tag == "FriendlyShipTag" || col.tag == "PlayerLaserTag" || col.tag == "PlayerProtonTorpedoTag" || col.tag == "PlayerConcussionMissileTag")
            return true;
        else return false;
    }

    // Used by enemies to select a player ship to target
    public static GameObject SelectPlayerAsTarget()
    {
        // Select a target
        if (LevelManager.is2PlayerGame)
        {
            if (LevelManager.playersRemaining == 2)
            {
                // 2Player, both players alive
                // randomly select 1 and set it as target
                bool choose = (Random.Range(0, 2) == 0);
                if (choose)
                    return LevelManager.Instance.playerShip1;
                else
                    return LevelManager.Instance.playerShip2;
            }
            else
            {
                // 2Player, 1 player is dead
                // check which ship is alive and set target to the one thats alive
                if (LevelManager.Instance.playerShip1 != null && LevelManager.Instance.playerShip1.activeInHierarchy)
                {
                    return LevelManager.Instance.playerShip1;
                }
                else
                {
                    return LevelManager.Instance.playerShip2;
                }
            }
        }
        else
        {
            // single player, find the player ship and set it as target
            return LevelManager.Instance.playerShip1;
        }

    }
}
