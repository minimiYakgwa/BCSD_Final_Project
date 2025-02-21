using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    protected float speed = 7;

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("ObstacleEndPoint"))
            StartCoroutine(DestroyObstacle());
    }

    protected IEnumerator DestroyObstacle()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }

}
