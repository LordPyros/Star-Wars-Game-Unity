using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUpSpawner : MonoBehaviour {
    public GameObject[] PowerUpPrefabs;
    public static float spawnRate {get; set;}
    public static PowerUpSpawner Instance { get; set; }    
    // Use this for initialization
    void Start () {
        Instance = this;
        spawnRate = 15f;
    }
	
    public void SpawnPowerUp()
    {
        Invoke("SpawnPowerUpNow", spawnRate);
    }

    private void SpawnPowerUpNow()
    {
        //bottm left screen corner
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));

        // top right screen corner
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));

        // choose a power up type at random
        int i = UnityEngine.Random.Range(0, 4);
        //int i = 1; // for testing

        // instantiate power up
        GameObject powerUp = (GameObject)Instantiate(PowerUpPrefabs[i]);
        powerUp.transform.position = new Vector2(UnityEngine.Random.Range(min.x, max.x), max.y);

        // schedule when to spawn next power up
        Invoke("SpawnPowerUpNow", spawnRate);
    }
    public void StopSpawnPowerUp()
    {
        CancelInvoke("SpawnPowerUpNow");
    }
    
}
