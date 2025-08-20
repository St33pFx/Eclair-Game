using UnityEngine.Rendering.Universal;
using UnityEngine;

public enum HabilidadID
{
    Dash,
    CruzArrojadiza,
    VelocidadProyectil,
    DanoProyectil,
    MasVision,
    Perforacion,
    Altar
}

public class HabilidadesManager : MonoBehaviour
{
    public static HabilidadesManager I { get; private set; }

    [Header("Referencias")]
    public PlayerMovement player;
    public CruzArrojadiza cruz;
    public Light2D LuzVision;

    [Header("Estado")]
    public bool tieneDash;
    public bool tieneCruz;
    public float vision;

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        Light2D luz = GetComponent<Light2D>();
        I = this;
    }

    public void Desbloqueas(HabilidadID id)
    {
        switch (id)
        {
            case HabilidadID.CruzArrojadiza:
                tieneCruz = true;
                if (cruz)
                {
                    cruz.enabled = true;
                    Debug.Log("Cruz Activada");
                }
                break;

            case HabilidadID.MasVision:
                vision *= 1.10f;
                AplicarVision();
                Debug.Log("Se ha incrementado la vision");
                break;

            case HabilidadID.Dash:
                tieneDash = true;
                if (player)
                {
                    // player.EnableDash(true);
                    Debug.Log("Dash activaado");
                }
                break;
        }
    }

    public void AplicarVision()
    {
        if (!LuzVision)
        {
            return;
        }

        LuzVision.pointLightOuterRadius *= 1.10f;
    }

}
