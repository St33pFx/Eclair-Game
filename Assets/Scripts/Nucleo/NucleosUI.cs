using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NucleosUI : MonoBehaviour
{
    [SerializeField] private List<Image> iconos = new List<Image>();

    [Header("Material base")]
    [SerializeField] private Material grayscaleBase;

    [Header("Ajustes de look")]
    [Range(0, 1)] public float satGris = 0f;
    [Range(0, 1)] public float satColor = 1f;
    [Range(0, 1)] public float alphaGris = 0.6f;
    public float tweenDur = 0.25f;

    private readonly List<Material> mats = new List<Material>();
    private int destruidos = 0;
    private const string SAT_PROP = "_Saturation";

    private void Reset()
    {
        iconos.Clear();
        GetComponentsInChildren(iconos);
    }

    private void Awake()
    {
        if(grayscaleBase == null)
        {
            Debug.Log("[NucleosUI] Asigna el material base(UI_Grayscale_Mat).");
            enabled = false; return ;
        }

        mats.Clear();
        foreach(var img in iconos)
        {
            if(img == null) continue;
            var m = new Material(grayscaleBase);
            img.material = m;
            m.SetFloat(SAT_PROP, satGris);

            var c = img.color; c.a = alphaGris; img.color = c;
            mats.Add(m);
        }

        SetProgreso(0, animate:false);
    }

    public void Incrementar() => SetProgreso(destruidos + 1, animate: true);

    public void SetProgreso(int cantidad, bool animate = true)
    {
        int previo = destruidos;
        destruidos = Mathf.Clamp(cantidad, 0, iconos.Count);

        for (int i = 0; i < iconos.Count; i++)
        {
            bool activo = i < destruidos;
            float targetSat = activo ? satColor : satGris;
            float targetAlpha = activo ? 1f : alphaGris;

            if (i < mats.Count && mats[i] != null)
            {
                if (animate)
                    mats[i].DOFloat(targetSat, SAT_PROP, tweenDur).SetEase(Ease.OutQuad);
                else
                    mats[i].SetFloat(SAT_PROP, targetSat);
            }

            if (iconos[i] != null)
            {
                if (animate)
                    iconos[i].DOFade(targetAlpha, tweenDur);
                else
                {
                    var c = iconos[i].color; c.a = targetAlpha; iconos[i].color = c;
                }

                // Pequeño "pop" SOLO para el nuevo activado
                if (animate && activo && i == destruidos - 1)
                {
                    iconos[i].rectTransform.DOPunchScale(Vector3.one * 0.12f, tweenDur, 8, 0.8f);
                }
            }
        }
    }

    public void ResetProgreso() => SetProgreso(0, animate: false);

    void OnDestroy()
    {
        // Limpia clones de materiales
        foreach (var m in mats) if (m) Destroy(m);
        mats.Clear();
    }
}
