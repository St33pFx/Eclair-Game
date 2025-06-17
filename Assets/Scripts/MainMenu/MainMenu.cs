using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Niveles para cargar")]
    public string nivelNombre;
    private string nivelACargar;
    

    public void ComenzarJuego()
    {
        SceneManager.LoadScene(nivelNombre);
    }

    public void SalirJuego()
    {
        Application.Quit();
    }
}
