using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestruir : MonoBehaviour
{

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
