using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public class CursorMenu : MonoBehaviour
{
    public RectTransform flecha;
    public Vector2 espacio_Distancia = new Vector2(10, 0);
    Selectable actual;

    private void Update()
    {
        var sel = EventSystem.current ? EventSystem.current.currentSelectedGameObject : null;
        if(!sel) return;

        if(sel.TryGetComponent(out Selectable s))
        {
            if(s != actual)
            {
                actual = s;
                PosicionarFlecha();
            }
        }
    }

    void PosicionarFlecha()
    {
        if(!flecha || !actual) return;
        var rt = actual.GetComponent<RectTransform>();
        flecha.SetParent(rt, worldPositionStays: false);
        flecha.anchorMin = new Vector2(0.5f, 0.5f); 
        flecha.anchorMax = new Vector2(1, 0.5f);
        flecha.anchoredPosition = espacio_Distancia;
        flecha.SetAsLastSibling();
        flecha.gameObject.SetActive(true);
    }
}
