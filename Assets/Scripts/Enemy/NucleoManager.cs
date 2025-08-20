using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nucleo
{ 

    public abstract class NucleoManager : MonoBehaviour
    {
        public static int vidaActual = 2;

        public virtual void RecibirDaño(int daño)
        {
            vidaActual -= daño;
            if (vidaActual <= 0)
            {
                Vidanucleo.muertes++;
                Nucleos.vidaActual = Vidanucleo.vidaTotal + Vidanucleo.muertes;
                Destroy(gameObject);
            }
        }
    }
}
