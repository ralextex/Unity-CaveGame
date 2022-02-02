using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    public bool init;

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
        init = true;
    }

    void Update()
    {
        if(init && Singleton.Instance.finishedGen){
            transform.position = Singleton.Instance.end;
            init = false;
        }
    }
}
