using System;
using System.Collections;
using Enemy;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Vida
    [SerializeField] private int _maxVida = 5;
    public int vidaActual;
    private bool _esInmune = false;

    // Movimiento
    public float speedMovement = 2f;
    public Vector2 moveDirection;
    private bool _isFacinRight = true;
    public bool canMove = true;

    public static Vector2 _direccionApunta = Vector2.right;
    public static float angulo;
    public static Quaternion rotacin;

    float horizontalMovement;
    float verticalMovement;

    private DamageFlash _damageFlash;
    private Animator _animator;
    private HealthSystem _healthSystem;

    [Header("Armas")]
    [SerializeField] private WeaponShoot _weaponShoot;
    [SerializeField] private CruzArrojadiza cruzArrojadiza;   // <- MISMA instancia que usa el player

    public static Rigidbody2D _rigidbody;

    [SerializeField] private ExperienceManager xp;
    [SerializeField] private GameObject _panelMuerte;

    [SerializeField] private AudioSource fuentePasos;
    [SerializeField] private AudioClip sonidoPasos;

    [Header("Blood Points")]
    private int _NuevoNivel = 50;
    private int _bloodPoints;

    [Header("Dash")]
    [SerializeField] bool dashEnabled = false;
    [SerializeField] float dashSpeed = 14f;
    [SerializeField] float dashTime = 0.15f;
    [SerializeField] float dashCooldown = 0.5f;

    bool dashing;
    bool dashCD;
    Vector2 lastMoveDir = Vector2.right;

    // Estado de cruz
    private bool cruzDesbloqueada = false;

    public void EnableDash(bool on) => dashEnabled = on;

    private void Awake()
    {
        vidaActual = _maxVida;
        _animator = GetComponent<Animator>();
        _healthSystem = FindObjectOfType<HealthSystem>();
    }

    private void Start()
    {
        if (!_weaponShoot)
            _weaponShoot = GameObject.FindGameObjectWithTag("Arma").GetComponent<WeaponShoot>();

        _rigidbody = GetComponent<Rigidbody2D>();
        _damageFlash = GetComponent<DamageFlash>();

        // Asegura que la cruz arranca deshabilitada
        if (cruzArrojadiza)
        {
            cruzArrojadiza.enabled = false;
            cruzArrojadiza.canShoot = false;
        }

    }

    void Update()
    {
        InputMovement();
        ReproducirPasos();

        if (moveDirection.sqrMagnitude > 0.01f)
            lastMoveDir = moveDirection.normalized;

        if (dashEnabled && canMove && !dashing && !dashCD && Input.GetKeyDown(KeyCode.LeftShift))
            StartCoroutine(DashRoutine());

        if (horizontalMovement < 0 && _isFacinRight) FlipCharacter();
        else if (horizontalMovement > 0 && !_isFacinRight) FlipCharacter();
    }

    void FixedUpdate() => Move();

    public void InputMovement()
    {
        if (!canMove) return;

        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontalMovement, verticalMovement).normalized;

        angulo = MathF.Atan2(_direccionApunta.y, _direccionApunta.x) * Mathf.Rad2Deg;
        rotacin = Quaternion.Euler(0, 0, angulo - 90);

        Vector2 _direccion = new Vector2(horizontalMovement, verticalMovement);
        if (_direccion != Vector2.zero)
            _direccionApunta = _direccion.normalized;

        _animator.SetBool("idle", moveDirection == Vector2.zero);
    }

    private void Move()
    {
        if (dashing)
            _rigidbody.velocity = lastMoveDir * dashSpeed;
        else
            _rigidbody.velocity = moveDirection * speedMovement;
    }

    private void FlipCharacter()
    {
        var s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
        _isFacinRight = !_isFacinRight;
    }

    public void RecibirDaño(int daño = 1)
    {
        if (_esInmune) return;

        vidaActual = Mathf.Clamp(vidaActual - daño, 0, _maxVida);
        _healthSystem.ActualizarCorazones(vidaActual);

        if (vidaActual <= 0)
        {
            _panelMuerte.SetActive(true);
            GameManager.Pausa();
            gameObject.SetActive(false);
            return;
        }

        _damageFlash.LlamarFlashDaño();
        StartCoroutine(Cooldown(1f));
    }

    public void RecibirVida(int vida = 1)
    {
        vidaActual = Mathf.Clamp(vidaActual + vida, 0, _maxVida);
        _healthSystem.ActualizarCorazones(vidaActual);
    }

    private IEnumerator Cooldown(float s)
    {
        _esInmune = true;
        yield return new WaitForSeconds(s);
        _esInmune = false;
    }

    private void OnTriggerStay2D(Collider2D c)
    {
        if (c.CompareTag("Enemy") && !_esInmune)
            RecibirDaño();

        if (c.CompareTag("noPaso"))
        {
            if (_weaponShoot) _weaponShoot._puedeDisparar = false;
            if (cruzArrojadiza) cruzArrojadiza.canShoot = false;
        }
    }

    private void OnTriggerExit2D(Collider2D c)
    {
        if (!c.CompareTag("noPaso")) return;

        if (_weaponShoot) _weaponShoot._puedeDisparar = true;

        // Si la cruz está desbloqueada, vuelve a permitir disparo al salir de noPaso
        if (cruzArrojadiza && cruzDesbloqueada)
            cruzArrojadiza.canShoot = true;
    }

    // ---- Blood points
    public void AgregarBloodPoints(int bloodpoints)
    {
        xp.AgregarExperiencia(bloodpoints);
        bloodpoints += _bloodPoints;
        if (bloodpoints == _NuevoNivel)
        {
            // …
        }
    }

    private bool _estaCaminando;
    private void ReproducirPasos()
    {
        _estaCaminando = moveDirection != Vector2.zero;

        if (_estaCaminando && !fuentePasos.isPlaying)
        {
            fuentePasos.clip = sonidoPasos;
            fuentePasos.Play();
        }
        else if (!_estaCaminando && fuentePasos.isPlaying)
        {
            fuentePasos.Stop();
        }
    }


    public void SetCruzDesbloqueada(bool on)
    {
        cruzDesbloqueada = on;
        if (cruzArrojadiza)
            cruzArrojadiza.canShoot = on;   // habilita el tiro de la cruz inmediatamente
        Debug.Log($"[PlayerMovement] CruzDesbloqueada = {on}");
    }


    private IEnumerator DashRoutine()
    {
        dashing = true;
        Vector2 dir = lastMoveDir.sqrMagnitude < 0.01f ? (_isFacinRight ? Vector2.right : Vector2.left) : lastMoveDir;

        float t = 0f;
        while (t < dashTime)
        {
            _rigidbody.velocity = dir * dashSpeed;
            t += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

        dashing = false;
        _rigidbody.velocity = Vector2.zero;

        dashCD = true;
        yield return new WaitForSeconds(dashCooldown);
        dashCD = false;
    }

 

}
