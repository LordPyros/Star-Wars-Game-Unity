using UnityEngine;

public class FriendlyFalconControl : MonoBehaviour {

    private int health;
    private int shields;
    public GameObject ExplosionAnim; // explosion prefab
    public GameObject ShieldPrefab;
    void Start()
    {
        health = Rules.GetFalconStartingHealth();
        shields = Rules.GetFalconStartingShields();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // detect collision between enemy lasers and ships
        switch (col.tag)
        {
            case "EnemyShipTag":
            case "EnemyCapitalShipTag":
                TakeDamage(Rules.GetShipCollisionDamage());
                break;
        }
    }
    void PlayExplosion()
    {
        GameObject explosion = Instantiate(ExplosionAnim);

        explosion.transform.position = transform.position;
    }

    private void UpdateShield()
    {
        ShieldPrefab.gameObject.SetActive(false);
        ShieldPrefab.gameObject.SetActive(true);
        if (shields <= 0)
        {
            Invoke("TurnOffShield", .4f);
        }

    }
    void TurnOffShield()
    {
        ShieldPrefab.gameObject.SetActive(false);
    }


    public void TakeDamage(int damage)
    {
        // if no shields, take health damage
        if (shields <= 0)
        {
            health -= damage;
        }
        // if shields, take shield damage, if there is excess damage, take that as health damage
        else
        {
            shields -= damage;
            UpdateShield();
            if (shields < 0)
            {
                health += shields;
            }
        }
        // check if ship is dead
        if (health <= 0)
        {
            PlayExplosion();
            Destroy(gameObject);
        }

    }
}
