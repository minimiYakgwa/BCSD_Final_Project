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
    private Transform spawnPointRotation;

    [SerializeField]
    private float maxSpawnDelay;
    [SerializeField]
    private float curSpawnDelay;

    private void Update()
    {
        if (!GameManager.instance.isStart)
            return;
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > maxSpawnDelay)
        {
            SpawnVehicles();
            curSpawnDelay = 0;
        }
    }

    private void SpawnVehicles()
    {
        Quaternion rotation = spawnPointRotation.rotation * Quaternion.Euler(0, 180, 0);

        int ranPoint = Random.Range(0, spawnPoints.Length);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i == ranPoint)
            {
                Debug.Log("쓰레기통 생성!!");
                Instantiate(vehicles[spawnPoints.Length-1], spawnPoints[i].position, spawnPoints[i].rotation);
            }
            else
            {
                int randomVehicle = Random.Range(1, vehicles.Length);

                Instantiate(vehicles[randomVehicle], spawnPoints[i].position, spawnPoints[i].rotation);
            }
                

            
        }
        
    }
}
