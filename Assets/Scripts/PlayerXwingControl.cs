using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerXwingControl : MonoBehaviour {
    //public static PlayerControl Instance { get; set; }
    public GameObject LevelManagerRef; // ref to game manager
    public GameObject ProtonTorpedoPrefab; //ProtonTorpedo prefab
    public GameObject[] PlayerLaser;// laser prefab
    public GameObject laserPosition01;
    public GameObject laserPosition02;
    public GameObject missilePosition01;
    public GameObject missilePosition02;
    public GameObject ExplosionAnim; // explosion prefab
    public GameObject ExplosionSmallAnim;
    public GameObject CounterMeasurePrefab; // countermeasure prefab
    public Text CounterMeasureTextUI;
    private int noOfCounterMeasures;
    private int health;

    public AudioClip firingLasers;
    public AudioClip firingTorpedos;
    
    private bool leftSideMissileFiredLast;
    private bool leftSideLaserFiredLast;
    public GameObject[] Shields;
    private int shieldHealth;
    private int LaserPowerUpLevel;
    private bool counterMeasureActive;
    // game over UI msg for 2 player mode
    public Image gameOverUI;
    // ref to lives ui images
    public Image Lives01;
    public Image Lives02;
    public Image Lives03;
    public Image[] TorpedoUI;
    public Image[] LaserLinkingUI;
    public Image[] TorpedoLinkingUI;
    public Image BombUI;
    public Image CounterMeasureUI;
    bool invincible;
    public int lives { get; set; } // current player lives
    private int torpedos;
    // ship has a bomb flag
    public static bool bombLoaded;
    // weapons linking ( 0 = quadfire, 1 = dualfire, 2 = singlefire modes)
    private int weaponsLinked = 0;
    private SpriteRenderer sr;
    private bool torpedosLinked;
    private float speed;
    private bool canFireManual;
    private bool manualFireMode;
    private bool sFoilClosed;
    private Vector2 min;
    private Vector2 max;
    public GameObject missileTargetGO;
    private List<GameObject> missileTargets;
    private int currentMissileTargetListPos;
    private GameObject target;

    public void Init()
    {
        // laser power up flag
        LaserPowerUpLevel = 1;
        // have bomb ready flag
        bombLoaded = true;
        BombUI.gameObject.SetActive(true);
        // starting lives ( 0 life counts eg. 3 lives really = 4 lives
        if (GameManager.Level == 1 || GameManager.Level == 11)
            lives = Rules.GetPlayerStartingLives();
        else
            lives = GameManager.Instance.player1Lives;
        if (lives < 0)
            lives = 0;
        health = Rules.GetXwingStartingHealth();
        shieldHealth = Rules.GetXwingStartingShields();
        torpedos = Rules.GetXwingStartingTorpedos();
        noOfCounterMeasures = Rules.GetPlayerStartingCounterMeasures();
        // set counter measure UI text
        CounterMeasureTextUI.text = noOfCounterMeasures.ToString();
        // reset player position
        MoveToStartPosition();
        // set torpedos to single fire
        torpedosLinked = false;
        // need to display if torps linked or not
        ChangeTorpedoLinkingUI();
        gameObject.SetActive(true); // needed??
        UpdateShieldSprite(Rules.GetXwingStartingShields());
        invincible = false;
        InvokeRepeating("FireLasersQuad", .5f, .4f);
        ChangeLivesUI();
        ChangeTorpedoUI();
        canFireManual = true;
        manualFireMode = false;
        sFoilClosed = false;
        speed = Rules.GetXwingSpeed();
    }

	void Start () {

        //Instance = this;
        invincible = true;
        sr = gameObject.GetComponent<SpriteRenderer>();
        // bottom left of screen
        min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));
        // top right of screen
        max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        missileTargets = new List<GameObject>();
    }
	
	void Update ()
    {

        // move target sprite to current targets location if its still alive
        if (missileTargetGO.activeSelf)
        {
            if (target != null && target.activeInHierarchy)
            {
                if (target.transform.position.x > min.x && target.transform.position.x < max.x && target.transform.position.y > min.y && target.transform.position.y < max.y)
                {
                    missileTargetGO.transform.position = target.transform.position;
                }
                else
                {
                    // target is out of screen
                    missileTargetGO.SetActive(false);
                    target = null;
                }
            }
            else
            {
                missileTargetGO.SetActive(false);
            }
        }

        // Pause / Unpause game
        if (Input.GetButtonDown("P1ButtonStart"))
        {
            LevelManager.Instance.PauseOrUnpause();
        }

        // change target
        if (Input.GetButtonDown("P1ButtonR1"))
        {
            // check the player has missiles
            if (torpedos > 0 && !LevelManager.isPaused)
            {
                // check if there is an active list of targets
                if (missileTargets.Count == 0)
                {
                    CreateListSetTarget();
                }
                else
                {
                    SelectNextTarget();
                }
            }
        }

        // toggle s-foils
        if (Input.GetButtonDown("P1ButtonRT") && !LevelManager.isPaused)
        {
            if (sFoilClosed)
            {
                // open S foils and allow laser firing, reset speed
                sFoilClosed = false;
                speed = Rules.GetXwingSpeed();
                if (!manualFireMode)
                    StartFiringLasers();
            }
            else
            {
                sFoilClosed = true;
                speed = Rules.GetXwingSpeed() + 3f;
                if (!manualFireMode)
                    StopLasersFiringAuto();
            }
        }

        // toggle manual/auto fire
        if (Input.GetButtonDown("P1ButtonRS") && LevelManager.Instance.GMState.ToString() == "GamePlay" && !LevelManager.isPaused)
        {
            if (!manualFireMode)
            {
                StopLasersFiringAuto();
            }
            else
            {
                if (!sFoilClosed)
                    StartFiringLasers();
            }
            manualFireMode = !manualFireMode;
        }
        
        // manual fire
        if (Input.GetButtonDown("P1ButtonA") && !sFoilClosed)
        {
            if (manualFireMode && canFireManual && !LevelManager.isPaused)
            {
                canFireManual = false;
                // get laser linking
                if (weaponsLinked == 0)
                {
                    FireLasersQuad();
                    Invoke("CanFireManual", .4f);
                }
                else if (weaponsLinked == 1)
                {
                    FireLasersDual();
                    Invoke("CanFireManual", .2f);
                }
                else if (weaponsLinked == 2)
                {
                    FireLasersSingle();canFireManual = false;
                    Invoke("CanFireManual", .1f);
                }
            }
        }
        
        // fire counter measure
        if (Input.GetButtonDown("P1ButtonX"))
        {
            // check if player has counter measure
            if (noOfCounterMeasures > 0 && !LevelManager.isPaused)
            {
                GameObject counterMeasure = Instantiate(CounterMeasurePrefab);
                counterMeasure.transform.position = transform.position;
                noOfCounterMeasures--;
                // update UI
                CounterMeasureTextUI.text = noOfCounterMeasures.ToString();
            }
        }
        
        // torpedo linking
        if (Input.GetButtonDown("P1ButtonLT") && !LevelManager.isPaused)
        {
            torpedosLinked = !torpedosLinked;
            ChangeTorpedoLinkingUI();
        }

        // use bomb
        if (Input.GetButtonDown("P1ButtonY"))
        {
            if (bombLoaded && !LevelManager.isPaused)
            {
                DestroyAllEnemies();
                bombLoaded = false;
                BombUI.gameObject.SetActive(false);
            }
        }

        // fire missile when M key is pressed
        if (Input.GetButtonDown("P1ButtonB"))
        {
            // check there is ammo available to shoot
            if (torpedos > 0 && !LevelManager.isPaused)
            {
                // play missile sound
                gameObject.GetComponent<AudioSource>().PlayOneShot(firingTorpedos);

                if (torpedosLinked && torpedos > 1)
                {
                    // fire 2 torpedos at once
                    GameObject missilePos1 = Instantiate(ProtonTorpedoPrefab);
                    missilePos1.transform.position = missilePosition01.transform.position;
                    GameObject missilePos2 = Instantiate(ProtonTorpedoPrefab);
                    missilePos2.transform.position = missilePosition02.transform.position;
                    torpedos -= 2;
                    // set torpedos target
                    if (target != null && target.activeInHierarchy)
                    {
                        missilePos1.GetComponent<Collider2D>().SendMessage("SetTarget", target);
                        missilePos1.GetComponent<Collider2D>().SendMessage("SetTarget", target);
                    }
                }
                else
                {
                    // find which missile position to use
                    if (leftSideMissileFiredLast)
                    {
                        GameObject missilePos1 = Instantiate(ProtonTorpedoPrefab);
                        missilePos1.transform.position = missilePosition01.transform.position;
                        // set torpedos target
                        if (target != null && target.activeInHierarchy)
                            missilePos1.GetComponent<Collider2D>().SendMessage("SetTarget", target);
                    }
                    else
                    {
                        GameObject missilePos2 = Instantiate(ProtonTorpedoPrefab);
                        missilePos2.transform.position = missilePosition02.transform.position;
                        // set torpedos target
                        if (target != null && target.activeInHierarchy)
                            missilePos2.GetComponent<Collider2D>().SendMessage("SetTarget", target);
                    }
                    torpedos--;

                    leftSideMissileFiredLast = !leftSideMissileFiredLast;
                }
                if (torpedos <= 0)
                    missileTargetGO.SetActive(false);
                ChangeTorpedoUI();
            }
        }
        
        // change laser linking when space is pressed
        if (Input.GetButtonDown("P1ButtonL1") && LevelManager.Instance.GMState.ToString() == "GamePlay" && !LevelManager.isPaused)
        {
            weaponsLinked++;
            if (weaponsLinked == 3)
                weaponsLinked = 0;
            ChangeLaserLinkingUI();
            if (!manualFireMode)
                StartFiringLasers();
        }

        float x = Input.GetAxisRaw("Horizontal");// value will be -1, 0, 1 (left, no input, right)
        float y = Input.GetAxisRaw("Vertical");// (down, no input, up)
        
        // now based on the input we compute a direction vector, and we normalize it to get a unit vector
        Vector2 direction = new Vector2(x, y).normalized;
        // compute and set players position
        Move(direction);
	}

    void Move (Vector2 direction)
    {
        // Find the screen limits to the player's movement
        Vector2 min = Camera.main.ViewportToWorldPoint(new Vector2(0, 0));// bottom left corner of screen
        Vector2 max = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));// top right corner

        max.x = max.x - 0.225f; // subtract the player sprite half width
        max.x = max.x + 0.225f; // add the player sprite half width

        max.y = max.y - 0.225f; // subtract the player sprite half height
        max.y = max.y + 0.225f; // add the player sprite half height

        // get player position
        Vector2 pos = transform.position;

        // calculate new position
        pos += direction * speed * Time.deltaTime;

        // make sure new position is not outside screen
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);

        // update player position
        transform.position = pos;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
            // Detect collision of the player ship with an enemy ship or with an enemy laser
            switch (col.tag)

            {
                case "EnemyShipTag":

                    if (!invincible)
                        TakeDamage(Rules.GetShipCollisionDamage());
                    break;
                case "EnemyCapitalShipTag":
                case "LeftShieldGenTag":
                case "RightShieldGenTag":
                    if (!invincible)
                        LoseALife();
                    break;
                case "PowerUpShieldTag":
                    shieldHealth = Rules.GetXwingMaxShields();
                    UpdateShieldSprite(shieldHealth);
                    break;
                case "PowerUpLaserTag":
                    // tell the power up what laser power level message to display
                    col.SendMessage("SpawnLaserPowerLevelMessage", LaserPowerUpLevel);
                    break;
            }
    }

    public void TakeDamage(int damage)
    {
        if (!invincible && health > 0)
        {
            // if no shields, take health damage
            if (shieldHealth <= 0)
            {
                health -= damage;
            }
            // if shields, take shield damage, if there is excess damage, take that as health damage
            else
            {
                shieldHealth -= damage;
                UpdateShieldSprite(shieldHealth);
                if (shieldHealth < 0)
                {
                    health += shieldHealth;
                }
            }
            // check if ship is dead
            if (health <= 0)
                LoseALife();
        }
    }

    void LoseALife()
    {
        // explosion anim
        PlayExplosion();
        // turn invincible
        invincible = true;
        
        
        // subtract life
        lives--;
        // update lives UI
        ChangeLivesUI();
        if (lives >= 0)
        {
            // make ship flash and reposition to bottom center
            MakeShipFlash();
            Invoke("StopShipFlashing", 3f);
            // reset player position
            MoveToStartPosition();
            // reset lasers back to starting power (no upgrades)
            LaserPowerUpLevel = 1;
            // give player bomb
            bombLoaded = true;
            // get starting shield health and update UI
            shieldHealth = Rules.GetXwingStartingShields();
            UpdateShieldSprite(shieldHealth);
            // get starting health
            health = Rules.GetXwingStartingHealth();
            // get starting torpedos and update UI
            torpedos = Rules.GetXwingStartingTorpedos();
            ChangeTorpedoUI();
            // turn off missile target
            missileTargetGO.SetActive(false);
        }
        // remove invincible after a few seconds
        Invoke("RemoveInvincible", 3f);
        // destroy all enemy fighters and projectiles
        if (!LevelManager.is2PlayerGame)
            DestroyAllEnemies();
        
        // if lives less than 0, change state to game over, disable player ship 
        if (lives < 0)
        {
            LevelManager.playersRemaining--;
            // stop lasers
            StopLasersFiringAuto();
            if (LevelManager.playersRemaining < 1)
            {
                DestroyEverythingGameOver();

                LevelManagerRef.GetComponent<LevelManager>().SetGameManagerState(LevelManager.GameManagerState.GameOver);
            }

            DisablePlayersUI();
            
            gameObject.SetActive(false);
        }
    }

    void RemoveInvincible()
    {
        invincible = false;
    }

    // function to instantiate explosion
    void PlayExplosion()
    {
        GameObject explosion = (GameObject)Instantiate(ExplosionAnim);

        explosion.transform.position = transform.position;
    }
    
    public void EnableBombUI()
    {
        BombUI.gameObject.SetActive(true);
    }
    public void ChangeLivesUI()
    {
        if (lives >= 3)
        {
            Lives01.gameObject.SetActive(true);
            Lives02.gameObject.SetActive(true);
            Lives03.gameObject.SetActive(true);
        }
        else if (lives == 2)
        {
            Lives01.gameObject.SetActive(true);
            Lives02.gameObject.SetActive(true);
            Lives03.gameObject.SetActive(false);
        }
        else if (lives == 1)
        {
            Lives01.gameObject.SetActive(true);
            Lives02.gameObject.SetActive(false);
            Lives03.gameObject.SetActive(false);
        }
        else
        {
            Lives01.gameObject.SetActive(false);
            Lives02.gameObject.SetActive(false);
            Lives03.gameObject.SetActive(false);
        }
    }
    public void ChangeTorpedoUI()
    {
        switch (torpedos)
        {
            case 0:
                TorpedoUI[0].gameObject.SetActive(false);
                TorpedoUI[1].gameObject.SetActive(false);
                TorpedoUI[2].gameObject.SetActive(false);
                TorpedoUI[3].gameObject.SetActive(false);
                TorpedoUI[4].gameObject.SetActive(false);
                TorpedoUI[5].gameObject.SetActive(false);
                break;
            case 1:
                TorpedoUI[0].gameObject.SetActive(true);
                TorpedoUI[1].gameObject.SetActive(false);
                TorpedoUI[2].gameObject.SetActive(false);
                TorpedoUI[3].gameObject.SetActive(false);
                TorpedoUI[4].gameObject.SetActive(false);
                TorpedoUI[5].gameObject.SetActive(false);
                break;
            case 2:
                TorpedoUI[0].gameObject.SetActive(true);
                TorpedoUI[1].gameObject.SetActive(true);
                TorpedoUI[2].gameObject.SetActive(false);
                TorpedoUI[3].gameObject.SetActive(false);
                TorpedoUI[4].gameObject.SetActive(false);
                TorpedoUI[5].gameObject.SetActive(false);
                break;
            case 3:
                TorpedoUI[0].gameObject.SetActive(true);
                TorpedoUI[1].gameObject.SetActive(true);
                TorpedoUI[2].gameObject.SetActive(true);
                TorpedoUI[3].gameObject.SetActive(false);
                TorpedoUI[4].gameObject.SetActive(false);
                TorpedoUI[5].gameObject.SetActive(false);
                break;
            case 4:
                TorpedoUI[0].gameObject.SetActive(true);
                TorpedoUI[1].gameObject.SetActive(true);
                TorpedoUI[2].gameObject.SetActive(true);
                TorpedoUI[3].gameObject.SetActive(true);
                TorpedoUI[4].gameObject.SetActive(false);
                TorpedoUI[5].gameObject.SetActive(false);
                break;
            case 5:
                TorpedoUI[0].gameObject.SetActive(true);
                TorpedoUI[1].gameObject.SetActive(true);
                TorpedoUI[2].gameObject.SetActive(true);
                TorpedoUI[3].gameObject.SetActive(true);
                TorpedoUI[4].gameObject.SetActive(true);
                TorpedoUI[5].gameObject.SetActive(false);
                break;
            case 6:
                TorpedoUI[0].gameObject.SetActive(true);
                TorpedoUI[1].gameObject.SetActive(true);
                TorpedoUI[2].gameObject.SetActive(true);
                TorpedoUI[3].gameObject.SetActive(true);
                TorpedoUI[4].gameObject.SetActive(true);
                TorpedoUI[5].gameObject.SetActive(true);
                break;
        }
    }

    private void ChangeTorpedoLinkingUI()
    {
        switch (torpedosLinked)
        {
            case false:
                TorpedoLinkingUI[0].gameObject.SetActive(true);
                TorpedoLinkingUI[1].gameObject.SetActive(false);
                break;
            case true:
                TorpedoLinkingUI[0].gameObject.SetActive(true);
                TorpedoLinkingUI[1].gameObject.SetActive(true);
                break;
        }
    }
    private void ChangeLaserLinkingUI()
    {
        // Update the UI to show current laser linking status
        switch (weaponsLinked)
        {
            case 0:
                LaserLinkingUI[0].gameObject.SetActive(true);
                LaserLinkingUI[1].gameObject.SetActive(true);
                LaserLinkingUI[2].gameObject.SetActive(true);
                LaserLinkingUI[3].gameObject.SetActive(true);
                break;
            case 1:
                LaserLinkingUI[0].gameObject.SetActive(true);
                LaserLinkingUI[1].gameObject.SetActive(false);
                LaserLinkingUI[2].gameObject.SetActive(false);
                LaserLinkingUI[3].gameObject.SetActive(true);
                break;
            case 2:
                LaserLinkingUI[0].gameObject.SetActive(true);
                LaserLinkingUI[1].gameObject.SetActive(false);
                LaserLinkingUI[2].gameObject.SetActive(false);
                LaserLinkingUI[3].gameObject.SetActive(false);
                break;
            case 3:
                LaserLinkingUI[0].gameObject.SetActive(false);
                LaserLinkingUI[1].gameObject.SetActive(false);
                LaserLinkingUI[2].gameObject.SetActive(false);
                LaserLinkingUI[3].gameObject.SetActive(false);
                break;

        }
    }
    public void UpdateCounterMeasureUIText()
    {
        CounterMeasureTextUI.text = noOfCounterMeasures.ToString();
    }

    private void UpdateShieldSprite(int s)
    {
        // turn off all shield sprites so that one can be enabled and play its shield hit anim
        Shields[0].gameObject.SetActive(false);// blue shield prefab
        Shields[1].gameObject.SetActive(false);// green shield prefab
        Shields[2].gameObject.SetActive(false);// yellow shield prefab
        Shields[3].gameObject.SetActive(false);// red shield prefab
        // blue shield
        if (s > 4)
        {
            Shields[0].gameObject.SetActive(true);
        }
        else
        {
            switch (s)
            {
                case 0: // no shield, make shields flash red then dissappear
                    Shields[3].gameObject.SetActive(true);// red shield prefab
                    Invoke("DisableRedShieldSprite", .4f);
                    break;
                case 1:// red shield
                    Shields[3].gameObject.SetActive(true);
                    break;
                case 2:// yellow shield
                    Shields[2].gameObject.SetActive(true);
                    break;
                case 3:// green shield
                case 4:
                    Shields[1].gameObject.SetActive(true);
                    break;
            }
        }

    }
    private void DisableRedShieldSprite()
    {
        Shields[3].gameObject.SetActive(false);// red shield prefab
    }

    void DestroyEverythingGameOver()
    {
        DestroyAllEnemies();
        GameObject[] go = GameObject.FindGameObjectsWithTag("EnemyCapitalShipTag");
        for (var i = 0; i < go.Length; i++)
        {
            GameObject explosion = Instantiate(ExplosionAnim);

            explosion.transform.position = go[i].transform.position;

            Destroy(go[i]);
        }
        

    }

    void DestroyAllEnemies()
    {
        GameObject[] go = GameObject.FindGameObjectsWithTag("EnemyShipTag");
        for (var i = 0; i < go.Length; i++)
        {
            GameObject explosion = Instantiate(ExplosionAnim);

            explosion.transform.position = go[i].transform.position;

            Destroy(go[i]);
        }

        go = GameObject.FindGameObjectsWithTag("EnemyLaserTag");
        for (var i = 0; i < go.Length; i++)
        {
            GameObject explosion = Instantiate(ExplosionSmallAnim);

            explosion.transform.position = go[i].transform.position;
            Destroy(go[i]);
        }
        go = GameObject.FindGameObjectsWithTag("EnemyHeavyLaserTag");
        for (var i = 0; i < go.Length; i++)
        {
            GameObject explosion = Instantiate(ExplosionSmallAnim);

            explosion.transform.position = go[i].transform.position;
            Destroy(go[i]);
        }
        go = GameObject.FindGameObjectsWithTag("EnemyProtonTorpedoTag");
        for (var i = 0; i < go.Length; i++)
        {
            GameObject explosion = Instantiate(ExplosionSmallAnim);

            explosion.transform.position = go[i].transform.position;
            Destroy(go[i]);
        }
        go = GameObject.FindGameObjectsWithTag("EnemyConcussionMissileTag");
        for (var i = 0; i < go.Length; i++)
        {
            GameObject explosion = Instantiate(ExplosionSmallAnim);

            explosion.transform.position = go[i].transform.position;
            Destroy(go[i]);
        }
    }

    void FireLasersDual()
    {

        // play laser sound
        float volume = 0.2f;
        gameObject.GetComponent<AudioSource>().PlayOneShot(firingLasers, volume);

        // instatiate first laser
        GameObject laser01 = Instantiate(PlayerLaser[LaserPowerUpLevel - 1]);
        laser01.transform.position = laserPosition01.transform.position;// set laser initial position

        // second laser
        GameObject laser02 = Instantiate(PlayerLaser[LaserPowerUpLevel - 1]);
        laser02.transform.position = laserPosition02.transform.position;
    }
    void FireLasersQuad()
    {

        // play laser sound
        float volume = 0.2f;
        gameObject.GetComponent<AudioSource>().PlayOneShot(firingLasers, volume);

        // instatiate first laser
        GameObject laser01 = Instantiate(PlayerLaser[LaserPowerUpLevel - 1]);
        laser01.transform.position = laserPosition01.transform.position;// set laser initial position

        // second laser
        GameObject laser02 = Instantiate(PlayerLaser[LaserPowerUpLevel - 1]);
        laser02.transform.position = laserPosition02.transform.position;
        
        // instatiate third laser
        GameObject laser03 = Instantiate(PlayerLaser[LaserPowerUpLevel - 1]);
        laser03.transform.position = laserPosition01.transform.position;// set laser initial position

        // fourth laser
        GameObject laser04 = Instantiate(PlayerLaser[LaserPowerUpLevel - 1]);
        laser04.transform.position = laserPosition02.transform.position;
    }
    void FireLasersSingle()
    {
        // play laser sound
        float volume = 0.2f;
        gameObject.GetComponent<AudioSource>().PlayOneShot(firingLasers, volume);

        if (leftSideLaserFiredLast)
        {
            // instatiate first laser
            GameObject laser01 = Instantiate(PlayerLaser[LaserPowerUpLevel - 1]);
            laser01.transform.position = laserPosition01.transform.position;// set laser initial position
        }
        else
        {
            // second laser
            GameObject laser02 = Instantiate(PlayerLaser[LaserPowerUpLevel - 1]);
            laser02.transform.position = laserPosition02.transform.position;
        }
        leftSideLaserFiredLast = !leftSideLaserFiredLast;
    }
    
    void MakeShipFlash()
    {
        InvokeRepeating("MakeShipFlashOff", 0f, .4f);
        InvokeRepeating("MakeShipFlashOn", .2f, .4f);
        
    }
    void StopShipFlashing()
    {
        // stop ship flashing and turn sprites on
        CancelInvoke("MakeShipFlashOff");
        CancelInvoke("MakeShipFlashOn");
        MakeShipFlashOn();
    }
    void MakeShipFlashOff()
    {
        // turn off ship sprite
        sr.enabled = false;
        // turn off shield sprite
        Shields[1].gameObject.SetActive(false);// green shield prefab
        // turn off engine blur
        var a = GameObject.Find("PlayerEngineBlurAnimLeft").GetComponent<SpriteRenderer>();
        a.enabled = false;
        var b = GameObject.Find("PlayerEngineBlurAnimRight").GetComponent<SpriteRenderer>();
        b.enabled = false;
    }
    void MakeShipFlashOn()
    {
        sr.enabled = true;
        Shields[1].gameObject.SetActive(true);// green shield prefab
        var a = GameObject.Find("PlayerEngineBlurAnimLeft").GetComponent<SpriteRenderer>();
        a.enabled = true;
        var b = GameObject.Find("PlayerEngineBlurAnimRight").GetComponent<SpriteRenderer>();
        b.enabled = true;
    }

    public void LaserPowerUpReceived()
    {
        LaserPowerUpLevel++;
        if (LaserPowerUpLevel > 5)
            LaserPowerUpLevel = 5;
    }
    public void TorpedoReloadreceived()
    {
        torpedos = Rules.GetXwingStartingTorpedos();
        noOfCounterMeasures = Rules.GetPlayerStartingCounterMeasures();
    }
    public void BombReloadReceived()
    {
        bombLoaded = true;
        noOfCounterMeasures = Rules.GetPlayerStartingCounterMeasures();
    }
    public void MoveToStartPosition()
    {
        // top right screen corner
        Vector2 topRightOfScreen = Camera.main.ViewportToWorldPoint(new Vector2(1, 1));
        // Move ship to starting point
        transform.position = new Vector2((topRightOfScreen.x / 2) * -1, (topRightOfScreen.y / 2) * -1);
        if (!LevelManager.is2PlayerGame)
        {
            transform.position = new Vector2(0, (topRightOfScreen.y / 2) * -1);
        }
    }

    private void DisablePlayersUI()
    {
        foreach (Image i in TorpedoUI)
        {
            i.gameObject.SetActive(false);
        }
        foreach (Image i in LaserLinkingUI)
        {
            i.gameObject.SetActive(false);
        }
        foreach (Image i in TorpedoLinkingUI)
        {
            i.gameObject.SetActive(false);
        }

        BombUI.gameObject.SetActive(false);
        CounterMeasureUI.gameObject.SetActive(false);
        gameOverUI.gameObject.SetActive(true);

    }
    private void EnablePlayersUI()
    {
        foreach (Image i in TorpedoUI)
        {
            i.gameObject.SetActive(true);
        }
        foreach (Image i in LaserLinkingUI)
        {
            i.gameObject.SetActive(true);
        }
        foreach (Image i in TorpedoLinkingUI)
        {
            i.gameObject.SetActive(true);
        }

        BombUI.gameObject.SetActive(true);
        CounterMeasureUI.gameObject.SetActive(true);
        gameOverUI.gameObject.SetActive(false);
    }
    
    public void GainLifeFromPoints()
    {
        lives++;
        ChangeLivesUI();
    }

    public void Revived()
    {
        EnablePlayersUI();
        LevelManager.playersRemaining++;
        // laser power up flag
        LaserPowerUpLevel = 1;
        // have bomb ready flag
        bombLoaded = true;
        BombUI.gameObject.SetActive(true);
        lives = 0;
        health = Rules.GetXwingStartingHealth();
        shieldHealth = Rules.GetXwingStartingShields();
        torpedos = Rules.GetXwingStartingTorpedos();
        noOfCounterMeasures = Rules.GetPlayerStartingCounterMeasures();
        // set counter measure UI text
        CounterMeasureTextUI.text = noOfCounterMeasures.ToString();
        // reset player position
        MoveToStartPosition();
        
        UpdateShieldSprite(Rules.GetXwingStartingShields());
        invincible = true;
        Invoke("RemoveInvincible", 3f);
        StartFiringLasers();
        ChangeLivesUI();
        ChangeTorpedoUI();
        ChangeTorpedoLinkingUI();
        ChangeLaserLinkingUI();
    }

    private void StartFiringLasers()
    {
        // find current laser linking settings and start firing
        switch (weaponsLinked)
        {
            case 0:
                CancelInvoke("FireLasersSingle");
                InvokeRepeating("FireLasersQuad", 0, .4f);
                break;
            case 1:
                CancelInvoke("FireLasersQuad");
                InvokeRepeating("FireLasersDual", 0, .2f);
                break;
            case 2:
                CancelInvoke("FireLasersDual");
                InvokeRepeating("FireLasersSingle", 0, .1f);
                break;
        }
    }
    private void StopLasersFiringAuto()
    {
        if (!manualFireMode)
        {
            // stop lasers firing
            if (weaponsLinked == 0)
            {
                CancelInvoke("FireLasersQuad");
            }
            else if (weaponsLinked == 1)
            {
                CancelInvoke("FireLasersDual");
            }
            else if (weaponsLinked == 2)
            {
                CancelInvoke("FireLasersSingle");
            }
        }
    }

    private void CanFireManual()
    {
        canFireManual = true;
    }

    public void SendLivesTotal()
    {
        // sends life total to GameManager at the end of each level
        GameManager.Instance.player1Lives = lives;
    }

    private void ClearMissileTargetsList()
    {
        // clear the possible missile targets array
        missileTargets.Clear();
    }

    private void CreateListSetTarget()
    {
        CancelInvoke("ClearMissileTargetsList");
        ClearMissileTargetsList();
        // create a list and select the first ship, start a timer to clear the list (enable sprite)
        GameObject[] go = GameObject.FindGameObjectsWithTag("PosMissileTargetTag");
        foreach (GameObject g in go)
        {
            missileTargets.Add(g);
        }
        // check array is not empty
        if (missileTargets.Count > 0)
        {
            target = missileTargets[0];
            // target found
            if (target != null && target.activeInHierarchy)
            {
                // check its inside the screen
                if (target.transform.position.x > min.x && target.transform.position.x < max.x && target.transform.position.y > min.y && target.transform.position.y < max.y)
                {
                    // put target icon on new target
                    missileTargetGO.SetActive(true);
                    missileTargetGO.transform.position = target.transform.position;
                    // set current index position
                    currentMissileTargetListPos = 0;
                    Invoke("ClearMissileTargetsList", 2f);
                    Debug.Log("Targeted - " + target.transform.root.name);
                }
                // outside of the screen, find a new target
                else
                {
                    SelectNextTarget();
                }
            }
            // target is dead, find a new target
            else
            {
                SelectNextTarget();
            }
        }
        // empty array, not targets available
        else
        {
            target = null;
            missileTargetGO.SetActive(false);
        }
        
    }
    private void SelectNextTarget()
    {
        // yes, check which ship is currently selected and select the next one, move the sprite to the new target (enable sprite)
        currentMissileTargetListPos++;
        // check target is inside the List
        if (currentMissileTargetListPos < missileTargets.Count)
        {
            // target is still alive and inside screen
            if (missileTargets[currentMissileTargetListPos] != null && missileTargets[currentMissileTargetListPos].activeInHierarchy)
            {
                if (missileTargets[currentMissileTargetListPos].transform.position.x > min.x && missileTargets[currentMissileTargetListPos].transform.position.x < max.x && missileTargets[currentMissileTargetListPos].transform.position.y > min.y && missileTargets[currentMissileTargetListPos].transform.position.y < max.y)
                {
                    // select target
                    target = missileTargets[currentMissileTargetListPos];
                    // move missile target sprite to targets location
                    missileTargetGO.transform.position = target.transform.position;
                    missileTargetGO.SetActive(true);
                }
                else
                {
                    SelectNextTarget();
                }
            }
            // target is dead, try next go in list
            else
            {
                SelectNextTarget();
            }
        }
        // end of list, create a new one and set target
        else
        {
            CreateListSetTarget();
        }
    }
}
