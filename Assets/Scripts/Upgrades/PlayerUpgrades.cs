using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerUpgrades : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] PlayerMovement player;
    [SerializeField] MonoBehaviour cruzScript;
    [SerializeField] Light2D visionLight;
    [SerializeField] private float maxOuterRadius = 12f;

    // Visión (sobre valor base)
    private float baseInner, baseOuter;
    private int visionTotalPct = 0;

    [Header("State")]
    public bool hasDash = false;
    public bool hasCruzArrojadiza = false;

    [Header("Combat")]
    public float projectileSpeedMult = 1f; // velocidad de la bala (multiplicador)
    public float fireRateMult = 1f;  // cadencia ( >1 => menos cooldown )

    // --- NUEVO: daño en pasos de 0.5 ---
    public float projectileBaseDamage = 1f; // daño base = 1.0
    public int damageHalfSteps = 0;       // cada step = +0.5 daño

    // --- NUEVO: perforación ---
    public int pierceExtra = 0;             // 0 = golpea 1 objetivo; 1 = 2 objetivos, etc.

    public float altarDamageMult = 1f;

    void Awake()
    {
        if (!visionLight) visionLight = GetComponentInChildren<Light2D>(true);
        if (visionLight)
        {
            baseInner = visionLight.pointLightInnerRadius;
            baseOuter = visionLight.pointLightOuterRadius;
        }
        if (player) player.EnableDash(false);
        if (cruzScript) cruzScript.enabled = false;
    }

    public void Apply(Mejora m)
    {
        switch (m.Tipo)
        {
            case TipoMejora.Dash:
                hasDash = true;
                if (player) player.EnableDash(true);
                break;

            case TipoMejora.CruzArrojadiza:
                hasCruzArrojadiza = true;
                if (cruzScript) cruzScript.enabled = true;
                if (player) player.SetCruzDesbloqueada(true);
                break;

            case TipoMejora.VelocidadProyectil:
                {
                    float k = 1.5f + (m.Aumento / 100f);   // 10 => 1.10
                    projectileSpeedMult *= k;
                    fireRateMult *= k;            // menos cooldown
                    break;
                }

            // --- NUEVO: +0.5 de daño por cada "Aumento" ---
            case TipoMejora.DanoProyectil:
                damageHalfSteps += Mathf.Max(1, m.Aumento); // 1 => +0.5 ; 2 => +1.0
                break;

            // --- NUEVO: +1 objetivo por cada "Aumento" ---
            case TipoMejora.Perforacion:
                pierceExtra += Mathf.Max(1, m.Aumento);     // 1 => +1 objetivo
                break;

            case TipoMejora.MasVision:
                if (visionLight)
                {
                    visionTotalPct += m.Aumento;
                    float k = 2f + (visionTotalPct / 100f);
                    visionLight.pointLightInnerRadius = baseInner * k;
                    visionLight.pointLightOuterRadius = Mathf.Min(baseOuter * k, maxOuterRadius);
                }
                break;

            case TipoMejora.Altar:
                altarDamageMult *= 1f + (m.Aumento / 100f);
                break;
        }
    }

    // --- NUEVO: helpers para leer daño y perforación ---
    public float GetProjectileDamage() => projectileBaseDamage + 0.5f * damageHalfSteps;
    public int GetMaxTargetsHit() => 1 + Mathf.Max(0, pierceExtra);
}
