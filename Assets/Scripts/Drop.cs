using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField]private int bloodPint = 5;
    
    // Referencia Jugador
    private GameObject player;
    
    [SerializeField] private float radioAtraccion = 1f;
    [SerializeField] private float velocidadAtraccion = 10f;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Atraccion();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement player = other.gameObject.GetComponent<PlayerMovement>();

            if (player != null)
            {
                player.AgregarBloodPoints(bloodPint);
                Destroy(this.gameObject);
            }
        }
    }
    
    
    private void Atraccion()
    {
        float distancia = Vector2.Distance((Vector2)player.transform.position, (Vector2)transform.position);

        if (distancia <= radioAtraccion)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, velocidadAtraccion  * Time.deltaTime);

        }
    }
}
