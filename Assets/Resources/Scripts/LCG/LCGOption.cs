using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LCGOption : MonoBehaviour {

	public void SFX(Toggle sfx)
    {
        Debug.Log(sfx.isOn);
    }
    public void BGM(Toggle bgm)
    {
        Debug.Log(bgm.isOn);
    }
    public void Credit()
    {
        Debug.Log("Credit open");
    }
    public void Surrender()
    {
        Debug.Log("you are surrender! you loss~");
    }
}
