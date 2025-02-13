using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatBackground : MonoBehaviour
{
    private Vector3 startPos;
    private float repeatWidth;

    [SerializeField]
    PlayerController player;

    private void Start()
    {
        startPos = transform.position;
        repeatWidth = 64;
    }

    private void Update()
    {
        float direction = player.GetPlayerHorizontal();
        Vector3 playerDir = player.GetPlayerMovement();
        if (true)
        {
            if (transform.position.z < startPos.z - repeatWidth)
            {
                Debug.Log("배경 원위치");
                transform.position = startPos;
            }
            else
            {
                Debug.Log("배경 이동중'");
                //transform.Translaste()
                //transform.Translate(new Vector3(0f, 0f, -1) * Time.deltaTime);
            }
        }
    }
}
