using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WakingUp : MonoBehaviour
{
    void Start()
    {
        MusicController.CurrentTrack = GetComponent<AudioSource>(); 
    }
}
