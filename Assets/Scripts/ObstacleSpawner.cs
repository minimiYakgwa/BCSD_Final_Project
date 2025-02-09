using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject[] vehicles;
    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private float maxSpawnDelay;
    [SerializeField]
    private float curSpawnDelay;

    private void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > maxSpawnDelay)
        {
            SpawnVehicles();
            curSpawnDelay = 0;
        }
    }

    private void SpawnVehicles()
    {
        int spawnCount = Random.Range(1, 4);
        Quaternion rotation = Quaternion.Euler(0, 180, 0);

        for (int i = 0; i < spawnCount; i++)
        {
            int randomVehicle = Random.Range(1, vehicles.Length);
            int ranPoint = Random.Range(0, spawnPoints.Length);

            Instantiate(vehicles[randomVehicle], spawnPoints[ranPoint].position, rotation);
        }
        
    }
}
