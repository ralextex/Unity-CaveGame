/** 
 *  @brief  Shared Variable between Objects
 *  @author A Lesage 
 *  @date   01-2022
 ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/**
* @brief start and ending point 
*/
public class TimerEnd : MonoBehaviour
{
    // self reference
    public static TimerEnd Instance{ get; private set; }

    // timer text 
    public Text timetext;

    // flag for variables not to be changed
    private void Awake() 
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}

