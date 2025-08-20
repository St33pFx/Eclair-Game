using System;
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

        // --- NUEVO: acumulador para daños fraccionarios (0.5, etc.) ---
        private float _damageAccumulator = 0f;

        private void Start()
        {
            VidaInicial();
        }

        public void VidaInicial()
        {
            _vidaActual = vidaMax;
        }

        // Daño entero (como ya tenías)
        public virtual void RecibirDaño(int daño)
        {
            _vidaActual -= daño;

            if (_vidaActual <= 0)
            {
                _vidaActual = 0;
                Morir();
            }
        }

        // --- NUEVO: acepta daño fraccionario (e.g., 0.5). 
        // Acumula y aplica en puntos de vida enteros.
        public virtual void RecibirDañoFloat(float daño)
        {
            _damageAccumulator += Mathf.Max(0f, daño);
            int entero = Mathf.FloorToInt(_damageAccumulator);
            if (entero > 0)
            {
                _damageAccumulator -= entero;
                RecibirDaño(entero);
            }
        }

        public virtual void ActualizarVida(int nuevaVida)
        {
            vidaMax = nuevaVida;

            if (_vidaActual > vidaMax)
                _vidaActual = vidaMax;
            else
                _vidaActual = nuevaVida;
        }

        protected virtual void Morir()
        {
            Destroy(gameObject);
        }
    }
}
