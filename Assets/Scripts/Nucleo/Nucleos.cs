using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Nucleo;

public class Nucleos : NucleoManager
{

    public override void RecibirDaño(int daño)
    {
        base.RecibirDaño(daño);
        Debug.Log($"Recibiendo {daño}");
    }
}