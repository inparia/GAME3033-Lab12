
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour
{

    [SerializeField]
    private int NumberOfZombiesToSpawn;

    [SerializeField]
    private GameObject[] ZombiePrefab;

    [SerializeField]
    private SpawnerVolume[] SpawnVolume;

    private GameObject FollowGameObject;

    // Start is called before the first frame update
    void Start()
    {
        FollowGameObject = GameObject.FindGameObjectWithTag("Player");
        for (int index = 0; index < NumberOfZombiesToSpawn; index++)
        {
            SpawnZombie();
        }
    }

    private void SpawnZombie()
    {
        GameObject ZombieToSpawn = ZombiePrefab[Random.Range(0, ZombiePrefab.Length)];
        SpawnerVolume spawnVolume = SpawnVolume[Random.Range(0, SpawnVolume.Length)];

        if (!FollowGameObject) return;

        GameObject zombie = Instantiate(ZombieToSpawn, spawnVolume.GetPositionInBounds(), spawnVolume.transform.rotation);


        zombie.GetComponent<ZombieComponent>().Initialize(FollowGameObject);


    }
    
}
