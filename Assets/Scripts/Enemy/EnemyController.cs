using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public abstract class EnemyController : MonoBehaviour
    {
        public int enemigoDamage = 1;
        public float velocidadMovimiento = 1f;
        public float radio = 20f;

        [Header("Enemigo Vida")]
        public int vidaMax = 2;
        public int _vidaActual;

        [SerializeField] protected GameObject objetoDrop;

        private void Start()
        {
            VidaInicial();
        }

        // Metodos
        public void VidaInicial()
        {
            _vidaActual = vidaMax;
        }

        public virtual void RecibirDaño(int daño)
        {
            _vidaActual -= daño;

            if (_vidaActual <= 0)
            {
                _vidaActual = 0;
                Morir();
            }
        }
        
        protected void ActualizarVida(int nuevaVida)
        {
            _vidaActual = nuevaVida;
        }

        

        protected virtual void Morir()
        {
            Destroy(gameObject);
            // Insertar animacion de muerte mas tarde...
        }
        
    }
}