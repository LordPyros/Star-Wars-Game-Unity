using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour {

    int livesAwardedSofar;
    Text scoreText;
    int playerScore;
    public static GameScore Instance { get; set; }

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

    void CheckIfShouldAwardExtraLife()
    {
        // if score is now above 50k, give lives
        if (playerScore >= (livesAwardedSofar + 1) * 30000)
        {
            // award extra life(s)
            if (LevelManager.is2PlayerGame)
            {
                GameObject[] playerShips = GameObject.FindGameObjectsWithTag("PlayerShipTag");
                foreach (GameObject go in playerShips)
                {
                    if (!go.activeSelf)
                    {
                        go.SetActive(true);
                        Collider2D col = go.GetComponent<Collider2D>();
                        col.SendMessage("Revived");
                    }
                    else
                    {
                        Collider2D col = go.GetComponent<Collider2D>();
                        col.SendMessage("GainLifeFromPoints");
                    }
                }
            }
            else
            {
                Collider2D playerShip = GameObject.FindGameObjectWithTag("PlayerShipTag").GetComponent<Collider2D>();
                playerShip.SendMessage("GainLifeFromPoints");
            }
            livesAwardedSofar++;
        }
    }

	// Use this for initialization
	void Start () {
        Instance = this;
        // get the text ui component of gameobject
        scoreText = GetComponent<Text>();
	}

    void UpdateScoreText()
    {
        string scoreStr = string.Format("{0:0000000}", playerScore);
        scoreText.text = scoreStr;
    }
	
}
