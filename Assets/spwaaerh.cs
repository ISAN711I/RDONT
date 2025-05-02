using UnityEngine;
using Unity.Cinemachine;

public class swaaerh : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    public GameObject enemyPrefab1;
    public GameObject enemyPrefab2;
    public GameObject enemyPrefab3;
    public GameObject Boss;

    [Header("Spawn Timing")]
    public float timeBetweenSpawns = 0.5f;

    [Header("Wave Settings")]
    public int startingEnemiesPerWave = 3;
    public float difficultyGrowth = 1.5f;

    [Header("Enemy Spawn Chances")]
    public float enemy2BaseChance = 0.1f;
    public float enemy2ChanceGrowth = 0.05f;
    public float enemy3BaseChance = 0.05f;
    public float enemy3ChanceGrowth = 0.03f;

    public static int enemiesAlive = 0;

    private int currentWave = 0;
    private int enemiesToSpawn;
    private float spawnTimer = 0;
    private float waveRotationTime = 0;

    private bool waveInProgress = false;

    public CinemachineCamera virtualCamera;

        private Transform player;

    void Start() {
         GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }
    void Update()
    {
        // Orbiting position logic
        waveRotationTime += Time.deltaTime;
        transform.position = new Vector3(15 * Mathf.Cos(waveRotationTime), 15 * Mathf.Sin(waveRotationTime));

        // Start new wave when no enemies remain and no wave is in progress
        if (!waveInProgress && enemiesAlive == 0)
        {
            StartNewWave();
        }

        // Spawn enemies in current wave
        if (waveInProgress && currentWave != 6)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0 && enemiesToSpawn > 0)
            {
                SpawnRandomEnemy();
                enemiesToSpawn--;
                spawnTimer = timeBetweenSpawns;
            }

            if (enemiesToSpawn <= 0)
            {
                waveInProgress = false;
            }
        }
    }

    void StartNewWave()
    {
        if(currentWave != 5) {
        currentWave++;
        enemiesToSpawn = Mathf.RoundToInt(startingEnemiesPerWave * Mathf.Pow(difficultyGrowth, currentWave - 1));
        waveInProgress = true;
        spawnTimer = 0;

        // Call the scrolling banner to show the wave
        Scrolling scroller = FindObjectOfType<Scrolling>();
        if (scroller != null)
        {
            scroller.DisplayWaveMessage(currentWave);
        }

        Debug.Log($"Wave {currentWave} started! Spawning {enemiesToSpawn} enemies.");
        }
        else {
            currentWave = 6;
             enemiesToSpawn = 9999999;
           GameObject bossInstance = Instantiate(Boss, new Vector3(0, 0, 0), Quaternion.identity);
virtualCamera.Follow = bossInstance.transform;
Invoke("LookAtPLayer",1f);
waveInProgress = true;
        }
    }

    void SpawnRandomEnemy()
    {
        float chance2 = Mathf.Clamp01(enemy2BaseChance + (enemy2ChanceGrowth * currentWave));
        float chance3 = Mathf.Clamp01(enemy3BaseChance + (enemy3ChanceGrowth * currentWave));
        float rand = Random.value;

        GameObject enemyToSpawn;

        if (rand < chance3)
        {
            enemyToSpawn = enemyPrefab3;
        }
        else if (rand < chance3 + chance2)
        {
            enemyToSpawn = enemyPrefab2;
        }
        else
        {
            enemyToSpawn = enemyPrefab1;
        }

        Instantiate(enemyToSpawn, transform.position, Quaternion.identity);
        enemiesAlive++;
    }
    void LookAtPLayer(){
        virtualCamera.Follow = player;
    }
}
