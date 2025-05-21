using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public abstract class EnemyController : MonoBehaviour
    {
        public int vidaMax;
        public int enemigoDamage = 1;
        public float velocidadMovimiento = 1f;
        public float radio = 20f;

        public int _vidaActual;

        private void Start()
        {
            VidaInicial();
        }

        // Metodos
        public void VidaInicial()
        {
            _vidaActual = vidaMax;
        }

        protected void ActualizarVida(int nuevaVida)
        {
            _vidaActual = nuevaVida;
        }

        public void RecibirDaño(int daño)
        {
            _vidaActual -= daño;
            if (_vidaActual <= 0)
            {
                _vidaActual = 0;
                Morir();
            }
        }

        protected virtual void Morir()
        {
            Destroy(gameObject);
            // Insertar animacion de muerte mas tarde...
        }
        
    }
}