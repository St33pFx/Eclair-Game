using UnityEngine;

public class MejoraBoton : MonoBehaviour
{
    [SerializeField] private Mejoras mejoras;   
    public TipoMejora tipo;                    
    public int aumento = 10;                   

    // asigna este método al OnClick del Button
    public void OnClickMejora()
    {
        if (!mejoras)
        {
            Debug.LogWarning("Mejoras no asignado en MejoraBoton.", this);
            return;
        }
        mejoras.AplicarSeleccion(tipo, aumento);
    }
}
