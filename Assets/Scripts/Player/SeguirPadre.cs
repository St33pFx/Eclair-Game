using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeguirPadre : MonoBehaviour
{
    [SerializeField] private Transform hijoAseguir;

    private void Start()
    {
        
        hijoAseguir = transform.GetChild(0);
        
    }

    private void LateUpdate()
    {
        if (hijoAseguir != null)
        {
            transform.position = hijoAseguir.position;
        }
    }
}
