using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_MenuMuerte : MonoBehaviour
{
    public void ReiniciarMenu()
    {
        GameManager.Reanudar();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SalirAlMenu()
    {
        GameManager.Reanudar();
        SceneManager.LoadScene("MainMenu");
    }
}
