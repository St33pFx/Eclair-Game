
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TextColor : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler
{
    public TextMeshProUGUI texto;
    public Color normalColor = new Color32(0x63, 0x00, 0x00, 0xFF);   // #630000
    public Color colorHover = new Color32(0xFF, 0xD4, 0xD4, 0xFF);   // #FFD4D4

    Selectable sel;

    void Awake()
    {
        if (!texto) texto = GetComponentInChildren<TextMeshProUGUI>();
        sel = GetComponent<Selectable>();
    }

    void OnEnable()
    {
        if (texto) texto.color = normalColor;
    }

    // Al entrar con el mouse, forzamos selección (esto dispara Deselect en el anterior)
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (sel) sel.Select();
    }

    // Colorea SOLO cuando este botón está seleccionado
    public void OnSelect(BaseEventData eventData)
    {
        if (texto) texto.color = colorHover;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (texto) texto.color = normalColor;
    }
}
