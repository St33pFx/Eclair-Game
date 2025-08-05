using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName;
    public EnemySpawnInf[] enemiesPorOleada;
    public int levelSpawn;
    public float spawnRate = 0.1f;

}

public class WaveEnemies : MonoBehaviour
{
    [SerializeField] Wave[] waves;
    private EnemigoSpawner enemigoSpawner;

    private Wave oleadaActual;
    private int oleadaActualNumero;

    [SerializeField] int maxEnemigosPorOleada = 35;
    
    private bool oleadaEnCurso = false;

    int CountEnemiesOfType(string enemyName)
    {
        int count = 0;
        GameObject[] todos = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (var obj in todos)
        {
            if (obj.name.Contains(enemyName)) // "Nosferatu(Clone)"
                count++;
        }

        return count;
    }

    
    
    IEnumerator SpawnOleada()
    {
        oleadaActual = waves[oleadaActualNumero];
        int totalEnemigosPorOleada = 0;

        foreach (var data in oleadaActual.enemiesPorOleada)
            totalEnemigosPorOleada += data.cantidad;

        while (true)
        {
            int enemigosVivos = GameObject.FindGameObjectsWithTag("Enemy").Length;

            if (enemigosVivos < totalEnemigosPorOleada)
            {
                foreach (EnemySpawnInf enemyData in oleadaActual.enemiesPorOleada)
                {
                    if (enemyData.enemyPrefab == null)
                    {
                        Debug.LogWarning("Prefab faltante en la oleada " + oleadaActual.waveName);
                        continue;
                    }

                    int instanciasDeEste = CountEnemiesOfType(enemyData.enemyPrefab.name);

                    if (instanciasDeEste < enemyData.cantidad)
                    {
                        enemigoSpawner.SpawnEnemigo(enemyData.enemyPrefab);
                        yield return new WaitForSeconds(oleadaActual.spawnRate);
                    }
                }

            }

            yield return null;
            
            // Verifica si ya no quedan enemigos vivos
            enemigosVivos = GameObject.FindGameObjectsWithTag("Enemy").Length;

            bool oleadaCompletada = true;
            foreach (var data in oleadaActual.enemiesPorOleada)
            {
                if (data.enemyPrefab == null)
                {
                    Debug.LogWarning("Prefab destruido o null en oleada: " + oleadaActual.waveName);
                    continue;
                }

                if (CountEnemiesOfType(data.enemyPrefab.name) > 0)
                {
                    oleadaCompletada = false;
                    break;
                }
            }


            if (oleadaCompletada)
            {
                Debug.Log("Oleada completada. Pasando a la siguiente...");
    
                oleadaActualNumero++;

                if (oleadaActualNumero < waves.Length)
                {
                    oleadaActual = waves[oleadaActualNumero];
                    yield return new WaitForSeconds(3f); // espera antes de la siguiente
                }
                else
                {
                    Debug.Log("¡No hay más oleadas!");
                    yield break; // rompe el ciclo
                }
            }

        }
    }

    
    void Awake()
    {
        foreach (var wave in waves)
        {
            int total = 0;
            foreach (var e in wave.enemiesPorOleada)
                total += e.cantidad;

            if (total > maxEnemigosPorOleada)
                Debug.LogWarning($"{wave.waveName} tiene más de {maxEnemigosPorOleada} enemigos!");
        }
    }

    
    private void Start()
    {
        enemigoSpawner = FindObjectOfType<EnemigoSpawner>();
        
    }


    void Update()
    {
        if (!oleadaEnCurso)
        {
            StartCoroutine(SpawnOleada());
            oleadaEnCurso = true;
        }
    }


}

[System.Serializable]
public class EnemySpawnInf
{
    public GameObject enemyPrefab;
    public int cantidad;
}

