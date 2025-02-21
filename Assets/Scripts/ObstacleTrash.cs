using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTrash : Obstacle
{
    private void Update()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(0, -1f, 0);
    }
}
