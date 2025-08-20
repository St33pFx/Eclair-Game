using UnityEngine;

public class NucleoDestruible : MonoBehaviour
{
    [SerializeField] private NucleosUI nucleosUI;   // arrástralo desde la escena
    private bool contado = false;

    // Llama a esto cuando el núcleo "muera"
    public void Destruir()
    {
        if (!contado)
        {
            if (nucleosUI != null) nucleosUI.Incrementar();
            contado = true;
        }
        Destroy(gameObject);
    }
}
