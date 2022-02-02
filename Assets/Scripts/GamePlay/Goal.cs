/** 
 *  @brief  Goal Object
 *  @author A Lesage 
 *  @date   01-2022
 ***********************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
    * @brief creates the finish flag
*/
public class Goal : MonoBehaviour
{
    // flag for initialisation
    public bool initFlag;

    /**
        * @brief When colliderbox is trigged Load gameover screen
    */
    private void OnTriggerEnter(Collider other) 
    {
        PlayerController component = other.gameObject.GetComponent<PlayerController>();
        if (component != null)
        {
            other.SendMessage("Finnish");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    private void Start()
    {
        initFlag = true;
    }

    /**
        * @brief moves flag to given end point
    */
    void Update()
    {
        if(initFlag && GlobalPoints.Instance.finishedGen){
            transform.position = GlobalPoints.Instance.end;
            initFlag = false;
        }
    }
}
