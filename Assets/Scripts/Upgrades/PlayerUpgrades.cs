using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PlayerUpgrades : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] PlayerMovement player;      // Player (mismo GO que mueve)
    [SerializeField] MonoBehaviour cruzScript;   // MISMO componente CruzArrojadiza del Player
    [SerializeField] Light2D visionLight;

    [Header("State")]
    public bool hasDash = false;
    public bool hasCruzArrojadiza = false;
    public float projectileSpeedMult = 1f;
    public float projectileDamageMult = 1f;
    public int pierceExtra = 0;
    public float altarDamageMult = 1f;

    void Awake()
    {
        if (player) player.EnableDash(false);
        if (cruzScript) cruzScript.enabled = false; // arranca apagada
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
                if (cruzScript) cruzScript.enabled = true;     // activa el script de cruz
                if (player) player.SetCruzDesbloqueada(true);  // <<--- ACTIVA AL PLAYER
                Debug.Log("[Upgrades] Cruz Arrojadiza ACTIVADA");
                break;



            case TipoMejora.VelocidadProyectil:
                projectileSpeedMult *= 1f + (m.Aumento / 100f);
                break;

            case TipoMejora.DanoProyectil:
                projectileDamageMult *= 1f + (m.Aumento / 100f);
                break;

            case TipoMejora.Perforacion:
                pierceExtra += m.Aumento;
                break;

            case TipoMejora.MasVision:
                if (visionLight)
                    visionLight.pointLightOuterRadius *= 1f + (m.Aumento / 100f);
                break;

            case TipoMejora.Altar:
                altarDamageMult *= 1f + (m.Aumento / 100f);
                break;
        }
    }
}
