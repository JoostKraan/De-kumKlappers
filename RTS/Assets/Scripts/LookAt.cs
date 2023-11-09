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

        target = cam.transform;
    }

    void Update()
    {
        transform.LookAt(target);
    }
}

