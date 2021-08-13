using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KMJOption : MonoBehaviour
{
    public GameObject option;


    void Start()
    {
        option.SetActive(false);
    }
    public void optionSetActive()
    {
        option.SetActive(!option.activeInHierarchy);

        if (option.activeInHierarchy)
        {
            Time.timeScale = 0.0f;
            Debug.Log("시발");
        }
        else
            Time.timeScale = 1.0f;
    }
}
