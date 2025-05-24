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
    private bool _puedeDisparar = true;

    private void Awake()
    {
        
    }

    private void Update()
    {
        EjecutarDisparo();
    }

    private IEnumerator CoolDown()
    {
        
        yield return new WaitForSeconds(_cooldownDisparo);
        _puedeDisparar = true;
    }
    
    private void EjecutarDisparo()
    {
        if (Input.GetMouseButtonDown(0) && _puedeDisparar == true)
        {
            GameObject _balaCLon = Instantiate(proyectilPrefab, shootPoint.transform.position, shootPoint.transform.rotation);
            _balaCLon.GetComponent<Rigidbody2D>().velocity = shootPoint.right * velocidadBala;
            _puedeDisparar = false;
            StartCoroutine(CoolDown());
        }

        
    }
    
}
