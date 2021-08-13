using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour {

    [SerializeField] float cameraDistance = 16f;
    [SerializeField] float cameraHeight = 16f;

    
    Transform mainCamera;
    Vector3 cameraOffset;
	// Use this for initialization
	void Start () {
        cameraOffset = new Vector3(0f, cameraHeight, -cameraDistance);

        mainCamera = Camera.main.transform;
        MoveCamera();
	}
	
	// Update is called once per frame
	void Update () {

        MoveCamera();
	}

    void MoveCamera()
    {
        mainCamera.position = transform.position;
        mainCamera.rotation = transform.rotation;
        mainCamera.Translate(cameraOffset);
        mainCamera.LookAt(transform);
    }
}
