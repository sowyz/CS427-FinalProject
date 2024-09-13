using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public int enemiesPerWave = 5;
    public int currentEnemiesPerWave;
    public float timeBetweenWaves = 10f;
    public int currentWave = 1;
    public float timeBetweenEnemies = 0.5f;
    public GameObject enemyPrefab;
    public bool isCoolingDown = false;
    public float coolDownTimer = 0f;
    private List<Enemy> enemiesAlive;
    public float spawnRadius = 5f;
    private List<Transform> wayPoints = new List<Transform>();

    private void Awake()
    {
        GameObject waypointCluster = GameObject.FindGameObjectWithTag("Waypoint");
        foreach(Transform child in waypointCluster.transform)
        {
            wayPoints.Add(child);
        }
    }
    private void Start()
    {
        currentEnemiesPerWave = enemiesPerWave;
        StartNewWave();
    }

    private void StartNewWave()
    {
        enemiesAlive = new List<Enemy>();
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        for (int i = 0; i < currentEnemiesPerWave; i++)
        {
            // Enemy spawn position
            Vector3 randomPosition = new Vector3(UnityEngine.Random.Range(-spawnRadius, spawnRadius), 0, UnityEngine.Random.Range(-spawnRadius, spawnRadius));
            Vector3 spawnPosition = wayPoints[UnityEngine.Random.Range(0, wayPoints.Count)].position + randomPosition;
            // New enemy
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            // Add enemy to track list
            enemiesAlive.Add(enemy.GetComponent<Enemy>());
            // Wait before spawning next enemy
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    private void Update()
    {
        // Get dead enemies
        List<Enemy> deadEnemies = new List<Enemy>();
        foreach (Enemy enemy in enemiesAlive)
        {
            if (enemy.isDead)
            {
                deadEnemies.Add(enemy);
            }
        }

        // Remove dead enemies from list
        foreach (Enemy deadEnemy in deadEnemies)
        {
            enemiesAlive.Remove(deadEnemy);
        }
        deadEnemies.Clear();

        // Check if all enemies are dead
        if (enemiesAlive.Count == 0 && !isCoolingDown)
        {
            StartCoroutine(WaveCoolDown());
        }

        if (isCoolingDown)
        {
            coolDownTimer -= Time.deltaTime;
        }
        else
        {
            coolDownTimer = timeBetweenWaves;
        }
    }

    private IEnumerator WaveCoolDown()
    {
        isCoolingDown = true;
        yield return new WaitForSeconds(timeBetweenWaves);
        isCoolingDown = false;
        currentEnemiesPerWave += currentWave;
        currentWave++;
        StartNewWave();
    }
}
