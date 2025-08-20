using TMPro;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class SelectParpadeo : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerEnterHandler
{
    public float cantidad;
    public float velocidad = 12f;

    public TextMeshProUGUI texto;
    bool ejecutandose;
    float t0;
    Vector3 escalaBase;

    private void Awake()
    {
        if (!texto)
        {
            texto = GetComponentInChildren<TextMeshProUGUI>();
        }
    }

    public void OnEnable()
    {
        ejecutandose = false;
        if (texto)
        {
            escalaBase = texto.rectTransform.localScale;
            texto.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0f);
        }
    }

    public void OnPointerEnter(PointerEventData e)
    {
        var sel = GetComponent<Selectable>();
        if (sel) sel.Select();
    }

    public void OnSelect(BaseEventData e)
    {
        if (!texto || ejecutandose) return;
        StartCoroutine(Parpadeo());
    }

    public void OnDeselect(BaseEventData e)
    {
        ejecutandose = false;
        if (texto)
        {
            texto.rectTransform.localScale = escalaBase;
            texto.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0f);
        }
    }

    System.Collections.IEnumerator Parpadeo()
    {
        ejecutandose = true;
        t0 = Random.value * 10f;
        var rt = texto.rectTransform;

        while (ejecutandose)
        {
            //  multiplicar por 'cantidad', no sumar
            float s = 1f + Mathf.Sin((Time.unscaledTime + t0) * velocidad) * cantidad;
            rt.localScale = escalaBase * s;

            float dilate = Mathf.Sin((Time.unscaledTime + t0) * velocidad) * 0.05f;
            texto.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, dilate);

            yield return null;
        }
    }
}
