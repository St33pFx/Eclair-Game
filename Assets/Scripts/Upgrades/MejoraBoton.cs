using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MejoraBoton : MonoBehaviour
{
    [SerializeField] private Mejoras Mejoras_script;

    public void Mejora()
    {
        string Mejora_Seleccionada = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent< TMP_Text> ().text;
        Mejoras_script.MejorarSeleccionada(Mejora_Seleccionada);
        
    }
}
