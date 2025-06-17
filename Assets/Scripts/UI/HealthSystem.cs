using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public GameObject[] corazones;

    public void ActualizarCorazones(int vidaActual)
    {
        for (int i = 0; i < corazones.Length; i++)
        {
            corazones[i].SetActive(i < vidaActual);
        }
    }
    public void tomarDanio(int danio)
    {
        
    }
}
