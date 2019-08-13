using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroyEffect : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(transform.parent.gameObject, GetComponent<ParticleSystem>().main.duration);
    }
    
}
