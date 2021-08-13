using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HostPlayer : NetworkBehaviour
{
    public GameObject objShotEffect;
    public GameObject objEmpShotEffect;
    public GameObject objFireEffect;
    public GameObject objBombEffect;

    public int nX;
    public int nY;
    
    int EmpEnemy = 0;

    void Start()
    {

    }

    public override void OnStartServer()
    {
        NetEmptys.instance.Naminheong++;
        base.OnStartServer();
    }

    void Update()
    {
        //if (!isLocalPlayer) // 자신것이 아니면 못움직이게
        //   return;
        if (NetEmptys.instance.netchange == 0)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
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

    IEnumerator Shot()
    {
        Debug.Log("Shot!");
        objEmpShotEffect.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        objEmpShotEffect.SetActive(false);
        Instantiate(objBombEffect, new Vector3(nX, 1, nY), Quaternion.Euler(90, 0, 0));
        if (EmpEnemy == 1 /*위치에 상대방이 있으면*/ )
        {
            Instantiate(objFireEffect, new Vector3(nX, 1, nY), Quaternion.Euler(90, 0, 0));
        }

    }
}
