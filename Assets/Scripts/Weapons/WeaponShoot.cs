using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float velocidadBala;

    private Rigidbody2D _rbProyectil;
    private ArmaAim _aim;

    private int _danio = 1;
    private float _cooldownDisparo = 0.5f;
    private float cooldownActual = 0f;
    public bool _puedeDisparar = true;
    private CinemachineImpulseSource impulseSource;
    
    [SerializeField] private AudioSource fuenteDisparo;
    [SerializeField] private AudioClip sonidoDisparo;

    private void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (GameManager.juegoPausado) return;
        if (_puedeDisparar)
        {
            EjecutarDisparo();
        }
        
        
    }

    private IEnumerator CoolDown()
    {
        
        yield return new WaitForSeconds(_cooldownDisparo);
        _puedeDisparar = true;
    }
    
    private void EjecutarDisparo()
    {
        if (_puedeDisparar)
        {
            if (Input.GetMouseButtonDown(0) && _puedeDisparar == true)
            {
                GameObject _balaCLon = Instantiate(proyectilPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
                _balaCLon.GetComponent<Rigidbody2D>().velocity = shootPoint.right * velocidadBala;
                _puedeDisparar = false;
                CameraShakeManager.instance.CameraShake(impulseSource);
                DispararSonido();
                StartCoroutine(CoolDown());
            }
        }
        

        
    }

    
    private void DispararSonido()
    {
        if (sonidoDisparo != null && fuenteDisparo != null)
        {
            fuenteDisparo.PlayOneShot(sonidoDisparo);
        }
    }
    
}
