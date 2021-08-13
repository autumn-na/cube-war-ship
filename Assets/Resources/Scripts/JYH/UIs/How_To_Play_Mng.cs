using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class How_To_Play_Mng : MonoBehaviour {

    public GameObject AddServerScene;
    public GameObject SearchScene;
    public GameObject OptionScene;
    public GameObject RoomScene;
    
	void Start () {
		
	}
	
	void Update () {
		
	}

    public void Room()
    {
        RoomScene.SetActive(true);
    }

    public void AddServer()
    {
        AddServerScene.SetActive(true);
    }

    public void AddServerOut()
    {
        AddServerScene.SetActive(false);
    }

    public void Search()
    {
        SearchScene.SetActive(true);
    }

    public void SearchOut()
    {
        SearchScene.SetActive(false);
    }
    
    public void Back()
    {
        SceneManager.LoadScene(0);
    }

    public void Option()
    {
        OptionScene.SetActive(true);
    }

    public void OptionOut()
    {
        OptionScene.SetActive(false);
    }

    public void Go()
    {
        SceneManager.LoadScene("2_InGameScene");
    }
}
