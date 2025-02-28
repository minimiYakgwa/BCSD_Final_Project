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
    private GameObject barricade;
    [SerializeField]
    private Transform spawnPointRotation;

    [SerializeField]
    private float maxVSpawnDelay;
    [SerializeField]
    private float maxBSpawnDelay;
    [SerializeField]
    private float curVSpawnDelay;
    [SerializeField]
    private float curBSpawnDelay;

    private void Start()
    {
        maxVSpawnDelay = 10 / GameManager.instance.level;
        maxBSpawnDelay = 6 / GameManager.instance.level;
    }
    private void Update()
    {
        if (!GameManager.instance.isStart)
            return;
        curVSpawnDelay += Time.deltaTime;
        curBSpawnDelay += Time.deltaTime;
        if (curVSpawnDelay > maxVSpawnDelay)
        {
            SpawnVehicles();
            curVSpawnDelay = 0;
        }
        if (curBSpawnDelay > maxBSpawnDelay)
        {
            SpawnBarricade();
            curBSpawnDelay = 0;
        }
    }
    private void SpawnBarricade()
    {
        Quaternion rotation = spawnPointRotation.rotation * Quaternion.Euler(0, 180, 0);

        int ranPoint = Random.Range(0, spawnPoints.Length);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i != ranPoint)
            {
                Instantiate(barricade, spawnPoints[i].position, spawnPoints[i].rotation);
            }



        }

    }

    private void SpawnVehicles()
    {
        Quaternion rotation = spawnPointRotation.rotation * Quaternion.Euler(0, 180, 0);

        int ranPoint = Random.Range(0, spawnPoints.Length);
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            if (i != ranPoint)
            {
                int randomVehicle = Random.Range(1, vehicles.Length);

                Instantiate(vehicles[randomVehicle], spawnPoints[i].position, spawnPoints[i].rotation);
            } 
        }
        SoundManager.instance.StopSE("VehicleSound");
        SoundManager.instance.PlaySE("VehicleSound");
        
    }
}
