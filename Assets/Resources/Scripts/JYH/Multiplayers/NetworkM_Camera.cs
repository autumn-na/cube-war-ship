using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkM_Camera : NetworkManager {

    [SerializeField] Transform sceneCamera;
    [SerializeField] float cameraRotationRadius = 6f;
    [SerializeField] float cameraRotationSpeed = 3f;
    [SerializeField] bool canRotate = true;

    float rotation;

	void Start () {
		
	}

    public override void OnStartClient(NetworkClient client)
    {
        canRotate = false;
    }

    public override void OnStartHost()
    {
        canRotate = false;
    }

    public override void OnStopClient()
    {
        canRotate = true;
    }

    public override void OnStopHost()
    {
        canRotate = true;
    }

    void Update () {
        if (!canRotate)
            return;

        rotation += cameraRotationSpeed * Time.deltaTime;
        if (rotation >= 360f)
            rotation -= 360f;

        sceneCamera.position = Vector3.zero;
        sceneCamera.rotation = Quaternion.Euler(0f, rotation, 0f);
        sceneCamera.Translate(0f, cameraRotationRadius, -cameraRotationRadius);
        sceneCamera.LookAt(Vector3.zero);
	}
}
