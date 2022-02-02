/** 
 *  @brief  Camera Controller Class
 *  @author A Lesage 
 *  @date   01-2022
 ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
 
/**
    * @brief controlls the camera for the player
*/
public class CameraController : MonoBehaviour {
    void Start()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    /**
    * @brief get axis input from player
    * @param axisName axis that should be used
    */
    public float GetAxisCustom(string axisName)
    {
        if(axisName == "Mouse X")
        {
            // on click
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse X");
            } 
            else
            {
                return 0;
            }
        }
        else if (axisName == "Mouse Y")
        {
            // on click
            if (Input.GetMouseButton(1))
            {
                return UnityEngine.Input.GetAxis("Mouse Y");
            } 
            else
            {
                return 0;
            }
        }
        return UnityEngine.Input.GetAxis(axisName);
    }
}