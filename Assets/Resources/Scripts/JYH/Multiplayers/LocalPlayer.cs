using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;


public class LocalPlayer : NetworkBehaviour
{
    [SerializeField] GameObject objShotEffect;
    [SerializeField] GameObject objEmpShotEffect;
    [SerializeField] GameObject objFireEffect;
    [SerializeField] GameObject objBombEffect;

    Text WinandLoseText;

    public int nX = 0;
    public int nY = 0;
    
    public int EmpEnemy = 0;
    
    void Start()
    {

    }

    void Update()
    {
        //if(체력 <= 0) // 체력이 다떨어지면
        //{
        //    RpcDied();
        //}

        if (!isLocalPlayer) // 자신것이 아니면 못움직이게
            return;
        //if (NetEmptys.instance.netchange == 1)
        //{
        if (Input.GetKeyDown(KeyCode.A))
        {
            CmdShot2();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            EmpEnemy = 1;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            EmpEnemy = 0;
        }
        //}
        if (Input.GetKeyDown(KeyCode.F))
        {
            RpcLOSE();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            RpcWIN();
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            BackToLobby();
        }
    }

    [Command]
    void CmdShot2()
    {
        GameObject Bombobj = Instantiate(objBombEffect, new Vector3(nX, 1, nY), Quaternion.Euler(90, 0, 0)) as GameObject;
        NetworkServer.Spawn(Bombobj);
        //if (EmpEnemy == 1 /*위치에 상대방이 있으면*/ )
        //{
        //    GameObject Fireobj = Instantiate(objFireEffect, new Vector3(nX, 1, nY), Quaternion.Euler(-90, 0, 0)) as GameObject;
        //    NetworkServer.Spawn(Fireobj);
        //}
    }

    [ClientRpc]
    void RpcLOSE()
    {
        GetComponent<Player_Color>().HidePlayer();
        if(isLocalPlayer)
        {
            //졋을때
            WinandLoseText = GameObject.FindObjectOfType<Text>();
            WinandLoseText.text = "LOSE";
            //GetComponent<LocalPlayer>().enabled = false;
        }
    }

    [ClientRpc]
    void RpcWIN()
    {
        if(isLocalPlayer)
        {
            WinandLoseText = GameObject.FindObjectOfType<Text>();
            WinandLoseText.text = "WIN";
        }
    }

    void BackToLobby() // 로비로 돌아가기
    {
        FindObjectOfType<NetworkLobbyManager>().ServerReturnToLobby();
    }
}
