using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoScene : MonoBehaviour
{
    public GameObject ShopScene;
    public GameObject OptionScene;
    public GameObject InfoScene;
    public GameObject LoScene;

    int sta = 0; // 0 = Logo , 1 = Shop , 2 = Info

    // Use this for initialization
    void Start()
    {
        NMHSoundMng.GetInstance().RunBGM(NMHSoundMng.BGMList.MAIN);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void sStart()
    {
        SceneManager.LoadScene("1_HowToPlayScene");
    }

    public void Shop()
    {
        sta = 1;
        ShopScene.SetActive(true);
        LoScene.SetActive(false);
    }

    public void Info()
    {
        sta = 2;
        InfoScene.SetActive(true);
        LoScene.SetActive(false);
    }

    public void Option()
    {
        OptionScene.SetActive(true);
    }

    public void Back()
    {
        if (sta == 0)
        {

        }
        else if(sta == 1)
        {
            ShopScene.SetActive(false);
        }
        else if(sta == 2)
        {
            InfoScene.SetActive(false);
        }
        sta = 0;
        LoScene.SetActive(true);
    }
}
