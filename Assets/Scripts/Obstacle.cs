using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float speed = 7;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ObstacleEndPoint"))
            StartCoroutine(DestroyObstacle());
    }

    private IEnumerator DestroyObstacle()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
