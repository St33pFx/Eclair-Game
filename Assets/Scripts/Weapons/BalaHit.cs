using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;

public class BalaHit : MonoBehaviour
{
    private int _danio = 1;
    private bool _haColisionadp = false;
    

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            if(_haColisionadp) return;
            Rigidbody2D rbEnemy = other.GetComponent<Rigidbody2D>();
            EnemyController enemigo = other.GetComponent<EnemyController>();
            if (enemigo != null)
            {
                Vector2 retroceso = (other.transform.position - transform.position).normalized;
                float fuerzaRetroceso = 600f;
                rbEnemy.AddForce(retroceso * fuerzaRetroceso);
                enemigo.RecibirDaño(_danio);
                _haColisionadp = true;
                Destroy(this.gameObject);
            }
        }
    }

}
