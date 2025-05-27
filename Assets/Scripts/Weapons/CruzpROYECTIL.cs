using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class CruzpROYECTIL : MonoBehaviour
{
    private bool _haColision = false;
    private int _cruzDamage = 2;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(_haColision) return;

            EnemyController enemigo = other.GetComponent<EnemyController>();
            if (enemigo != null)
            {
                _haColision = true;
                enemigo.RecibirDaño(_cruzDamage);
                Destroy(this.gameObject);
            }


        }
    }
}
