using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
// using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

using UnityEngine;

public class CruzArrojadiza : WeaponController
{
    /// <summary>
    /// Esta daga se lanza automaticamente
    /// </summary>
    [Header("Cruz")]
    public Rigidbody2D _projectile;
    public Transform _puntoDeDisparo;
    public float _velocidadDisparo;
    private bool _haColision; 
    public bool canShoot = true;

    private void Awake()
    {
        if (_puntoDeDisparo == null)
        {
            Debug.LogError($"Te hace falta poner el {_puntoDeDisparo} papu.");
        }
    }

    

    protected override void Attack()
    {
        if ( canShoot == true)
        {

            Rigidbody2D _nuevoClon = Instantiate(_projectile, _puntoDeDisparo.position, PlayerMovement.rotacin);
            _nuevoClon.velocity = PlayerMovement._direccionApunta * _velocidadDisparo;
            gameObject.GetComponent<SpriteRenderer>();
            // Cooldown para poder disparar nuevamente.
            _cooldownActual = _cooldownDuracion;
            Debug.Log("no atacas");
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("noPaso"))
        {
            canShoot = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("noPaso"))
        {
            canShoot = true;
        }

    }
}
