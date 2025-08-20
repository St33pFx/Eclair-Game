using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] GameObject pausaPanel;
    [SerializeField] Selectable primerSeleccion;

    bool estaPausado;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pausa();
        }
    }

    public void Pausa()
    {
         estaPausado = !estaPausado;
        pausaPanel.SetActive(estaPausado);
        Time.timeScale = estaPausado ? 0f : 1f;
        AudioListener.pause = estaPausado;

        var pc = FindAnyObjectByType<PlayerMovement>();
        var shoot = FindAnyObjectByType<WeaponShoot>();

        if (pc)
        {
            pc.canMove = !estaPausado;
        } 
            
        if (shoot)
        {
            shoot._puedeDisparar = !estaPausado;
        }

        if (estaPausado && primerSeleccion != null)
        {
            EventSystem.current.SetSelectedGameObject(null);
            primerSeleccion.Select();
        }
    }

    public void OnResume()
    {
        Pausa();
    }

    public void OnOptions()
    {
        Debug.Log("Abrir menu opciones");
    }

    public void OnExit()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }

    public void RegrasarMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}
