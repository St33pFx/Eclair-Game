using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemigoDespawn : MonoBehaviour
{
    private Transform jugadorPos;
    private EnemigoSpawner spawner;
    private bool _puedeDespawn = false;
    
    [SerializeField] private float despawnDistancia = 9f;
    [SerializeField] private float tiempoProteccionDespawn = 10f;   
    private void Awake()
    {
        jugadorPos = GameObject.FindWithTag("Player").transform;
        StartCoroutine(ProteccionDespawn());
    }

    private void Update()
    {
        DistanciaMaximaDespawn();
    }

    // Este codigo calcula la distancia entre la posicion del objeto y la del jugador que fue referenciado
    //Pa despues calcular si la distancia es mayor a la permitida y despawnear al enemigo
    private void DistanciaMaximaDespawn()
    {
        if (Vector2.Distance(jugadorPos.position, transform.position) >= despawnDistancia)
        {
            if (spawner != null)
            {
                if (_puedeDespawn == true)
                {
                    spawner.EliminarEnemigo(this.gameObject);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    public void SetSpawner(EnemigoSpawner spwn)
    {
        spawner = spwn; 
    }

    private IEnumerator ProteccionDespawn()
    {
        _puedeDespawn = false;
        yield return new WaitForSeconds(tiempoProteccionDespawn);
        _puedeDespawn = true;
    }
}
