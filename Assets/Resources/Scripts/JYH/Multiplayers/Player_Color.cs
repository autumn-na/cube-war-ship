using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player_Color : NetworkBehaviour {
    [SyncVar]
    public Color color;
    MeshRenderer[] rends;

	void Start () {
        rends = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.color = color;
        }
    }
	
	void Update () {
		
	}

    public void HidePlayer()
    {
        for (int i = 0; i < rends.Length; i++)
        {
            rends[i].material.color = Color.clear;
        }
    }
}
