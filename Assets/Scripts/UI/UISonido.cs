using UnityEngine;
using UnityEngine.EventSystems;

public class UISound : MonoBehaviour, ISelectHandler, IPointerClickHandler
{
    public AudioSource src;
    public AudioClip hover;
    public AudioClip click;

    void Awake() { if (!src) src = FindObjectOfType<AudioSource>(); }

    public void OnSelect(BaseEventData e)
    {
        if (src && hover) src.PlayOneShot(hover, 0.6f);
    }

    public void OnPointerClick(PointerEventData e)
    {
        if (src && click) src.PlayOneShot(click, 0.9f);
    }
}
