using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarDestoryerImp1 : MonoBehaviour {
    public GameObject LaserPrefab;
    public GameObject HeavyLaserPrefab;
    public GameObject[] LaserPositions;
    public GameObject[] HeavyLaserPositions;
    public GameObject[] HeavyLaserTurrets;
    private float fireLaserRandomMin = 2f;
    private float fireLaserRandomMax = 3f;
    private float fireHeavyLaserRandomMin = 2f;
    private float fireHeavyLaserRandomMax = 3f;

    public GameObject torpedoPrefab;
    private GameObject target1;
    private GameObject target2;

    public GameObject fighterPhasePrefab;
    public GameObject fighterPhase1Prefab;
    public GameObject fighterPhase2Prefab;

    public AudioClip missileBeep;

    // change this to false to turn off random fire mode
    private bool randomLaserMode;
    private bool torpedoMode;
    
    // Use this for initialization
    void Start ()
    {
        InvokeRepeating("StartPhases", 0f, 66f);
        //randomLaserMode = true;
        //FireAllLeftSideLasersAtOnce();
        //FireLeftSideLasersRandom();
        //FireLasersRandom();
        //FireHeavyLasersRandom();
        //FireRightSideLasersRandom();
        //FireTopLasersRandom();
        //FireAllHeavyLasersRandom();
    }
	

    void StartPhases()
    {
        // Random Laser Phase
        Invoke("EnableRandomLaserMode", 2f);
        Invoke("FireLasersRandom", 2.1f);
        Invoke("FireHeavyLasersRandom", 2.1f);
        Invoke("DisableRandomLaserMode", 22f);
        // Torepdo Phase
        Invoke("EnableTorpedoMode", 27.1f);
        Invoke("TorpedoPhase", 27.2f);
        Invoke("FireLasersTorpedoMode", 27.2f);
        Invoke("DisableTorpedoMode", 44f);
        // Fighter Phase
        Invoke("FighterPhase", 48f);
        Invoke("FighterPhase2", 53.5f);
        Invoke("FighterPhase1", 61.5f);
    }
    

    void FireHeavyLasersRandom()
    {
        // fire heavy lasers, semi random direction, semi random fire delay
        StartCoroutine(FireHeavyLaserGroup(0, 70f, 85f, -90, 90, Random.Range(0f, 2f), 0));
        StartCoroutine(FireHeavyLaserGroup(1, 55f, 70f, -90, 90, Random.Range(0f, 2f), 2));
        StartCoroutine(FireHeavyLaserGroup(2, 40f, 55f, -90, 90, Random.Range(0f, 2f), 4));
        StartCoroutine(FireHeavyLaserGroup(3, 25f, 40f, -90, 90, Random.Range(0f, 2f), 6));
        StartCoroutine(FireHeavyLaserGroup(4, -70f, -85f, 90, -90, Random.Range(0f, 2f), 8));
        StartCoroutine(FireHeavyLaserGroup(5, -55f, -70f, 90, -90, Random.Range(0f, 2f), 10));
        StartCoroutine(FireHeavyLaserGroup(6, -40f, -55f, 90, -90, Random.Range(0f, 2f), 12));
        StartCoroutine(FireHeavyLaserGroup(7, -25f, -40f, 90, -90, Random.Range(0f, 2f), 14));
        StartCoroutine(FireHeavyLaserGroup(8, 10f, 25f, 0, 0, Random.Range(0f, 2f), 16));
        StartCoroutine(FireHeavyLaserGroup(9, -10f, -25f, 0, 0, Random.Range(0f, 2f), 18));
        StartCoroutine(FireHeavyLaserGroup(10, 0f, 15f, 0, 0, Random.Range(0f, 2f), 20));
        StartCoroutine(FireHeavyLaserGroup(10, -15f, 0f, 0, 0, Random.Range(0f, 2f), 20));
    }

    IEnumerator FireHeavyLaserGroup(int index, float min, float max, float pos1, float pos2, float fireDelay, int laserPos)
    {
        yield return new WaitForSeconds(fireDelay);
        // instantiate enemy lasers
        GameObject laser01 = Instantiate(HeavyLaserPrefab);
        GameObject laser02 = Instantiate(HeavyLaserPrefab);
        // set rotation of turret
        float ran = Random.Range(min, max);
        HeavyLaserTurrets[index].transform.rotation *= Quaternion.Euler(0, 0, pos1 + ran);
        // set the laser's initial position
        laser01.transform.position = HeavyLaserPositions[laserPos].transform.position;
        laser02.transform.position = HeavyLaserPositions[laserPos + 1].transform.position;
        // reset turret roation
        HeavyLaserTurrets[index].transform.rotation *= Quaternion.Euler(0, 0, pos2 - ran);
        // get the direction
        laser01.transform.rotation = transform.rotation;
        laser02.transform.rotation = transform.rotation;
        // set the rotation
        laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
        laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
        // set the laser's direction
        laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
        laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
        fireDelay = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
        if (randomLaserMode || torpedoMode)
        {
            
            StartCoroutine(FireHeavyLaserGroup(index, min, max, pos1, pos2, fireDelay, laserPos));

        }
    }
    
    void FireLasersRandom()
    {
        // fire left group
        for (int i = 0; i < 10; i++)
        {
            StartCoroutine(FireLaserGroup(i, 50f, 85f, Random.Range(0f, 2f)));
        }
        // fire right group
        for (int i = 10; i < 20; i++)
        {
            StartCoroutine(FireLaserGroup(i, 275f, 310f, Random.Range(0f, 2f)));
        }
        // fire top laser group
        StartCoroutine(FireLaserGroup(20, 0f, 20f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(21, -20f, 0f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(22, 10f, 25f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(23, -25f, -10f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(24, 20f, 35f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(25, -35f, -20f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(26, 15f, 30f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(27, -30f, -10f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(28, 20f, 35f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(29, -35f, -20f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(30, 25f, 40f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(31, -40f, -25f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(32, 30f, 45f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(33, -45f, -30f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(34, 35f, 50f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(35, -50f, -35f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(36, 40f, 55f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(37, -55f, -40f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(38, 45f, 60f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(39, -60f, -45f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(40, 50f, 65f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(41, -65f, -50f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(42, 70f, 85f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(43, -85f, -70f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(44, 75f, 90f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(45, -90f, -75f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(46, 80f, 95f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(47, -95f, -80f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(48, 65f, 80f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(49, -80f, -65f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(50, 60f, 75f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(51, -75f, -60f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(52, 55f, 70f, Random.Range(0f, 2f)));
        StartCoroutine(FireLaserGroup(53, -70f, -55f, Random.Range(0f, 2f)));
    }

    IEnumerator FireLaserGroup(int index, float min, float max, float fireDelay)
    {
        yield return new WaitForSeconds(fireDelay);
        // instantiate enemy lasers
        GameObject laser01 = Instantiate(LaserPrefab);
        // set the laser's initial position
        laser01.transform.position = LaserPositions[index].transform.position;
        // get the direction
        laser01.transform.rotation = transform.rotation;
        // set the rotation
        float ran = Random.Range(min, max);
        laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
        // set the laser's direction
        laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
        fireDelay = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
        if (randomLaserMode || torpedoMode)
        {
            
            StartCoroutine(FireLaserGroup(index, min, max, fireDelay));
        }
    }

    void TorpedoPhase()
    {
        // need to call fireTorpedos once a second for 4 seconds, then pause for 4 seconds
        StartCoroutine(PlayMissileLockBeep(0f));
        Invoke("FireTorpedos", 0f);
        Invoke("FireTorpedos", 1f);
        Invoke("FireTorpedos", 2f);
        Invoke("FireTorpedos", 3f);

        StartCoroutine(PlayMissileLockBeep(7f));
        Invoke("FireTorpedos", 7f);
        Invoke("FireTorpedos", 8f);
        Invoke("FireTorpedos", 9f);
        Invoke("FireTorpedos", 10f);

        StartCoroutine(PlayMissileLockBeep(14f));
        Invoke("FireTorpedos", 14f);
        Invoke("FireTorpedos", 15f);
        Invoke("FireTorpedos", 16f);
        Invoke("FireTorpedos", 17f);

        // fire front lasers continuosly
    }

    void FireTorpedos()
    {
        
        
        
        // Set targets for torpedos
        // 2 player mode, both players are alive
        if (LevelManager.is2PlayerGame && LevelManager.playersRemaining == 2)
        {
            target1 = LevelManager.Instance.playerShip1;
            target2 = LevelManager.Instance.playerShip2;

            // player 1 is closer to the left side of screen, fire right side torpedos at P1
            if (target1.transform.position.x <= target2.transform.position.x)
            {
                StartCoroutine(FireTorpedo(20, target2));
                StartCoroutine(FireTorpedo(21, target1));
                StartCoroutine(FireTorpedo(24, target2));
                StartCoroutine(FireTorpedo(25, target1));
                StartCoroutine(FireTorpedo(36, target2));
                StartCoroutine(FireTorpedo(37, target1));
                StartCoroutine(FireTorpedo(48, target2));
                StartCoroutine(FireTorpedo(49, target1));
            }
            else
            {
                StartCoroutine(FireTorpedo(20, target1));
                StartCoroutine(FireTorpedo(21, target2));
                StartCoroutine(FireTorpedo(24, target1));
                StartCoroutine(FireTorpedo(25, target2));
                StartCoroutine(FireTorpedo(36, target1));
                StartCoroutine(FireTorpedo(37, target2));
                StartCoroutine(FireTorpedo(48, target1));
                StartCoroutine(FireTorpedo(49, target2));
            }
            
        }
        // 1 player mode or 2 player and 1 of the players is dead
        else
        {
            target1 = GameMechanics.SelectPlayerAsTarget();
            StartCoroutine(FireTorpedo(20, target1));
            StartCoroutine(FireTorpedo(21, target1));
            StartCoroutine(FireTorpedo(24, target1));
            StartCoroutine(FireTorpedo(25, target1));
            StartCoroutine(FireTorpedo(36, target1));
            StartCoroutine(FireTorpedo(37, target1));
            StartCoroutine(FireTorpedo(48, target1));
            StartCoroutine(FireTorpedo(49, target1));
        }
        // locations
        // TL1, TL3, TL9, TL15

    }

    void FireLasersTorpedoMode()
    {
        StartCoroutine(FireLaserGroup(7, 10f, 20f, 0)); // L8
        StartCoroutine(FireLaserGroup(8, 10f, 20f, 0)); // L9
        StartCoroutine(FireLaserGroup(9, 10f, 20f, 0)); // L10
        StartCoroutine(FireLaserGroup(17, -20f, -10f, 0)); // R8
        StartCoroutine(FireLaserGroup(18, -20f, -10f, 0)); // R9
        StartCoroutine(FireLaserGroup(19, -20f, -10f, 0)); // R10
        StartCoroutine(FireLaserGroup(20, -10f, 0f, 0)); // TL1
        StartCoroutine(FireLaserGroup(22, -10f, 0f, 0)); // TL2
        StartCoroutine(FireLaserGroup(24, -10f, 0f, 0)); // TL3
        StartCoroutine(FireLaserGroup(21, 0f, 10f, 0)); // TR1
        StartCoroutine(FireLaserGroup(23, 0f, 10f, 0)); // TR2
        StartCoroutine(FireLaserGroup(25, 0f, 10f, 0)); // TR3
        StartCoroutine(FireHeavyLaserGroup(10, -10f, 0f, 0, 0, 0, 20));
        StartCoroutine(FireHeavyLaserGroup(10, 0f, 10f, 0, 0, 0, 20));
        StartCoroutine(FireHeavyLaserGroup(9, -15f, -5f, 0, 0, 0, 18));
        StartCoroutine(FireHeavyLaserGroup(8, 5f, 15f, 0, 0, 0, 16));
    }

    IEnumerator FireTorpedo(int index, GameObject target)
    {
        // Fire torpedo from selected position at player ship
        GameObject torpedo = Instantiate(torpedoPrefab);
        torpedo.transform.position = LaserPositions[index].transform.position;
        torpedo.transform.rotation = transform.rotation;
        torpedo.GetComponent<EnemyProtonTorpedo>().GetTarget(target);
        // pointless return to satisfy the compiler
        if (randomLaserMode)
        {
            yield return new WaitForEndOfFrame();
        }
        
    }

    void FighterPhase()
    {
        Instantiate(fighterPhasePrefab);
    }
    void FighterPhase1()
    {
        Instantiate(fighterPhase1Prefab);
    }
    void FighterPhase2()
    {
        Instantiate(fighterPhase2Prefab);
    }

    void DisableRandomLaserMode()
    {
        randomLaserMode = false;
    }
    void EnableRandomLaserMode()
    {
        randomLaserMode = true;
    }
    void DisableTorpedoMode()
    {
        torpedoMode = false;
    }
    void EnableTorpedoMode()
    {
        torpedoMode = true;
    }

    public IEnumerator PlayMissileLockBeep(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        gameObject.GetComponent<AudioSource>().PlayOneShot(missileBeep);
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<AudioSource>().PlayOneShot(missileBeep);
        yield return new WaitForSeconds(0.3f);
        gameObject.GetComponent<AudioSource>().PlayOneShot(missileBeep);
    }


    //void FireLaserL1()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[0].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserL1", rand);
    //}
    //void FireLaserL2()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[1].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserL2", rand);
    //}
    //void FireLaserL3()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[2].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserL3", rand);
    //}
    //void FireLaserL4()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[3].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserL4", rand);
    //}
    //void FireLaserL5()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[4].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserL5", rand);
    //}
    //void FireLaserL6()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[5].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserL6", rand);
    //}
    //void FireLaserL7()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[6].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserL7", rand);
    //}
    //void FireLaserL8()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[7].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserL8", rand);
    //}
    //void FireLaserL9()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[8].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserL9", rand);
    //}
    //void FireLaserL10()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[9].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserL10", rand);
    //}
    //void FireLaserR1()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[10].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(275f, 305f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserR1", rand);
    //}
    //void FireLaserR2()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[11].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(275f, 305f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserR2", rand);
    //}
    //void FireLaserR3()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[12].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(275f, 305f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserR3", rand);
    //}
    //void FireLaserR4()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[13].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(275f, 305f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserR4", rand);
    //}
    //void FireLaserR5()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[14].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(275f, 305f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserR5", rand);
    //}
    //void FireLaserR6()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[15].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(275f, 305f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserR6", rand);
    //}
    //void FireLaserR7()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[16].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(275f, 305f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserR7", rand);
    //}
    //void FireLaserR8()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[17].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(275f, 305f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserR8", rand);
    //}
    //void FireLaserR9()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[18].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(275f, 305f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserR9", rand);
    //}
    //void FireLaserR10()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[19].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(275f, 305f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserR10", rand);
    //}
    //void FireLaserTL1()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[20].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(0f, 20f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL1", rand);
    //}
    //void FireLaserTR1()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[21].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-20f, 0f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR1", rand);
    //}
    //void FireLaserTL2()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[22].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(10f, 25f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL2", rand);
    //}
    //void FireLaserTR2()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[23].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-25f, -10f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR2", rand);
    //}
    //void FireLaserTL3()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[24].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(20f, 35f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL3", rand);
    //}
    //void FireLaserTR3()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[25].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-35f, -20f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR3", rand);
    //}
    //void FireLaserTL4()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[26].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(15f, 30f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL4", rand);
    //}
    //void FireLaserTR4()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[27].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-30f, -15f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR4", rand);
    //}
    //void FireLaserTL5()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[28].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(20f, 35f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL5", rand);
    //}
    //void FireLaserTR5()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[29].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-35f, -20f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR5", rand);
    //}
    //void FireLaserTL6()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[30].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(25f, 40f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL6", rand);
    //}
    //void FireLaserTR6()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[31].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-40f, -25f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR6", rand);
    //}
    //void FireLaserTL7()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[32].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(30f, 45f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL7", rand);
    //}
    //void FireLaserTR7()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[33].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-45f, -30f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR7", rand);
    //}
    //void FireLaserTL8()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[34].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(35f, 50f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL8", rand);
    //}
    //void FireLaserTR8()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[35].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-50f, -35f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR8", rand);
    //}
    //void FireLaserTL9()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[36].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(40f, 55f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL9", rand);
    //}
    //void FireLaserTR9()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[37].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-55f, -40f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR9", rand);
    //}
    //void FireLaserTL10()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[38].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(45f, 60f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL10", rand);
    //}
    //void FireLaserTR10()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[39].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-60f, -45f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR10", rand);
    //}
    //void FireLaserTL11()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[40].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 65f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL11", rand);
    //}
    //void FireLaserTR11()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[41].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-65f, -50f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR11", rand);
    //}
    //void FireLaserTL12()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[42].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(70f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL12", rand);
    //}
    //void FireLaserTR12()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[43].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-85f, -70f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR12", rand);
    //}
    //void FireLaserTL13()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[44].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(75f, 90f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL13", rand);
    //}
    //void FireLaserTR13()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[45].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-90f, -75f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR13", rand);
    //}
    //void FireLaserTL14()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[46].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(80f, 95f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL14", rand);
    //}
    //void FireLaserTR14()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[47].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-95f, -80f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR14", rand);
    //}
    //void FireLaserTL15()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[48].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(65f, 80f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL15", rand);
    //}
    //void FireLaserTR15()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[49].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-80f, -65f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR15", rand);
    //}
    //void FireLaserTL16()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[50].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(60f, 75f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL16", rand);
    //}
    //void FireLaserTR16()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[51].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-75f, -60f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR16", rand);
    //}
    //void FireLaserTL17()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[52].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(55f, 70f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL17", rand);
    //}
    //void FireLaserTR17()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[53].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(-70f, -55f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR17", rand);
    //}
    //void FireAllLeftSideLasersAtOnce()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(LaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(LaserPrefab);
    //    GameObject laser03 = (GameObject)Instantiate(LaserPrefab);
    //    GameObject laser04 = (GameObject)Instantiate(LaserPrefab);
    //    GameObject laser05 = (GameObject)Instantiate(LaserPrefab);
    //    GameObject laser06 = (GameObject)Instantiate(LaserPrefab);
    //    GameObject laser07 = (GameObject)Instantiate(LaserPrefab);
    //    GameObject laser08 = (GameObject)Instantiate(LaserPrefab);
    //    GameObject laser09 = (GameObject)Instantiate(LaserPrefab);
    //    GameObject laser10 = (GameObject)Instantiate(LaserPrefab);

    //    // set the laser's initial position
    //    laser01.transform.position = LaserPositions[0].transform.position;
    //    laser02.transform.position = LaserPositions[1].transform.position;
    //    laser03.transform.position = LaserPositions[2].transform.position;
    //    laser04.transform.position = LaserPositions[3].transform.position;
    //    laser05.transform.position = LaserPositions[4].transform.position;
    //    laser06.transform.position = LaserPositions[5].transform.position;
    //    laser07.transform.position = LaserPositions[6].transform.position;
    //    laser08.transform.position = LaserPositions[7].transform.position;
    //    laser09.transform.position = LaserPositions[8].transform.position;
    //    laser10.transform.position = LaserPositions[9].transform.position;

    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    laser03.transform.rotation = transform.rotation;
    //    laser04.transform.rotation = transform.rotation;
    //    laser05.transform.rotation = transform.rotation;
    //    laser06.transform.rotation = transform.rotation;
    //    laser07.transform.rotation = transform.rotation;
    //    laser08.transform.rotation = transform.rotation;
    //    laser09.transform.rotation = transform.rotation;
    //    laser10.transform.rotation = transform.rotation;
    //    // set the rotation
    //    float ran = Random.Range(50f, 85f);
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser03.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser04.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser05.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser06.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser07.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser08.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser09.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser10.transform.rotation *= Quaternion.Euler(0, 0, ran);

    //    // set the laser's direction
    //    laser01.GetComponent<EnemyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyLaser>().SetForwardDirection(laser02.transform.right);
    //    laser03.GetComponent<EnemyLaser>().SetForwardDirection(laser03.transform.right);
    //    laser04.GetComponent<EnemyLaser>().SetForwardDirection(laser04.transform.right);
    //    laser05.GetComponent<EnemyLaser>().SetForwardDirection(laser05.transform.right);
    //    laser06.GetComponent<EnemyLaser>().SetForwardDirection(laser06.transform.right);
    //    laser07.GetComponent<EnemyLaser>().SetForwardDirection(laser07.transform.right);
    //    laser08.GetComponent<EnemyLaser>().SetForwardDirection(laser08.transform.right);
    //    laser09.GetComponent<EnemyLaser>().SetForwardDirection(laser09.transform.right);
    //    laser10.GetComponent<EnemyLaser>().SetForwardDirection(laser10.transform.right);

    //    float rand = Random.Range(1f, 2f);
    //    Invoke("FireLeftSideLasers", rand);
    //}
    //void FireLeftSideLasersRandom()
    //{
    //    float rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserL1", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserL2", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserL3", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserL4", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserL5", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserL6", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserL7", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserL8", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserL9", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserL10", rand);
    //}
    //void FireRightSideLasersRandom()
    //{
    //    float rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserR1", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserR2", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserR3", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserR4", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserR5", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserR6", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserR7", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserR8", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserR9", rand);
    //    rand = Random.Range(1f, 2f);
    //    Invoke("FireLaserR10", rand);
    //}
    //void FireTopLasersRandom()
    //{
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL1", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR1", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL2", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR2", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL3", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR3", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL4", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR4", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL5", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR5", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL6", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR6", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL7", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR7", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL8", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR8", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL9", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR9", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL10", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR10", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL11", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR11", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL12", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR12", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL13", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR13", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL14", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR14", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL15", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR15", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL16", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR16", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTL17", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireLaserTR17", rand);
    //}
    //void FireAllHeavyLasersRandom()
    //{
    //    float rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserL1", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserL2", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserL3", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserL4", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserR1", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserR2", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserR3", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserR4", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserFTL", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserFTR", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserFL", rand);
    //    rand = Random.Range(fireLaserRandomMin, fireLaserRandomMax);
    //    Invoke("FireHeavyLaserFR", rand);
    //}
    //void FireHeavyLaserL1()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    // set rotation of turret
    //    float ran = Random.Range(70f, 85f);
    //    HeavyLaserTurrets[0].transform.rotation *= Quaternion.Euler(0, 0, -90 + ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[0].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[1].transform.position;
    //    // reset turret roation
    //    HeavyLaserTurrets[0].transform.rotation *= Quaternion.Euler(0, 0, 90 - ran);
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // set the rotation
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserL1", rand);
    //}
    //void FireHeavyLaserL2()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    float ran = Random.Range(55f, 70f);
    //    HeavyLaserTurrets[1].transform.rotation *= Quaternion.Euler(0, 0, -90 + ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[2].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[3].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // reset turret roation
    //    HeavyLaserTurrets[1].transform.rotation *= Quaternion.Euler(0, 0, 90 - ran);
    //    // set the rotation
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserL2", rand);
    //}
    //void FireHeavyLaserL3()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    float ran = Random.Range(40f, 55f);
    //    HeavyLaserTurrets[2].transform.rotation *= Quaternion.Euler(0, 0, -90 + ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[4].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[5].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // reset turret roation
    //    HeavyLaserTurrets[2].transform.rotation *= Quaternion.Euler(0, 0, 90 - ran);
    //    // set the rotation

    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserL3", rand);
    //}
    //void FireHeavyLaserL4()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    float ran = Random.Range(25f, 40f);
    //    HeavyLaserTurrets[3].transform.rotation *= Quaternion.Euler(0, 0, -90 + ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[6].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[7].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // reset turret roation
    //    HeavyLaserTurrets[3].transform.rotation *= Quaternion.Euler(0, 0, 90 - ran);
    //    // set the rotation
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserL4", rand);
    //}
    //void FireHeavyLaserR1()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    // set turret rotation
    //    float ran = Random.Range(-70f, -85f);
    //    HeavyLaserTurrets[4].transform.rotation *= Quaternion.Euler(0, 0, 90 + ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[8].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[9].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // reset turret roation
    //    HeavyLaserTurrets[4].transform.rotation *= Quaternion.Euler(0, 0, -90 - ran);
    //    // set the rotation
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserR1", rand);
    //}
    //void FireHeavyLaserR2()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    // set turret rotation
    //    float ran = Random.Range(-55f, -70f);
    //    HeavyLaserTurrets[5].transform.rotation *= Quaternion.Euler(0, 0, 90 + ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[10].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[11].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // reset turret rotation
    //    HeavyLaserTurrets[5].transform.rotation *= Quaternion.Euler(0, 0, -90 - ran);
    //    // set the rotation
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserR2", rand);
    //}
    //void FireHeavyLaserR3()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    // set turret rotation
    //    float ran = Random.Range(-35f, -55f);
    //    HeavyLaserTurrets[6].transform.rotation *= Quaternion.Euler(0, 0, 90 + ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[12].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[13].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // reset turret rotation
    //    HeavyLaserTurrets[6].transform.rotation *= Quaternion.Euler(0, 0, -90 - ran);
    //    // set the rotation
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserR3", rand);
    //}
    //void FireHeavyLaserR4()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    // set turret rotation
    //    float ran = Random.Range(-25f, -45f);
    //    HeavyLaserTurrets[7].transform.rotation *= Quaternion.Euler(0, 0, 90 + ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[14].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[15].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // reset turret rotation
    //    HeavyLaserTurrets[7].transform.rotation *= Quaternion.Euler(0, 0, -90 - ran);
    //    // set the rotation
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserR4", rand);
    //}
    //void FireHeavyLaserFTL()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    // set turret rotation
    //    float ran = Random.Range(10f, 25f);
    //    HeavyLaserTurrets[8].transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[16].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[17].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // reset turret rotation
    //    HeavyLaserTurrets[8].transform.rotation *= Quaternion.Euler(0, 0, -ran);
    //    // set the rotation
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserFTL", rand);
    //}
    //void FireHeavyLaserFTR()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    // set turret rotation
    //    float ran = Random.Range(-10f, -25f);
    //    HeavyLaserTurrets[9].transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[18].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[19].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // reset turret rotation
    //    HeavyLaserTurrets[9].transform.rotation *= Quaternion.Euler(0, 0, -ran);
    //    // set the rotation
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserFTR", rand);
    //}
    //void FireHeavyLaserFL()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    // set turret rotation
    //    float ran = Random.Range(0f, 15f);
    //    HeavyLaserTurrets[10].transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[20].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[21].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // reset turret rotation
    //    HeavyLaserTurrets[10].transform.rotation *= Quaternion.Euler(0, 0, -ran);
    //    // set the rotation
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserFL", rand);
    //}
    //void FireHeavyLaserFR()
    //{
    //    // instantiate enemy lasers
    //    GameObject laser01 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    GameObject laser02 = (GameObject)Instantiate(HeavyLaserPrefab);
    //    // set turret rotation
    //    float ran = Random.Range(0f, -15f);
    //    HeavyLaserTurrets[10].transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's initial position
    //    laser01.transform.position = HeavyLaserPositions[20].transform.position;
    //    laser02.transform.position = HeavyLaserPositions[21].transform.position;
    //    // get the direction
    //    laser01.transform.rotation = transform.rotation;
    //    laser02.transform.rotation = transform.rotation;
    //    // reset turret rotation
    //    HeavyLaserTurrets[10].transform.rotation *= Quaternion.Euler(0, 0, -ran);
    //    // set the rotation
    //    laser01.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    laser02.transform.rotation *= Quaternion.Euler(0, 0, ran);
    //    // set the laser's direction
    //    laser01.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser01.transform.right);
    //    laser02.GetComponent<EnemyHeavyLaser>().SetForwardDirection(laser02.transform.right);
    //    float rand = Random.Range(fireHeavyLaserRandomMin, fireHeavyLaserRandomMax);
    //    Invoke("FireHeavyLaserFR", rand);
    //}
}
