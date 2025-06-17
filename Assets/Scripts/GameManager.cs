using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool juegoPausado = false;

    public static void Pausa()
    {
        Time.timeScale = 0;
        juegoPausado = true;
    }

    public static void Reanudar()
    {
        juegoPausado = false;
        Time.timeScale = 1;
    }
}
