/** 
 *  @brief  Game Over GUI Object
 *  @author A Lesage 
 *  @date   01-2022
 ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
    * @brief creates the Game over screen
*/
public class GameOver : MonoBehaviour
{   
    //
    public Text endTimer;

    public void Start()
    {
        endTimer.text = TimerEnd.Instance.timetext.text;
    }

    /**
    * @brief when button pressed load game
    */
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    /**
        * @brief when button pressed quit game
    */
    public void QuitGame()
    {
        Application.Quit();
    }

    /**
    * @brief when button pressed go back to main menu
    */
    public void BackGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 2);
    }
}
