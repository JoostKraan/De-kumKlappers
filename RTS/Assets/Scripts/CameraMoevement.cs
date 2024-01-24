 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoevement : MonoBehaviour
{
    public Camera camera;
    
    public float zoomSpeed = 5.0f;
    public float minZoomDistance = 1.0f;
    public float maxZoomDistance = 10.0f;

    public float moveSpeed = 10f;
    public float minFOV = 20.0f;
    public float maxFOV = 50.0f;
    public float smoothTime = 0.2f;
    public float zoomSensitivity = 1.0f;

    private float targetFOV;
    private float zoomVelocity;

    void Start()
    {
        targetFOV = camera.fieldOfView;
    }

    void Update()
    {
        float scrollWheelInput = Input.GetAxis("Mouse ScrollWheel");
        targetFOV -= scrollWheelInput * zoomSpeed * zoomSensitivity;

        // Clamp the target FOV to min and max values
        targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);

        // Smoothly interpolate the current FOV to the target FOV
        Camera.main.fieldOfView = Mathf.SmoothDamp(camera.fieldOfView, targetFOV, ref zoomVelocity, smoothTime);

        // Moving
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput);

        // Normalize the move direction to avoid faster diagonal movement
        if (moveDirection.magnitude > 1)
        {
            moveDirection.Normalize();
        }

        // Move the camera
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
    }
}
