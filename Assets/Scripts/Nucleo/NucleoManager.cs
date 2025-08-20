using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Nucleo
{

    public abstract class NucleoManager : MonoBehaviour
    {
        public static int vidaActual = 2;
        private AudioSource carne;

        private void Start()
        {
            carne = GetComponent<AudioSource>();
        }

        public virtual void RecibirDaño(int daño)
        {

            if (carne != null)
            {
                carne.Play();
            }
            vidaActual -= daño;
            if (vidaActual <= 0)
            {
                Vidanucleo.muertes++;
                Nucleos.vidaActual = Vidanucleo.vidaTotal + Vidanucleo.muertes;
                GetComponent<NucleoDestruible>()?.Destruir();

            }
        }
    }
}