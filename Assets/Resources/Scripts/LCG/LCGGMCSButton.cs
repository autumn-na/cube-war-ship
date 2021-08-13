using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LCGGMCSButton : MonoBehaviour {

    enum eMode { PVP, Campain, Wait };

    eMode ModeChose = eMode.Wait;
    public void PVPBt()
    {
        ModeChose = eMode.PVP;
        Debug.Log(ModeChose);
    }
    public void CampainBt()
    {
        ModeChose = eMode.Campain;
        Debug.Log(ModeChose);
    }
    public void BackBt()
    {
        ModeChose = eMode.Wait;
        Debug.Log("Secne Change : 0_MainScene");
        SceneManager.LoadScene("0_MainScene");
        
    }
}
