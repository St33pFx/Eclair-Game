using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner_2 : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;
        public List<EnemyGroup> enemyGroups;
        public int waveQuota;
        public float spawnInterval;
        public int spawnCount;
    }

    [System.Serializable]
    public class EnemyGroup
    {
        public string enemyName;
        public int enemyCount;
        public int spawnCount;
        public GameObject enemyPrefab;
    }

    public List<Wave> waves;
    public int waveActualNumero;

    [Header("Spawner Attributes")]
    float spawnTimer;
    public int enemiesVivos;
    public int enemigosPermitidosMaximos;
    public bool enemigosMaximosCompletado = false;
    public float waveInterval;
    public bool puedeSpawn = true;

    public LayerMask layerObstaculo;
    public float radioChequeo = 0.5f;

    [Header("Spawn Position")]
    public List<Transform> relativeSpawnPoints;

    
    Transform player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        CalcularWaveCuota();
    }

    // Update is called once per frame
    void Update()
    {
        if (waveActualNumero < waves.Count && waves[waveActualNumero].spawnCount == 0)
        {
            StartCoroutine(IniciarSiguienteWave());
        }
                


        spawnTimer += Time.deltaTime;

        if (spawnTimer >= waves[waveActualNumero].spawnInterval)
        {
            spawnTimer = 0f;
            SpawnEnemigos();

        }
    }

    IEnumerator IniciarSiguienteWave()
    {
        yield return new WaitForSeconds(waveInterval);

        if(waveActualNumero < waves.Count -1)
        {
            waveActualNumero++;
            CalcularWaveCuota();
        }
    }

    void CalcularWaveCuota()
    {
        int currentWaveCuota = 0;

        foreach (var enemyGroup in waves[waveActualNumero].enemyGroups)
        {
            currentWaveCuota += enemyGroup.enemyCount;
        }

        waves[waveActualNumero].waveQuota = currentWaveCuota;
        Debug.LogWarning(currentWaveCuota);
         
    }

    void SpawnEnemigos()
    {
        if (waves[waveActualNumero].spawnCount < waves[waveActualNumero].waveQuota && !enemigosMaximosCompletado)
        {
            foreach(var enemyGroup in waves[waveActualNumero].enemyGroups)
            {
                if (enemyGroup.spawnCount < enemyGroup.enemyCount)
                {
                    if (enemiesVivos >= enemigosPermitidosMaximos)
                    {
                        enemigosMaximosCompletado = true;
                        return;
                    }

                    bool spawnHecho = false;
                    int intentos = 0;
                    int maxIntentos = 10;


                    //Instantiate(enemyGroup.enemyPrefab, player.position + relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)].position, Quaternion.identity);

                    //var pt = relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)];
                    //Instantiate(enemyGroup.enemyPrefab, pt.position, Quaternion.identity);

                    //enemyGroup.spawnCount++;
                    //waves[waveActualNumero].spawnCount++;
                    //enemiesVivos++;

                    while (!spawnHecho && intentos < maxIntentos)
                    {
                        var pt = relativeSpawnPoints[Random.Range(0, relativeSpawnPoints.Count)];
                        Vector2 posicion = pt.position;

                        // Debug visual (opcional)
                        Debug.DrawRay(posicion, Vector2.up * radioChequeo, Color.red, 1f);
                        Debug.DrawRay(posicion, Vector2.down * radioChequeo, Color.red, 1f);
                        Debug.DrawRay(posicion, Vector2.left * radioChequeo, Color.red, 1f);
                        Debug.DrawRay(posicion, Vector2.right * radioChequeo, Color.red, 1f);

                        // Checar si hay un collider con la layer Obstaculo en esa zona
                        Collider2D colision = Physics2D.OverlapCircle(posicion, radioChequeo, layerObstaculo);

                        // Si no hay colisión con obstáculos, podemos spawnear
                        if (colision == null)
                        {
                            Instantiate(enemyGroup.enemyPrefab, posicion, Quaternion.identity);
                            enemyGroup.spawnCount++;
                            waves[waveActualNumero].spawnCount++;
                            enemiesVivos++;
                            spawnHecho = true;
                        }
                        else
                        {
                            intentos++;
                        }
                    }
                }
            }
        }

        if(enemiesVivos < enemigosPermitidosMaximos)
        {
            enemigosMaximosCompletado = false;
        }
    }

    public void OnEnemyKilled()
    {
        enemiesVivos --;
    }

}
