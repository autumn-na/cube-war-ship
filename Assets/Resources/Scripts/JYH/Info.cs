using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour {

    public GameObject Cruiser;
    public GameObject Destroyer;
    public GameObject BattleShip;
    public GameObject FastShip;
    public GameObject PatrolShip;
    public GameObject Submarine;

    int Ship = 0;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void R()
    {
        if(Ship == 5)
        {
            Ship = 0;
        }
        else
        {
            Ship++;
        }
        NextShip();
    }

    public void L()
    {
        if (Ship == 0)
        {
            Ship = 5;
        }
        else
        {
            Ship--;
        }
        NextShip();
    }

    void NextShip()
    {
        if(Ship == 0)
        {
            Submarine.SetActive(false);
            Cruiser.SetActive(true);
            Destroyer.SetActive(false);
        }
        else if(Ship == 1)
        {
            Cruiser.SetActive(false);
            Destroyer.SetActive(true);
            BattleShip.SetActive(false);
        }
        else if(Ship == 2)
        {
            Destroyer.SetActive(false);
            BattleShip.SetActive(true);
            FastShip.SetActive(false);
        }
        else if(Ship == 3)
        {
            BattleShip.SetActive(false);
            FastShip.SetActive(true);
            PatrolShip.SetActive(false);
        }
        else if(Ship == 4)
        {
            FastShip.SetActive(false);
            PatrolShip.SetActive(true);
            Submarine.SetActive(false);
        }
        else if(Ship == 5)
        {
            PatrolShip.SetActive(false);
            Submarine.SetActive(true);
            Cruiser.SetActive(false);
        }
    }
}
