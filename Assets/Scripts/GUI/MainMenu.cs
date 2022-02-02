/** 
 *  @brief  Main Menu GUI Object
 *  @author A Lesage 
 *  @date   01-2022
 ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
    * @brief creates main menu
*/
public class MainMenu : MonoBehaviour
{
    /**
        * @brief when button pressed load game
    */
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    /**
        * @brief when button pressed quit game
    */
    public void QuitGame()
    {
        Application.Quit();
    }
}
