using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemigoSpawner : MonoBehaviour
{
    /// <summary>
    /// Comentarios del poderoso yahir para el poderoso yahir del futuro :)
    /// </summary>
    
    // prefab a instanciar 
    [SerializeField] private GameObject enemigoPrefab;
    [SerializeField] private GameObject enemigoPrefab2;
    [SerializeField] private GameObject enemigoPrefab3;
    
    // Referenciar al jugador (su posicion krak)
    [SerializeField] private Transform jugador;
    
    // Tiempo de espera al  iniciar juegos
    [SerializeField] private float tiempoSpawn = 0.1f;
    
    // Distancia/Campo de vision para spawn
    [SerializeField] private float visionMinima = 10f;
    [SerializeField] private float visionMaxima = 20f;

    // Numero maximo de Spawn de enemigos
    [SerializeField] private int numeroMaxEnemigos = 22;
    
    private List<GameObject> _enemigos = new List<GameObject>();
    
    // Metodo para iniciar corutina de spawn
    private void Start()
    {
        // Inicio de la lista y su capacidad papa
        _enemigos = new List<GameObject>();
        _enemigos.Capacity = numeroMaxEnemigos;
        
        // Inicio de la corutina pa spawnear chango
        StartCoroutine(InstanciarEnemigoCoroutine());
    }
    
    // Metodo para la posicion aleatoria
    private Vector2 SpawnEnemigoPos()
    {
        Vector2 direccionAleatoria = Random.insideUnitCircle.normalized;
        float distancia = Random.Range(visionMinima, visionMaxima);
        Vector2 puntospawn = (Vector2)jugador.position + direccionAleatoria * distancia;
        
        return puntospawn;
    }

    private IEnumerator InstanciarEnemigoCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(tiempoSpawn);

            if (_enemigos.Count <= numeroMaxEnemigos)
            {
                SpawnEnemigo();
            }
        } 
    }

    private void SpawnEnemigo()
    {
        GameObject _nuevoEnemigo = Instantiate(enemigoPrefab, SpawnEnemigoPos(), Quaternion.identity);
        _nuevoEnemigo.GetComponent<EnemigoDespawn>().SetSpawner(this);
        IEnemigoSpawneable enemigoSpawneable = _nuevoEnemigo.GetComponent<IEnemigoSpawneable>();
        enemigoSpawneable.ReferenciarSpawn(this);

        
        if (_enemigos.Count <= numeroMaxEnemigos && !_enemigos.Contains(_nuevoEnemigo))
        {
            _enemigos.Add(_nuevoEnemigo);
        }
    }
    
    public void EliminarEnemigo(GameObject enemigo)
    {
        if (_enemigos.Contains(enemigo))
        {
            _enemigos.Remove(enemigo);
        }
    }
    
}
