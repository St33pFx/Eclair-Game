using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TipoMejora
{
    Dash,
    CruzArrojadiza,
    VelocidadProyectil,
    DanoProyectil,
    MasVision,
    Perforacion,
    Altar
}


public class PlayerUpgrades : MonoBehaviour
{
    public bool hasDash = false;
    public bool hasCruzArrojadiza = false;

    public float projectileSpeedMult = 1f;
    public float projectileDamageMult = 1f;
    public int pierceExtra = 0;
    public float visionRadiusExtra = 0f;
    public float altarDamageMult = 1f;

    public void Apply(Mejora m)
    {
        switch (m.Tipo)
        {
            case TipoMejora.Dash: hasDash = true; break;
            case TipoMejora.CruzArrojadiza: hasCruzArrojadiza = true; break;
            case TipoMejora.VelocidadProyectil: projectileSpeedMult *= 1f + (m.Aumento / 100f); break;
            case TipoMejora.DanoProyectil: projectileDamageMult *= 1f + (m.Aumento / 100f); break;
            case TipoMejora.Perforacion: pierceExtra += m.Aumento; break;
            case TipoMejora.MasVision: visionRadiusExtra += m.Aumento; break;  
            case TipoMejora.Altar: altarDamageMult *= 1f + (m.Aumento / 100f); break;
        }
    }
}
