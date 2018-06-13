using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; set; }
    public int player1Lives;
    public int player2Lives;
    public int currentScore;
    

    //  Level changing system 
    // levels 1-8 will be reserved for the single player missions
    // levels 11-18 will be reserved for 2 player missions
    // Enemy spawner will use this to select the correct spawn pattern for the level
    public static int Level { get; set; }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    
	void Start () {
        Instance = this;
        currentScore = 0;
        //Level = 11; // temp to avoid having to start via title screen - remove when not needed

	}
	

    public void LoadNextLevel()
    {
        Level++;
        switch (Level)
        {
            case 2:
                SceneManager.LoadScene("Level2Players1");
                break;
            case 3:
                SceneManager.LoadScene("Level3Players1");
                break;
            case 4:
                SceneManager.LoadScene("Level4Players1");
                break;
            case 5:
                SceneManager.LoadScene("Level5Players1");
                break;
            case 6:
                SceneManager.LoadScene("Level6Players1");
                break;
            case 7:
                SceneManager.LoadScene("Level7Players1");
                break;
            case 8:
                SceneManager.LoadScene("Level8Players1");
                break;
            case 12:
                SceneManager.LoadScene("Level2Players2");
                break;
            case 13:
                SceneManager.LoadScene("Level3Players2");
                break;
            case 14:
                SceneManager.LoadScene("Level4Players2");
                break;
            case 15:
                SceneManager.LoadScene("Level5Players2");
                break;
            case 16:
                SceneManager.LoadScene("Level6Players2");
                break;
            case 17:
                SceneManager.LoadScene("Level7Players2");
                break;
            case 18:
                SceneManager.LoadScene("Level8Players2");
                break;
            case 9:
            case 19:
                SceneManager.LoadScene("VictoryScene");
                break;
        }
    }
    
}
