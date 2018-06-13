using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

	public static LevelManager Instance { get; set; }
    public static bool counterMeasureActive;
    public static bool is2PlayerGame { get; set; }
    public bool twoPlayerMode;
    public static int playersRemaining { get; set; }
    public static bool isPaused { get; set; }
    public static int activeCounterMeasures { get; set; }

    public GameObject playerShip1;
    public GameObject playerShip2;

    public GameObject playButton;
    public GameObject enemySpawner; // ref to enemy spawner
    public GameObject GameOver; // ref to game over image
    public Text scoreText; // ref to score text ui
    public GameObject powerUpSpawner;  // ref to power up spawner
    public Image levelCompleteImage;
    public Image pausedImage;

    int livesAwardedSofar;
    int playerScore;

    public int Score
    {
        get
        {
            return playerScore;
        }
        set
        {
            playerScore = value;
            CheckIfShouldAwardExtraLife();
            UpdateScoreText();
        }
    }

    public enum GameManagerState
    {
        Opening,
        GamePlay,
        GameOver,
    }

    public GameManagerState GMState;
    
    // Use this for initialization
    void Start () {
        Instance = this;
        is2PlayerGame = twoPlayerMode;
        GMState = GameManagerState.Opening;
        activeCounterMeasures = 0;
    }
    
    // function to update game manager state
    void UpdateGameManagerState()
    {
        switch (GMState)
        {
            case GameManagerState.Opening:
                // hide game over
                GameOver.gameObject.SetActive(false);
                // set play button visible
                playButton.SetActive(true);
                break;

            case GameManagerState.GamePlay:
                // reset players remaining counter
                if (is2PlayerGame)
                    playersRemaining = 2;
                else
                    playersRemaining = 1;
                //reset the score
                Score = GameManager.Instance.currentScore;
                livesAwardedSofar = Score / 50000;
                // hide play button
                playButton.SetActive(false);
                // set the player visible (active) and Init the player lives
                // check if 2 player
                if (is2PlayerGame)
                {
                    GameObject[] playerShips = GameObject.FindGameObjectsWithTag("PlayerShipTag");
                    foreach (GameObject go in playerShips)
                    {
                        var col = go.GetComponent<Collider2D>();
                        col.SendMessage("Init");
                    }
                }
                else
                {
                    GameObject playerShip = GameObject.FindGameObjectWithTag("PlayerShipTag");
                    var col = playerShip.GetComponent<Collider2D>();
                    col.SendMessage("Init");
                }
                //Start Enemy Spawner
                enemySpawner.GetComponent<EnemySpawner>().ScheduleEnemySpawner();
                // start power up spawner
                powerUpSpawner.GetComponent<PowerUpSpawner>().SpawnPowerUp();
                break;

            case GameManagerState.GameOver:
                // stop enemy spawner
                enemySpawner.GetComponent<EnemySpawner>().UnscheduleEnemySpawner();
                // stop power up spawner
                powerUpSpawner.GetComponent<PowerUpSpawner>().StopSpawnPowerUp();
                // display game over
                GameOver.gameObject.SetActive(true);
                // go back to main title screen
                Invoke("LoadTitleScreen", 8f);
                break;
        }
    }

    public void SetGameManagerState(GameManagerState state)
    {
        GMState = state;
        UpdateGameManagerState();
    }

    // play button
    public void StartGamePlay()
    {
        GMState = GameManagerState.GamePlay;
        UpdateGameManagerState();
    }

    // change game manager to opening state
    public void ChangeToOpeningState()
    {
        SetGameManagerState(GameManagerState.Opening);
    }

    public void LoadTitleScreen()
    {
        // Unpause the game
        Time.timeScale = 1f;

        Destroy(GameManager.Instance);
        SceneManager.LoadScene("Title Screen Scene");
    }

    void CheckIfShouldAwardExtraLife()
    {
        // if score is now above 50k, give lives
        if (playerScore >= (livesAwardedSofar + 1) * 50000)
        {
            // award extra life(s)
            if (is2PlayerGame)
            {
                // check if both players are alive
                if (playersRemaining == 1)
                {
                    if (playerShip1.activeSelf)
                    {
                        playerShip1.GetComponent<Collider2D>().SendMessage("GainLifeFromPoints");
                    }
                    else
                    {
                        playerShip1.SetActive(true);
                        playerShip1.GetComponent<Collider2D>().SendMessage("Revived");
                    }
                    if (playerShip2.activeSelf)
                    {
                        playerShip2.GetComponent<Collider2D>().SendMessage("GainLifeFromPoints");
                    }
                    else
                    {
                        playerShip2.SetActive(true);
                        playerShip2.GetComponent<Collider2D>().SendMessage("Revived");
                    }
                }
            }
            else
            {
                playerShip1.GetComponent<Collider2D>().SendMessage("GainLifeFromPoints");
            }
            livesAwardedSofar++;
        }
    }

    void UpdateScoreText()
    {
        string scoreStr = string.Format("{0:0000000}", playerScore);
        scoreText.text = scoreStr;
    }


    // called by bosses as they are destroyed
    public void EndOfLevel()
    {
        // display level complete graphic
        levelCompleteImage.gameObject.SetActive(true);

        // give game manager players current lives totals
        if (is2PlayerGame)
        {
            // both players send their lives totals to the game manager
            // re activate ship and send lives total
            playerShip1.SetActive(true);
            playerShip1.GetComponent<Collider2D>().SendMessage("SendLivesTotal");
            playerShip2.SetActive(true);
            playerShip2.GetComponent<Collider2D>().SendMessage("SendLivesTotal");
        }
        else if (!is2PlayerGame)
        {
            // single player, send lives total to game manager
            playerShip1.GetComponent<Collider2D>().SendMessage("SendLivesTotal");
        }

        // give game manager the current score
        GameManager.Instance.currentScore = playerScore;
        
        // load next level after 3 seconds
        Invoke("LoadNextLevel", 3f);
    }
    private void LoadNextLevel()
    {
        GameManager.Instance.LoadNextLevel();
    }

    public void PauseOrUnpause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            pausedImage.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            pausedImage.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
    }
    
}
