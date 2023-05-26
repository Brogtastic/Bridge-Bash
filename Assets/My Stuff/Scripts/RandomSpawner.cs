using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static class GlobalSpawn
{
    public static int thisSpawnPoint;

    public static int increaseEnemyMoveSpeed;
    public static int increaseEnemyDamage;
    public static int increaseEnemyHealth;
    public static int increaseEnemySize;
}

public class RandomSpawner : MonoBehaviour
{

    public Transform[] spawnPoints;
    public Transform[] spawnPoints2;
    public Transform[] spawnPoints3;
    public GameObject[] enemyPrefabs;
    public GameObject[] enemyPrefabs2;
    public GameObject[] testingPrefabs;

    private int phase;
    private float startingInterval;

    [SerializeField] private float interval;
    float time;
    float incrementTime;

    // Start is called before the first frame update
    void Start()
    {
        phase = 1;
        startingInterval = interval;
        time = 0f;
        incrementTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Global.currentHealth > 0)
        {
            time += Time.deltaTime;

            //a
            if ((time > interval) && (phase == 1))
            {
                int randEnemy = Random.Range(0, enemyPrefabs.Length);
                int randSpawnPoint = Random.Range(0, spawnPoints.Length);

                GlobalSpawn.thisSpawnPoint = randSpawnPoint;

                Instantiate(enemyPrefabs[randEnemy], spawnPoints[randSpawnPoint].position, transform.rotation);

                time = 0f;

            }
            // phase 2
            if ((time > interval) && (phase == 2))
            {
                int randEnemy = Random.Range(0, enemyPrefabs2.Length);
                int randSpawnPoint = Random.Range(0, spawnPoints.Length);

                GlobalSpawn.thisSpawnPoint = randSpawnPoint;

                Instantiate(enemyPrefabs2[randEnemy], spawnPoints[randSpawnPoint].position, transform.rotation);

                time = 0f;

            }
            //phase 3
            if ((time > interval) && (phase == 3))
            {
                int randEnemy = Random.Range(0, testingPrefabs.Length);
                int randSpawnPoint;
                int randChance = Random.Range(0, 100);
                if ((randEnemy == 0) && (Global.playerCoordinatesX >= 0)) // if it's the hover enemy and player's on the right
                {
                    randSpawnPoint = Random.Range(6, spawnPoints2.Length-2); // spawn on the left side
                }
                else if ((randEnemy == 0) && (Global.playerCoordinatesX < 0)) // if it's the hover enemy and player's on the left
                {
                    randSpawnPoint = Random.Range(8, spawnPoints2.Length); // spawn on the right side
                }
                else
                {
                    if (randChance < 10)
                    {
                        randSpawnPoint = Random.Range(0, spawnPoints2.Length); // spawn wherever (10% chance)
                    }
                    else
                    {
                        randSpawnPoint = Random.Range(0, spawnPoints2.Length-4); // spawn from top (90% chance)
                    }
                }

                GlobalSpawn.thisSpawnPoint = randSpawnPoint;

                Instantiate(testingPrefabs[randEnemy], spawnPoints2[randSpawnPoint].position, transform.rotation);

                time = 0f;

            }

            incrementTime += Time.deltaTime;
            //phase 1
            if ((incrementTime > interval * 3) && (interval > 1.5f) && (phase == 1))
            {
                interval -= 0.1f;
                incrementTime = 0f;
                Debug.Log("interval is " + interval);
                if (interval <= 1.5f)
                {
                    phase += 1;
                    interval = 2.5f;
                }

            }
            //phase 2
            else if ((incrementTime > interval * 3) && (interval > 1.7f) && (phase == 2))
            {
                Debug.Log("Phase 2 babyyyyy");
                interval -= 0.1f;
                Debug.Log("interval is " + interval);
                incrementTime = 0f;
                if (interval <= 1.7f)
                {
                    phase += 1;
                    interval = 2.5f;
                }
            }
            //phase 3
            else if ((incrementTime > interval * 3) && (interval > 1.5f) && (phase == 3))
            {
                interval -= 0.1f;
                Debug.Log("interval is " + interval);
                incrementTime = 0f;
            }
            //ultra phase 3
            else if ((incrementTime > interval * 3) && (interval <= 1.5f) && (phase == 3))
            {
                interval -= 0.01f;
                Debug.Log("ultra phase 3 interval is " + interval);
                incrementTime = 0f;
            }
        }
    }
}
