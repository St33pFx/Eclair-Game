using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using TMPro;



public class Mejoras : MonoBehaviour
{
    [SerializeField] private PlayerUpgrades playerUpgrades;
    [SerializeField] private GameObject panelMejoras;
    private Mejora mejora_1, mejora_2, mejora_3;

    Mejora[] _Mejoras = new Mejora[]
    {
        new Mejora { Nombre = "Dash", Descripcion = "Aprende la habilidad de esquivar ataques.", Raresa = "Comun", Aumento = 5, Tipo = TipoMejora.Dash},
        new Mejora { Nombre = "Cruz Arrojadiza", Descripcion = "Aprende la habilidad de lanzar una cruz bendita.", Raresa = "Raro", Aumento = 0, Tipo = TipoMejora.CruzArrojadiza},
        new Mejora { Nombre = "Velocidad de Projectil", Descripcion = "Aumenta la velocidad a la que los proyectiles son disparados por X%", Raresa = "Raro", Aumento = 2, Tipo = TipoMejora.VelocidadProyectil},
        new Mejora { Nombre = "Daño de Proyectil", Descripcion = "Aumenta el daño de proyectil un X%", Raresa = "Raro", Aumento = 2, Tipo = TipoMejora.DanoProyectil},
        new Mejora { Nombre = "Mas Vision", Descripcion = "Aumenta el area de vision un X%", Raresa = "Epico", Aumento = 10, Tipo = TipoMejora.MasVision},
        new Mejora { Nombre = "Perforacion", Descripcion = "Aumenta el numero de enemigos que pueden ser dañados por +X%", Raresa = "Epico", Aumento = 1, Tipo = TipoMejora.Perforacion},
        new Mejora { Nombre = "Altar", Descripcion = "Aumenta el daño que puede infligir el altar a enemigos un X%", Raresa = "Legendario", Aumento = 2, Tipo = TipoMejora.Altar},
    };

    [SerializeField] private Button Boton_Mejora1;
    [SerializeField] private Button Boton_Mejora2;
    [SerializeField] private Button Boton_Mejora3;

    [SerializeField] private TMP_Text Descripcion_Mejora1;
    [SerializeField] private TMP_Text Descripcion_Mejora2;
    [SerializeField] private TMP_Text Descripcion_Mejora3;
    

    private void Start()
    {
        Butones();
    }

    public void Butones()
    {
        List<int> mejorasDisponibles = new List<int>();
        for (int i = 0; i < _Mejoras.Length; i++)
        {
            mejorasDisponibles.Add(i);
        }

        SortearLista(mejorasDisponibles);
        mejora_1 = _Mejoras[mejorasDisponibles[0]];
        mejora_2 = _Mejoras[mejorasDisponibles[1]];
        mejora_3 = _Mejoras[mejorasDisponibles[2]];

        Boton_Mejora1.transform.GetChild(0).GetComponent<TMP_Text>().text = mejora_1.Nombre;
        Boton_Mejora2.transform.GetChild(0).GetComponent<TMP_Text>().text = mejora_2.Nombre;
        Boton_Mejora3.transform.GetChild(0).GetComponent<TMP_Text>().text = mejora_3.Nombre;

        Descripcion_Mejora1.text = mejora_1.Descripcion.Replace("X", mejora_1.Aumento.ToString()); 
        Descripcion_Mejora2.text = mejora_2.Descripcion.Replace("X", mejora_2.Aumento.ToString()); 
        Descripcion_Mejora3.text = mejora_3.Descripcion.Replace("X", mejora_3.Aumento.ToString());

        Dictionary<string, Color> rarezaColores = new Dictionary<string, Color>()
        {
            { "Comun",      new Color(0f, 0f, 0f, 0.5f) }, // 50% de opacidad
            { "Raro",       new Color(0f, 0f, 0f, 0.5f) },
            { "Epico",      new Color(0f, 0f, 0f, 0.5f) },
            { "Legendario", new Color(0f, 0f, 0f, 0.5f) }
        };

        //Boton_Mejora1.GetComponent<Image>().color = rarezaColores[mejora_1.Raresa];
        //Boton_Mejora2.GetComponent<Image>().color = rarezaColores[mejora_2.Raresa];
        //Boton_Mejora3.GetComponent<Image>().color = rarezaColores[mejora_3.Raresa];

    }

    public void MejorarSeleccionada(string nombreSeleccionado)
    {
        Mejora m = System.Array.Find(_Mejoras, x => x.Nombre == nombreSeleccionado);
        if (m == null)
        {
            Debug.LogWarning($"No encontré la mejora con nombre: {nombreSeleccionado}");
            return;
        }

        // Aplica al jugador
        playerUpgrades.Apply(m);

        
        if (panelMejoras) panelMejoras.SetActive(false);
        Time.timeScale = 1f;

        Debug.Log($"Mejora aplicada: {m.Nombre}");
    }



    public void SortearLista(List<int>list)
    {
        for(int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        
    }


}

public class Mejora
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string Raresa { get; set; }
    public int Aumento { get; set; }
    public TipoMejora Tipo { get; set; }
}
