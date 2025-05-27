using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using TMPro;
using UnityEngine;


public class Nosferatu : EnemyController, IEnemigoSpawneable
{
    [SerializeField] private Rigidbody2D enemyRb;
    private Transform playerTransform;


    private EnemigoSpawner spawner;
    private DamageFlash _damageFlash;
    private Nosferatu _nosferatu;

    public void ReferenciarSpawn(EnemigoSpawner spwn)
    {
        spawner = spwn;
    }
    
    

    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        _damageFlash = GetComponent<DamageFlash>();

        if (playerTransform == null)
        {
            Debug.LogError("Ups, transform del player no ha sido asignado!");
        }

    }
    

    private void FixedUpdate()
    {
        if (playerTransform == null) return;
        
        // Direccion crea un vector y NuevaDireccion crea el movimiento 
        Vector2 direccion = ((Vector2)(playerTransform.position - transform.position)).normalized;
        Vector2 nuevaDireccion = (Vector2)transform.position + direccion * velocidadMovimiento * Time.fixedDeltaTime; 
        
        // If para evitar que se glitchee el nosferatu
        // Vector distance devuelve la distancia entre puntos a y b
        if (Vector2.Distance((Vector2)playerTransform.position, (Vector2)transform.position) > 0.1f)
        {
            // MovePosition mueve el rigid body a una posicion especificada, acuerdate papui 
            enemyRb.MovePosition(nuevaDireccion);
        }
    }

    public void EstablecerSpawn(EnemigoSpawner spwn)
    {
        spawner = spwn;
        
    }

    public override void RecibirDaño(int daño)
    {
        _damageFlash.LlamarFlashDaño();
        base.RecibirDaño(daño);
        Debug.Log($"Recibiendo {daño}");
    }

    protected override void Morir()
    {
        if (spawner != null)
        {
            spawner.EliminarEnemigo(this.gameObject);
        }
        Instantiate(objetoDrop, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }

    
    
}
