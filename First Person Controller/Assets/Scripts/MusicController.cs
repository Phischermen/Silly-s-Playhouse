using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private static AudioSource currentTrack;
    private static AudioSource previousTrack;
    public static float trackVolume { get; private set; }
    public static AudioSource CurrentTrack {
        get
        {
            return currentTrack;
        }
        set
        {
            //Stop old track
            if (currentTrack != null)
                currentTrack.Stop();
            currentTrack = value;
            //Start new track
            if (currentTrack != null)
            {
                currentTrack.Play();
                trackVolume = currentTrack.volume;
            }
                
        }
    }
    
}
