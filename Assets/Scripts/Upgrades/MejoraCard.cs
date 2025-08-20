using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MejoraCard : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] TMP_Text titulo;
    [SerializeField] TMP_Text descripcion;
    [SerializeField] Button boton;

    // runtime
    Mejora dato;
    Mejoras controller;

    // La usa el controller para “inyectar” la mejora sorteada
    public void SetData(Mejora m, Mejoras owner)
    {
        dato = m;
        controller = owner;

        if (titulo) titulo.text = m.Nombre;
        if (descripcion) descripcion.text = m.Descripcion.Replace("X", m.Aumento.ToString());

        if (boton)
        {
            boton.onClick.RemoveAllListeners();
            boton.onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        if (controller != null && dato != null)
            controller.OnPick(dato);
    }
}
