using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurnChange : NetworkBehaviour
{
    int nCha;
    public int netchange = 0;
    void Start () {
        
    }
    
    //자신과 다른유닛 구분하는 방법 (카메라에서)
    public override void OnStartClient()
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
        base.OnStartClient();
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
        base.OnStartLocalPlayer();
    }

    [ClientCallback]
    void Update () {
        
        if (isServer) // 호스트면 이곳
        {
            
        }
        else// if (isClient) // 호스트가 아니면 이곳
            

        if (!isLocalPlayer) // 자신것이 아니면 못움직이게
            return;
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
