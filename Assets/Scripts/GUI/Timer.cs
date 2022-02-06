/** 
 *  @brief  Timer GUI Object
 *  @author A Lesage 
 *  @date   01-2022
 ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
* @brief creates a timer
*/
public class Timer : MonoBehaviour
{
    // declare Text
    public Text timerText;
    // time at which game was started
    private float startTime;
    // if game is finnished
    private bool finnished = false;

    /**
    * @brief give start time value
    */
    void Start()
    {
        startTime = Time.time;
    }

    /**
    * @brief update time on timer
    */
    void Update()
    {
        if(finnished)
        {
            return;
        }

        else
        {
           float t = Time.time - startTime;

            string minutes = ((int) t / 60).ToString();
            string seconds = (t % 60).ToString("f2");

            timerText.text = minutes + ":" + seconds; 
        }
    }

    /**
    * @brief if finnished 
    */
    public void Finnish()
    {
        TimerEnd.Instance.timetext = timerText;
        finnished = true;
        timerText.color = Color.yellow;
    }
}
