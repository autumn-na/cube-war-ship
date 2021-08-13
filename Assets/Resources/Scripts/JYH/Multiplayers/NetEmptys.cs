using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public sealed class NetEmptys : NetworkBehaviour
{
    public static NetEmptys instance;
    public int nempty = 0;
    public int nempty2 = 0;
    public void Awake()
    {
        NetEmptys.instance = this;
    }
    public int Naminheong = 0;
    public int netchange = 0;

    void Start () {

    }
	
	void Update () {

	}
    
    public void Buttonreturn()
    {
        if(netchange == 0)
        {
            netchange = 1;
        }
        else if(netchange == 1)
        {
            netchange = 0;
        }
    }
}
