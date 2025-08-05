using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class enemyFollow : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AIDestinationSetter destino = GetComponent<AIDestinationSetter>();
        GameObject jugador = GameObject.FindGameObjectWithTag("Player");

        if (destino != null && jugador != null)
        {
            destino.target = jugador.transform;
        }
        else
        {
            Debug.LogWarning("Faltan referencias en el enemigo: " + gameObject.name);
        }
    }

}
