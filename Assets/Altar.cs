using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class Altar : MonoBehaviour
{
   public static int llenarRitual = 0;
    public static bool Matar = false; 
    void Update()
    {
        if(llenarRitual >= 5)
        {
            Matar = true; 
           StartCoroutine (WaitFiveSeconds());
        }
    }
    IEnumerator WaitFiveSeconds()
    {
        yield return new WaitForSeconds(10);
        llenarRitual = 0;
        Matar = false; 
    }
}
