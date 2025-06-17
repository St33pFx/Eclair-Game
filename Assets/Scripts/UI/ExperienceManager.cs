    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

public class ExperienceManager : MonoBehaviour
{
    [Header("Enemigos")]
    [SerializeField] private GameObject enemigo2;
    [SerializeField] private GameObject enemigo3;
    
    private EnemigoSpawner spawner;
    
    [Header("Experiencia")]
    [SerializeField] AnimationCurve experienciaCurve;

    [Header("UI")]
    [SerializeField] Image experienciaFill;
    
    [SerializeField] private int nivelActual;
    private int expTotal;
    private int expAnterior;
    private int sigNivelExp;

    private void Start()
    {
        ActualizarBarraNivel();
    }

    private void Update()
    {
        
    }

    public void AgregarExperiencia(int cantidad)
    {
        expTotal += cantidad;
        RevisarSubidaNivel();
        ActualizarInterfaz();
    }

    public void RevisarSubidaNivel()
    {
        if (expTotal >= sigNivelExp)
        {
            nivelActual++;
            ActualizarBarraNivel();
            SpawnEnemigo();
            MostrarNivel();
        }
    }

    private void ActualizarBarraNivel()
    {
        expAnterior = (int)experienciaCurve.Evaluate(nivelActual);
        sigNivelExp = (int)experienciaCurve.Evaluate(nivelActual + 1);

        ActualizarInterfaz();
    }

    private void ActualizarInterfaz()
    {
        int inicio = expTotal - expAnterior;
        int final = sigNivelExp - expAnterior;
        
        experienciaFill.fillAmount = (float)(inicio) / (float)(final);
    }

    public void SpawnEnemigo()
    {
        if (nivelActual == 2)
        {
            
        }

        if (nivelActual == 4)
        {
            
        }
    }


    public void MostrarNivel()
    {
        Debug.Log($"nivelActual: {nivelActual}");
    }

}
