using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    private Transform target;
    public Camera cam;

    void Start()
    {
        cam= Camera.main;
        // Assuming the camera is tagged as "MainCamera"
        target = cam.transform;
    }

    void Update()
    {
        // Make the plane face the camera
        transform.LookAt(target);
    }
}

