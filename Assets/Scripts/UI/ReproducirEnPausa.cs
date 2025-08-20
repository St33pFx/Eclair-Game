using UnityEngine;

[RequireComponent(typeof(AudioSource))] 
public class ReproducirEnPausa : MonoBehaviour
{
    void Awake()
    {
        var src = GetComponent<AudioSource>();
        src.ignoreListenerPause = true;  // clave: la UI suena aunque AudioListener.pause = true
        src.playOnAwake = false;
        src.loop = false;
        src.spatialBlend = 0f;
        Debug.Log($"[UIAudioInit] ignoreListenerPause = {src.ignoreListenerPause}");
    }
}
