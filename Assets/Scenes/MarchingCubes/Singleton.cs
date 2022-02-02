using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    public static Singleton Instance{ get; private set; }

    public Vector3 start;
    public Vector3 end;
    public bool finishedGen;

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

//     // public static Singleton instance;
//     // public Vector3 start, end;

//     // private void Awake() {
//     //     if (instance != null)
//     //     {
//     //         Destroy(gameObject);
//     //     }
//     //     else
//     //     {
//     //         instance = this;
//     //         DontDestroyOnLoad(gameObject);
//     //     }
//     // }
}
