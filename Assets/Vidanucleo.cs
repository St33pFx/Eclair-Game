using System.Collections;
using System.Collections.Generic;
using Nucleo;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Vidanucleo : MonoBehaviour
{
    public static int vidaTotal = 2;
    public static int muertes;
    void Update()
    {
        if ( NucleoManager.vidaActual == 7)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
