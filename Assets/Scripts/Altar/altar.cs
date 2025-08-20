using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Altar : MonoBehaviour
{
    public static int llenarRitual = 0;
    public static bool Matar = false;
    private EnemySpawner_2 es;

    private void Start()
    {
        es = FindObjectOfType<EnemySpawner_2>();
    }

    void Update()
    {

    }
}
