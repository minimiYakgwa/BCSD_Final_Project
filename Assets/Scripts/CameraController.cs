using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    PlayerController player;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate()
    {
        transform.position = player.transform.position;
    }


    public IEnumerator IsTurnCamera(Collider other)
    {
        Quaternion targetRot = transform.rotation * Quaternion.Euler(0f, 90f, 0f);
            

        while (Quaternion.Angle(transform.rotation, targetRot) > 0.1f)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, 0.1f);
            yield return null;
        }
        transform.rotation = targetRot;
    }
}
