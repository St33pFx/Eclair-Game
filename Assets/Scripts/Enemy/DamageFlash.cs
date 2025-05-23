using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    private Material _material;
    
    [ColorUsage(true, true)]
    [SerializeField] private Color _flashColor = Color.red;
    [SerializeField] private float _duracionFlashDaño = 0.25f;

    private void Awake()
    {
        
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Init();
    }

    private void Init()
    {
        _material = _spriteRenderer.material;
    }

    private Coroutine _damageFlashCoroutine;
    
    public void LlamarFlashDaño()
    {
        _damageFlashCoroutine = StartCoroutine(DañoFlash());
    }

    private IEnumerator DañoFlash()
    {
        // Establecer Color
        CambiarColor();
        
        //
        float CantidadBrillo = 0f;
        float tiempoTranscurrido = 0f;

        while (tiempoTranscurrido < _duracionFlashDaño)
        {
            tiempoTranscurrido += Time.deltaTime;
            
            //Lerp cantidad de flash
            CantidadBrillo = Mathf.Lerp(1f, 0f, tiempoTranscurrido / _duracionFlashDaño);
            EstablencerCantidadFlash(CantidadBrillo);
            
            yield return null;
        }
    }

    private void CambiarColor()
    {
        _material.SetColor("_FlashColor", _flashColor);
    }

    private void EstablencerCantidadFlash(float amount)
    {
        _material.SetFloat("_FlashAmount", amount);
        
    }
}
