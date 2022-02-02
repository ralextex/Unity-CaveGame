/** 
 *  @brief  Shared Variable between Objects
 *  @author A Lesage 
 *  @date   01-2022
 ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
* @brief start and ending point 
*/
public class GlobalPoints : MonoBehaviour
{
    // self reference
    public static GlobalPoints Instance{ get; private set; }

    // start point
    public Vector3 start;
    // end point
    public Vector3 end;

    // if generation is finished
    public bool finishedGen;

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

