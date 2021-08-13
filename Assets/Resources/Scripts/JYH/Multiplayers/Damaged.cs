using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Damaged : NetworkBehaviour {
    [SerializeField] float LifeTime = 0.2f;
    float age;
	// Use this for initialization
    //[ServerCallback]
	void Start () {
        //Destroy(gameObject, 0.15f);
	}
	
	// Update is called once per frame
    [Server]
	void Update () {
        age += Time.deltaTime;
        if (age > LifeTime)
            Destroy(gameObject);
	}

    //void set()
    //{
        //gameObject.SetActive(false);
    //}
}
