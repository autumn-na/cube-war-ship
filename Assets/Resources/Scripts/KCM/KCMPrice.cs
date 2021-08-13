using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class KCMPrice : MonoBehaviour {
    public int Nmoney = 10000;
    public Text Price;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Nmoney += 10000;
            Price.text = Nmoney.ToString();
        }
	}
    public void Minus()
    {
        Nmoney -= 1000;
        Price.text = Nmoney.ToString();
    }

    public void Back()
    {
        Debug.Log("Secne Change : 0_MainScene");
        SceneManager.LoadScene("0_MainScene");
    }
}
