using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float speed = 7;

    private void Update()
    {
        transform.Translate(transform.forward * Time.deltaTime * speed * -1);

        if (transform.position.z <= -11)
            Destroy(gameObject);
    }
}
