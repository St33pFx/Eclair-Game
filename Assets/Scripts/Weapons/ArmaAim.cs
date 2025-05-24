using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmaAim : MonoBehaviour
{
    private Camera _cam;
    private Vector3 _mousePos;
    private float _angulo;
    //private Transform _playerTransform;
    private GameObject playerObject;

    private void Awake()
    {
        _cam = Camera.main;
        playerObject = GameObject.FindWithTag("Player");
        
    }

    private void Update()
    {
        //CentrarArma();
        //AramaSeguirCursor();
    }

    private void FixedUpdate()
    {
        CentrarArma();
        AramaSeguirCursor();
    }

    private void CentrarArma()
    {
        transform.position = playerObject.transform.position + new Vector3(0, 1f, 0);
    }
     
    private void ArmaApuntando()
    {
        _mousePos = _cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
    }

    private void AramaSeguirCursor()
    {
        ArmaApuntando();

        Vector2 direccion = (Vector2)_mousePos - (Vector2)transform.position;

        _angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        Quaternion rotacion = Quaternion.Euler(0, 0, _angulo);
        transform.rotation = rotacion;

        rotarEscala();
    }

    private void rotarEscala()
    {
        Vector3 escalaPlayer = transform.localScale;

        if (_angulo > 90f || _angulo < -90f)
        {
            escalaPlayer.y = -Mathf.Abs(escalaPlayer.y);
        }
        else
        {
            escalaPlayer.y = Mathf.Abs(escalaPlayer.y);
        }

        transform.localScale = escalaPlayer;
    }
}