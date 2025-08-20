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

    public LayerMask layerPermitidaParaSpawn;
    public float radioChequeo;
    PlayerStats playerStats;

    public LayerMask muros;


    [Header("Spawn Position")]
    public List<Transform> relativeSpawnPoints;
    public Collider2D playableArea;


    Transform player;

    private bool avanzandoWave = false;
    private bool todasWavesCompletadas = false;

    void Awake()
    {
        Debug.Log($"[Spawner] Awake id={GetInstanceID()}");
    }

    // Start is called before the first frame update
    void Start()
    {
        playerStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        player = GameObject.FindWithTag("Player").transform;
        CalcularWaveCuota();
    }

    // Update is called once per frame
    void Update()
    {
        if (todasWavesCompletadas) return;
        if (waveActualNumero >= waves.Count) return;

        bool waveTerminada =
            waves[waveActualNumero].spawnCount >= waves[waveActualNumero].waveQuota
            && enemiesVivos == 0;

        if (!avanzandoWave && waveTerminada)
        {
            Debug.Log($"[Spawner] Wave terminada idx={waveActualNumero} " +
                      $"spawnCount={waves[waveActualNumero].spawnCount}/" +
                      $"{waves[waveActualNumero].waveQuota}, vivos={enemiesVivos}");
            avanzandoWave = true;
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
        Debug.Log($"[Spawner] Iniciar siguiente (antes) idx={waveActualNumero}");
        yield return new WaitForSeconds(waveInterval);

        if (waveActualNumero < waves.Count - 1)
        {
            waveActualNumero++;
            Debug.Log($"[Spawner] Avanzó a idx={waveActualNumero}");

            
            playerStats.AumentarOleadas(waveActualNumero + 1);

            PrepararWave(waveActualNumero);
            CalcularWaveCuota();
        }
        else
        {
            todasWavesCompletadas = true;
        }
        avanzandoWave = false;
    }


    void PrepararWave(int idx)
    {
        waves[idx].spawnCount = 0;
        foreach (var g in waves[idx].enemyGroups) g.spawnCount = 0;
        enemigosMaximosCompletado = false;
        puedeSpawn = true;
        spawnTimer = 0f;
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
                        Collider2D colision = Physics2D.OverlapCircle(posicion, radioChequeo, layerPermitidaParaSpawn);

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
