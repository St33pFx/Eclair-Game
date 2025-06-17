using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Este es mi referencia global para mi singleton
    public static AudioManager Instance;
    
    [Header("Fuentes de Audio")]
    [SerializeField] private AudioSource fuenteMusica;
    [SerializeField] private AudioSource fuenteSfx;
    
    [SerializeField] private AudioSource pasosSfx;
    
    [Header("Audio de Fondo")]
    [SerializeField] private AudioClip musicaFondo;
    [SerializeField] private AudioClip clipPasos;
    
    private void Awake()
    {
        // Nomas if pa evitar errores de consola jeje, este if si es null es true
        if (Instance == null)
        {
            // Si efectivamente no existe pues esta es la nueva instancia papui
            Instance = this;
            // Metodo para que no se desactive/elimine en cambios de escena
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si de pura casualidad existe pos se elimina pa evitar dupliocados
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (musicaFondo != null)
        {
            fuenteMusica.clip = musicaFondo;
            fuenteMusica.loop = true;
            fuenteMusica.Play();
        }
    }

    public void PlaySonido(AudioClip clip)
    {
        if (clip != null && fuenteSfx != null)
        {
            fuenteSfx.PlayOneShot(clip);
        }
    }

    public void CambiarMusica(AudioClip nuevaMusica)
    {
        if (nuevaMusica != null)
        {
            fuenteMusica.clip = nuevaMusica;
            fuenteMusica.Play();
        }
    }

    public void ReanudarMusica()
    {
        fuenteSfx.UnPause();
    }
}
