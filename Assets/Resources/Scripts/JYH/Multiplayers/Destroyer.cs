using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
// 3x1 구축함

public class Destroyer : NetworkBehaviour
{ 
    public GameObject objShotEffect;
    public GameObject objEmpShotEffect;
    public GameObject objFireEffect;
    public GameObject objBombEffect;

    public int nX;
    public int nY;

    int netchange = 0;
    int nempty = 0;
    int x, y;
    int EmpEnemy = 0, EmpEnemy2 = 0;

    void Start()
    {

    }

    public override void OnStartClient()
    {
        nempty = 1;
        Debug.Log(nempty);
    }

    public override void OnStartServer()
    {

        base.OnStartServer();
    }

    public override void OnStartLocalPlayer()
    {
        nempty = 0;
        Debug.Log(nempty);
    }

    void Update()
    {
        //if (!isLocalPlayer) // 자신것이 아니면 못움직이게
          //  return;

        if (nempty == 1)
        {
            if (netchange == 1)
            {
                if (Input.GetKeyDown(KeyCode.A))
                {
                    x = nX;
                    y = nY;
                    StartCoroutine("Shot");
                }
                if (Input.GetKeyDown(KeyCode.S))
                {
                    EmpEnemy2 = 1;
                }
                if (Input.GetKeyDown(KeyCode.D))
                {
                    EmpEnemy2 = 0;
                }
            }
        }
        else if (nempty == 0)
        {
            if (netchange == 0)
            {
                if (Input.GetKeyDown(KeyCode.Q))
                {
                    x = nX;
                    y = nY;
                    StartCoroutine("Shot");
                }
                if (Input.GetKeyDown(KeyCode.W))
                {
                    EmpEnemy = 1;
                }
                if (Input.GetKeyDown(KeyCode.E))
                {
                    EmpEnemy = 0;
                }
            }
        }
    }

    IEnumerator Shot()
    {
        Debug.Log("Shot!");
        objEmpShotEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        objEmpShotEffect.SetActive(false);
        Instantiate(objBombEffect, new Vector3(x, 1, y), Quaternion.Euler(90, 0, 0));
        if (EmpEnemy == 1 /*위치에 상대방이 있으면*/ )
        {
            Instantiate(objFireEffect, new Vector3(x, 1, y), Quaternion.Euler(-90, 0, 0));
        }

    }

    IEnumerator Shot2()
    {
        Debug.Log("Shot2!");
        objEmpShotEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        objEmpShotEffect.SetActive(false);
        Instantiate(objBombEffect, new Vector3(x, 1, y), Quaternion.Euler(90, 0, 0));
        if (EmpEnemy2 == 1 /*위치에 상대방이 있으면*/ )
        {
            Instantiate(objFireEffect, new Vector3(x, 1, y), Quaternion.Euler(-90, 0, 0));
        }

    }

    public void Buttonreturn()
    {
        if (netchange == 0)
        {
            netchange = 1;
        }
        else if (netchange == 1)
        {
            netchange = 0;
        }
    }
}
