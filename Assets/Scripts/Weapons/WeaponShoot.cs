using System.Collections;
using Cinemachine;
using UnityEngine;

public class WeaponShoot : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private GameObject proyectilPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private PlayerUpgrades upgrades;

    private CinemachineImpulseSource impulseSource;

    [Header("Disparo")]
    [SerializeField] private float velocidadBala = 10f;
    [SerializeField] private float baseCooldown = 0.5f;
    [SerializeField] private float minCooldown = 0.2f;
    [SerializeField] public bool _puedeDisparar = true;

    [Header("Debug")]
    [SerializeField] private float currentCooldown;

    [Header("Audio")]
    [SerializeField] private AudioSource fuenteDisparo;
    [SerializeField] private AudioClip sonidoDisparo;

    private void Awake()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>();
        if (!upgrades) upgrades = GetComponentInParent<PlayerUpgrades>();
        if (!upgrades) upgrades = FindObjectOfType<PlayerUpgrades>();
    }

    private void Update()
    {
        if (GameManager.juegoPausado) return;
        if (!_puedeDisparar) return;

        if (Input.GetMouseButtonDown(0))
            Disparar();
    }

    public void Disparar()
    {
        float speedMult = (upgrades ? Mathf.Max(1f, upgrades.projectileSpeedMult) : 1f);
        float fireMult = (upgrades ? Mathf.Max(1f, upgrades.fireRateMult) : 1f);

        GameObject bala = Instantiate(proyectilPrefab, shootPoint.position, shootPoint.rotation);

        // --- NUEVO: setear daño y perforación en el proyectil ---
        var proj = bala.GetComponent<Projectile>();
        if (proj)
        {
            float dmg = upgrades ? upgrades.GetProjectileDamage() : 1f;   // 1.0 + 0.5*steps
            int pier = upgrades ? upgrades.GetMaxTargetsHit() : 1;    // 1 + pierceExtra
            proj.InitDamage(dmg, pier);
        }

        // velocidad
        var rb = bala.GetComponent<Rigidbody2D>();
        if (rb) rb.velocity = shootPoint.right * (velocidadBala * speedMult);

        if (impulseSource) CameraShakeManager.instance.CameraShake(impulseSource);
        if (sonidoDisparo && fuenteDisparo) fuenteDisparo.PlayOneShot(sonidoDisparo);

        currentCooldown = Mathf.Max(minCooldown, baseCooldown / fireMult);
        _puedeDisparar = false;
        StartCoroutine(CooldownRutina(currentCooldown));
    }

    private IEnumerator CooldownRutina(float cd)
    {
        yield return new WaitForSeconds(cd);
        _puedeDisparar = true;
    }

    public void SetUpgrades(PlayerUpgrades u) => upgrades = u;
}
