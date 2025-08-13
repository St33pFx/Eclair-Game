using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{

    public int bloodPint = 50;
    [SerializeField] private int xpAmount = 10;


    // Referencia Jugador
    private GameObject player;
    private PlayerStats playerStats;


    [SerializeField] private float radioAtraccion = 1f;
    [SerializeField] private float velocidadAtraccion = 10f;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
            playerStats = player.GetComponent<PlayerStats>();
    }

    private void Update()
    {
        Atraccion();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            var pm = other.GetComponent<PlayerMovement>();
            if (pm != null)
                pm.AgregarBloodPoints(bloodPint);
            playerStats.AumentarBloodPoints(bloodPint);


            if (playerStats != null)
                playerStats.AumentarExperiencia(xpAmount);

            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Ritual"))
        {
            if (Altar.llenarRitual <= 5)
            {
                Altar.llenarRitual++;

                Destroy(gameObject);
            }


        }
    }


    private void Atraccion()
    {
        float distancia = Vector2.Distance((Vector2)player.transform.position, (Vector2)transform.position);

        if (distancia <= radioAtraccion)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, velocidadAtraccion * Time.deltaTime);

        }
    }
}