using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    // Movimiento
    public float speedMovement = 2f;
    private Vector2 moveDirection;
    private bool _isFacinRight = true;

    // Input Movimiento
    float horizontalMovement;
    float verticalMovement;

    // Referencias
    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
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
    
}
