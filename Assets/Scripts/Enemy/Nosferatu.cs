
using Enemy;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;


public class Nosferatu : EnemyController
{
    [SerializeField] private Rigidbody2D enemyRb;
    
    [SerializeField] private AudioClip SonidoImpactos;

    private DamageFlash _damageFlash;
    private Nosferatu _nosferatu;
    private bool isFacingRight = true;
    private Transform playerTransform;

    public float despawnDistance = 20f;
    
    private EnemySpawner_2 spawner;

    [SerializeField] private Vector2 direccion;


    [SerializeField] private Collider2D spawnArea;
    public Transform player;

    public void ReferenciarSpawn(EnemySpawner_2 spwn)
    {
        spawner = spwn;
    }
    
    

    private void Awake()
    {
        spawner = FindObjectOfType<EnemySpawner_2>();
        if (spawner == null) Debug.LogError("[Nosferatu] No hay EnemySpawner_2 en escena.");

        spawnArea = spawner != null ? spawner.playableArea : null; // <-- AQUÍ se asigna
        if (spawnArea == null) Debug.LogError("[Nosferatu] spawnArea NULL. Asigna 'Playable Area' en el Spawner.");


        FindObjectOfType<EnemySpawner_2>();
        enemyRb = GetComponent<Rigidbody2D>();
        playerTransform = GameObject.FindWithTag("Player")?.transform;
        _damageFlash = GetComponent<DamageFlash>();

        if (playerTransform == null)
        {
            Debug.LogError("Ups, transform del player no ha sido asignado!");
        }

    }

    private void Start()
    {
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    void Update()
    {
        GirarEnemigo();

        

        if (Vector2.Distance(transform.position, player.position) >= despawnDistance)
        {
            ReturnEnemy();
        }
    }
     
    //private void FixedUpdate()
    //{
    //    if (playerTransform == null) return;
        
    //    // Direccion crea un vector y NuevaDireccion crea el movimiento 
    //    direccion = ((Vector2)(playerTransform.position - transform.position)).normalized;
    //    Vector2 nuevaDireccion = (Vector2)transform.position + direccion * velocidadMovimiento * Time.fixedDeltaTime; 
        
    //    // If para evitar que se glitchee el nosferatu
    //    // Vector distance devuelve la distancia entre puntos a y b
    //    if (Vector2.Distance((Vector2)playerTransform.position, (Vector2)transform.position) > 0.1f)
    //    {
    //        // MovePosition mueve el rigid body a una posicion especificada, acuerdate papui 
    //        enemyRb.MovePosition(nuevaDireccion);
    //    }
    //}

    public void EstablecerSpawn(EnemySpawner_2 spwn)
    {
        spawner = spwn;
        
    }

    public override void RecibirDaño(int daño)
    {
        AudioManager.Instance.PlaySonido(SonidoImpactos);
        _damageFlash.LlamarFlashDaño();
        base.RecibirDaño(daño);
        Debug.Log($"Recibiendo {daño}");
    }

    protected override void Morir()
    {
        if (spawner != null)
        {
            //spawner.EliminarEnemigo(this.gameObject);
        }
        Instantiate(objetoDrop, transform.position, Quaternion.identity);
        
        EnemySpawner_2 es = FindObjectOfType<EnemySpawner_2>();
        es.OnEnemyKilled();

        Destroy(this.gameObject);
    }

    private void GirarEnemigo()
    {
        if (direccion.x < 0 && isFacingRight)
        {
            FlipCharacter();
        }

        else if (direccion.x > 0 && !isFacingRight)
        {
            FlipCharacter();
        }
}
    
    private void FlipCharacter()
    {
        Vector3 currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        isFacingRight = !isFacingRight;
    }

    void ReturnEnemy()
    {

        EnemySpawner_2 es = FindObjectOfType<EnemySpawner_2>();

        Vector3 spawnPos;
        int intentor = 0;
        const int maxIntentos = 10;

        do
        {
            Vector3 randomOffset = es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
            spawnPos = player.position + randomOffset;

            intentor++;
        }
        while (!spawnArea.OverlapPoint(spawnPos) && intentor < maxIntentos);

        transform.position = spawnPos;
        //transform.position = player.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
    }

    //void MoverEnemigos()
    //{
    //    var pt = spawnPoint.relativeSpawnPoints[Random.Range(0, spawnPoint.relativeSpawnPoints.Count)];
    //    Vector2 posicion = pt.position;

    //    Collider2D collision = Physics2D.OverlapCircle(posicion, spawnPoint.radioChequeo, spawnPoint.layerPermitidaParaSpawn);
    //    if (collision == null)
    //    {
    //        EnemySpawner_2 es = FindObjectOfType<EnemySpawner_2>();
    //        transform.position = player.position + es.relativeSpawnPoints[Random.Range(0, es.relativeSpawnPoints.Count)].position;
    //    }
    //}

}
