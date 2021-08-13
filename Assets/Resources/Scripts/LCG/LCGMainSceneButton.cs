using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LCGMainSceneButton : MonoBehaviour {

    public void Store()
    {
        Debug.Log("StoreScene Open");
        SceneManager.LoadScene("3_StoreScene");
    }
    public void GameStart()
    {
        Debug.Log("Secne Change : 1_GameModeChoseScene");
        SceneManager.LoadScene("1_GameModeChoseScene");
    }
    public void Option(GameObject option)
    {
        option.SetActive(!option.activeInHierarchy);
        Debug.Log("OptionScene" + option);
    }
    public void Help()
    {
        Debug.Log("HelpScene Open");
    }
}
