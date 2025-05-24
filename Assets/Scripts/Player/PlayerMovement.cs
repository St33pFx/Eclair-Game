using System;
using System.Collections;
using System.Collections.Generic;
using Enemy;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // Vital Personake
    private int maxVida = 4;
    public int vidaActual;
    private bool _esInmune = false;
    
    // Movimiento
    public float speedMovement = 2f;
    private Vector2 moveDirection;
    private bool _isFacinRight = true;

    public static Vector2 _direccionApunta = Vector2.right;

    public static Vector2 _orientacionObjeto;
    public static float angulo;
    public static Quaternion rotacin;

    // Input Movimiento
    float horizontalMovement;
    float verticalMovement;
    
    private DamageFlash _damageFlash;

    // Referencias
    public static Rigidbody2D _rigidbody;

    private void Awake()
    {
        Debug.Log($"Hola amiguitos!");
        vidaActual = maxVida;
    }

    // Start is called before the first frame update
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _damageFlash = GetComponent<DamageFlash>();
    }

    // Update is called once per frame
    void Update()
    {
        InputMovement();

        // Girar personaje

        if (horizontalMovement < 0 && _isFacinRight == true)
        {
            FlipCharacter();
        }
        else if (horizontalMovement > 0 && _isFacinRight == false)
        {
            FlipCharacter();
        }
    }

    void FixedUpdate()
    {
        Move();
    }

    private void InputMovement()
    {
        // Moviento de personaje 

        horizontalMovement = Input.GetAxisRaw("Horizontal");
        verticalMovement = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector2(horizontalMovement, verticalMovement).normalized;

        angulo = MathF.Atan2(_direccionApunta.y, _direccionApunta.x) * Mathf.Rad2Deg;
        rotacin = Quaternion.Euler(0, 0, angulo - 90);

        Vector2 _direccion = new Vector2(horizontalMovement, verticalMovement);

        if (_direccion != Vector2.zero)
        {
        _direccionApunta = _direccion.normalized;

        }



    }

    private void Move()
    {
        _rigidbody.velocity = new Vector2(moveDirection.x * speedMovement, moveDirection.y * speedMovement);
    }

    private void FlipCharacter()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        _isFacinRight = !_isFacinRight;
    }

    public void RecibirDaño(int daño = 1)
    {
        vidaActual -= daño;
        if (vidaActual <= 0)
        {
            vidaActual = 0;
            this.gameObject.SetActive(false);
            return; 
        }

        _damageFlash.LlamarFlashDaño();
        StartCoroutine(Cooldown(1f));
    }
    
    private IEnumerator Cooldown(float segundos)
    {
        _esInmune = true;
        yield return new WaitForSeconds(segundos);
        _esInmune = false;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") && _esInmune == false)
        {
            RecibirDaño();
            Debug.Log($"Ahora tienes: {vidaActual}");
        }
    }
    
}
