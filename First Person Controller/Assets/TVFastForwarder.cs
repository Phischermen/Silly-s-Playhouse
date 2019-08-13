using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class TVFastForwarder : Interaction
{
    public VideoPlayer TV;
    
    public override void interact(Pickup pickup)
    {
        if (TV.isPlaying)
        {
            TV.playbackSpeed = 8;
            TV.GetComponent<AudioSource>().pitch = 2;
        }
        else
        {
            pickup.e_button_exception.flash_text(get_exception(0), 1);
        }
            
    }
}
