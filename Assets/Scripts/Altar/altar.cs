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
        if (llenarRitual >= 5)
        {
            Matar = true;
            StartCoroutine(WaitFiveSeconds());
        }
    }
    IEnumerator WaitFiveSeconds()
    {
        yield return new WaitForSeconds(10);
        llenarRitual = 0;
        Matar = false;
    }
}
