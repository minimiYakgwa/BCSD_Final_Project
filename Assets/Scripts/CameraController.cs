using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    PlayerController player;

    [SerializeField]
    private GameObject cameraLocation;

    [SerializeField]
    private float lookSensivity;
    [SerializeField]
    private float cameraRotationLimit;
    private float currentCameraRotationX = 0f;
    private float currentCameraRotationY = 0f;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position;
    }
}
