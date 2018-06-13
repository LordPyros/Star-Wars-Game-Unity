using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleScreen : MonoBehaviour {


    // load single player game
    public void StartSinglePlayerGame()
    {
        GameManager.Level = 1;
        SceneManager.LoadScene("Level1Players1");
    }

    // load 2 player game
    public void Start2PlayerGame()
    {
        GameManager.Level = 11;
        SceneManager.LoadScene("Level1Players2");
    }
    // exit game
    public void QuitGame()
    {
        Application.Quit();
    }

}
