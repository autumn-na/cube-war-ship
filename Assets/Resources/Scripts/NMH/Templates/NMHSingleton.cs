using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NMHSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = FindObjectOfType(typeof(T)) as T;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public static T GetInstance()
    {
        if (instance != null)
        {
            return instance;
        }
        else
        {
            return null;
        }
    }
}


