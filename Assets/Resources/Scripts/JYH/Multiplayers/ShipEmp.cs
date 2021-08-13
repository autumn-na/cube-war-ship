using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ShipEmp : NetworkBehaviour {

    public GameObject objShotEffect;
    public GameObject objEmpShotEffect;
    public GameObject objFireEffect;
    public GameObject objBombEffect;

    public int nX;
    public int nY;
    public int nPosnum;

    private int x = 0, y = 0;
    private int EmpEnemy = 0;

    void Start()
    {

    }

    //public override void OnStartClient()
    //{
    //    nempty = 1;
    //    Debug.Log(nempty);
    //}

    //public override void OnStartLocalPlayer()
    //{
    //    nempty = 0;
    //    Debug.Log(nempty);
    //}

    void Update()
    {
        if (NetEmptys.instance.netchange == 1)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                x = nX;
                y = nY;
                StartCoroutine("Shot");
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                EmpEnemy = 1;
            }
            if (Input.GetKeyDown(KeyCode.D))
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
        Instantiate(objBombEffect, new Vector3(x, 1, y), Quaternion.Euler(90, 0, 0));
        if (EmpEnemy == 1 /*위치에 상대방이 있으면*/ )
        {
            /*objBombEffect.transform.rotation*/
            Instantiate(objFireEffect, new Vector3(x, 1, y), Quaternion.Euler(-90, 0, 0));
        }

        //objEmpty[0] = Instantiate(objShotEffect, transform.position, Quaternion.Euler(90, 0, 0)) as GameObject;
        //Destroy(objEmpty[0], 0.25f);
    }
}
