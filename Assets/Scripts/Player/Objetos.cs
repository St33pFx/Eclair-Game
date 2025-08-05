using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objetos : MonoBehaviour
{
    private PlayerMovement player;
    private int vidaPunto = 1;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player.RecibirVida(vidaPunto);
            Destroy(this.gameObject);
        }
    }

    
}
