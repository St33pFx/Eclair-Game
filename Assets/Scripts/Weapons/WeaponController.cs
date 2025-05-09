using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponController : MonoBehaviour
{
    /// <summary>
    /// Creacion de base para todas las armas.
    /// </summary>

    [Header("Estadisticas del arma")]

    
    public GameObject _prefab;
    public float _danio;
    // public float _velocidad;
    public float _cooldownDuracion;
    public float _cooldownActual;
    public int _perforacion;

    

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _cooldownActual = _cooldownDuracion;
    }

    private void Update()
    {
        _cooldownActual -= Time.deltaTime;

        if (_cooldownActual <= 0f)
        {
            Attack();
        }
    }

    protected abstract void Attack();

    

    
}
