using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class Mejoras : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private PlayerUpgrades playerUpgrades;
    [SerializeField] private GameObject panelMejoras;

    [SerializeField] private Button Boton_Mejora1;
    [SerializeField] private Button Boton_Mejora2;
    [SerializeField] private Button Boton_Mejora3;

    [SerializeField] private TMP_Text Descripcion_Mejora1;
    [SerializeField] private TMP_Text Descripcion_Mejora2;
    [SerializeField] private TMP_Text Descripcion_Mejora3;

    // Pool de mejoras
    private readonly Mejora[] _Mejoras = new Mejora[]
    {
        new Mejora { Nombre = "Dash",                 Descripcion = "Aprende la habilidad de esquivar ataques.",                         Raresa = "Comun",      Aumento = 5,  Tipo = TipoMejora.Dash },
        new Mejora { Nombre = "Cruz Arrojadiza",      Descripcion = "Aprende la habilidad de lanzar una cruz bendita.",                 Raresa = "Raro",       Aumento = 0,  Tipo = TipoMejora.CruzArrojadiza },
        new Mejora { Nombre = "Velocidad de Proyectil", Descripcion = "Aumenta la velocidad a la que los proyectiles son disparados por X%", Raresa = "Raro",  Aumento = 2,  Tipo = TipoMejora.VelocidadProyectil },
        new Mejora { Nombre = "Daño de Proyectil",    Descripcion = "Aumenta el daño de proyectil un X%",                               Raresa = "Raro",       Aumento = 2,  Tipo = TipoMejora.DanoProyectil },
        new Mejora { Nombre = "Más Visión",           Descripcion = "Aumenta el área de visión un X%",                                  Raresa = "Epico",      Aumento = 10, Tipo = TipoMejora.MasVision },
        new Mejora { Nombre = "Perforación",          Descripcion = "Aumenta el número de enemigos que pueden ser dañados por +X",      Raresa = "Epico",      Aumento = 1,  Tipo = TipoMejora.Perforacion },
        new Mejora { Nombre = "Altar",                Descripcion = "Aumenta el daño que puede infligir el altar a enemigos un X%",     Raresa = "Legendario", Aumento = 2,  Tipo = TipoMejora.Altar },
    };

    // Runtime
    private Mejora m1, m2, m3;
    private readonly List<string> ultimoSet = new List<string>(3);

    void OnEnable()
    {
        SortearYArmar();
    }

    /// <summary>Úsalo desde tu sistema de LevelUp.</summary>
    public void Show()
    {
        if (panelMejoras) panelMejoras.SetActive(true);
        Time.timeScale = 0f;
        AudioListener.pause = true;
        SortearYArmar();
    }

    private void SortearYArmar()
    {
        if (Boton_Mejora1 == null || Boton_Mejora2 == null || Boton_Mejora3 == null)
        {
            Debug.LogError("Asigna los botones de mejoras en el Inspector.");
            return;
        }

        // 1) Elegimos 3 distintas, evitando repetir el set anterior completo
        var indices = new List<int>(_Mejoras.Length);
        for (int i = 0; i < _Mejoras.Length; i++) indices.Add(i);

        int intentos = 0;
        while (true)
        {
            intentos++;
            for (int i = 0; i < indices.Count; i++)
            {
                int r = Random.Range(i, indices.Count);
                (indices[i], indices[r]) = (indices[r], indices[i]);
            }

            m1 = _Mejoras[indices[0]];
            m2 = _Mejoras[indices[1]];
            m3 = _Mejoras[indices[2]];

            int coincidencias = 0;
            if (ultimoSet.Contains(m1.Nombre)) coincidencias++;
            if (ultimoSet.Contains(m2.Nombre)) coincidencias++;
            if (ultimoSet.Contains(m3.Nombre)) coincidencias++;

            if (coincidencias <= 1 || intentos > 10) break;
        }

        ultimoSet.Clear();
        ultimoSet.Add(m1.Nombre);
        ultimoSet.Add(m2.Nombre);
        ultimoSet.Add(m3.Nombre);

        // 2) Textos
        SetCarta(Boton_Mejora1, Descripcion_Mejora1, m1);
        SetCarta(Boton_Mejora2, Descripcion_Mejora2, m2);
        SetCarta(Boton_Mejora3, Descripcion_Mejora3, m3);

        // 3) Si prefieres enlazar por código (opcional):
        Boton_Mejora1.onClick.RemoveAllListeners();
        Boton_Mejora2.onClick.RemoveAllListeners();
        Boton_Mejora3.onClick.RemoveAllListeners();
        Boton_Mejora1.onClick.AddListener(Pick1);
        Boton_Mejora2.onClick.AddListener(Pick2);
        Boton_Mejora3.onClick.AddListener(Pick3);
    }

    // --- MÉTODOS PARA EL INSPECTOR (y usados arriba también) ---
    public void Pick1() { OnPick(m1); }
    public void Pick2() { OnPick(m2); }
    public void Pick3() { OnPick(m3); }

    public void AplicarSeleccion(TipoMejora tipo, int aumento)
    {
        var m = new Mejora
        {
            Nombre = tipo.ToString(),
            Descripcion = "",
            Raresa = "",
            Aumento = aumento,
            Tipo = tipo
        };
        OnPick(m);
    }

    private void SetCarta(Button boton, TMP_Text desc, Mejora m)
    {
        var titulo = boton.transform.GetChild(0).GetComponent<TMP_Text>();
        if (titulo) titulo.text = m.Nombre;
        if (desc) desc.text = m.Descripcion.Replace("X", m.Aumento.ToString());
    }

    public void OnPick(Mejora m)
    {
        if (!playerUpgrades)
        {
            Debug.LogError("PlayerUpgrades no asignado en Mejoras.");
            return;
        }

        playerUpgrades.Apply(m);

        if (panelMejoras) panelMejoras.SetActive(false);
        Time.timeScale = 1f;
        AudioListener.pause = false;

        Debug.Log($"Mejora aplicada: {m.Nombre} (+{m.Aumento}, {m.Tipo})");
    }
}

// ===================================================================

public class Mejora
{
    public string Nombre { get; set; }
    public string Descripcion { get; set; }
    public string Raresa { get; set; }
    public int Aumento { get; set; }
    public TipoMejora Tipo { get; set; }
}
