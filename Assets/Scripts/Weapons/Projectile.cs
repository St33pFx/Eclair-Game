using System.Collections.Generic;
using Enemy;
using Nucleo;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage = 1f;   // se setea al instanciar
    [SerializeField] private int maxTargets = 1;
    [SerializeField] private float lifeTime = 5f;

    private int _hits = 0;
    private bool _haColisionado = false;
    private readonly HashSet<EnemyController> _hitEnemies = new();

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    // Llamado por el arma al instanciar
    public void InitDamage(float dmg, int maxTargetsHit)
    {
        damage = Mathf.Max(0f, dmg);
        maxTargets = Mathf.Max(1, maxTargetsHit);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ---------- 1) NÚCLEO PRIMERO ----------
        if (other.CompareTag("Nucleo"))
        {
            if (_haColisionado) return;

            // Busca componentes en este GO o en el padre (por si el collider está en un hijo)
            var rbNucleo = other.attachedRigidbody ?? other.GetComponentInParent<Rigidbody2D>();
            var nucleo = other.GetComponentInParent<NucleoManager>() ?? other.GetComponent<NucleoManager>();

            if (nucleo != null)
            {
                Vector2 retroceso = (other.transform.position - transform.position).normalized;
                float fuerzaRetroceso = 600f; // ajusta al gusto
                if (rbNucleo) rbNucleo.AddForce(retroceso * fuerzaRetroceso);

                nucleo.RecibirDaño(1);
                _haColisionado = true;
                Destroy(gameObject);
            }
            return; // ya procesamos este impacto
        }

        // ---------- 2) ENEMIGOS ----------
        var enemy = other.GetComponentInParent<EnemyController>() ?? other.GetComponent<EnemyController>();
        if (enemy == null) return;

        // evita múltiples hits al mismo enemigo con el mismo proyectil
        if (_hitEnemies.Contains(enemy)) return;
        _hitEnemies.Add(enemy);

        // aplica daño (acepta fracciones 0.5)
        if (Mathf.Abs(damage - Mathf.Round(damage)) < 0.0001f)
            enemy.RecibirDaño(Mathf.RoundToInt(damage));
        else
            enemy.RecibirDañoFloat(damage);

        _hits++;
        if (_hits >= maxTargets)
            Destroy(gameObject);
    }
}
