using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;


public class Nosferatu : EnemyController
{
    [SerializeField] private Rigidbody2D enemyRb;
    [SerializeField] private Transform playerTransform;


    private void Awake()
    {
        enemyRb = GetComponent<Rigidbody2D>();

        if (playerTransform == null)
        {
            Debug.LogError("Ups, transform del player no ha sido asignado!");
        }

    }

    private void FixedUpdate()
    {
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

}
