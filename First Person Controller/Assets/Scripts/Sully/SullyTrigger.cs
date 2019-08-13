using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SullyTrigger : MonoBehaviour
{
    protected AudioSource source;
    bool triggered;
    private void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    private void OnDestroy()
    {
        if(MusicController.CurrentTrack != null)
            MusicController.CurrentTrack.volume = (MusicController.trackVolume);
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            source.Play();
            MusicController.CurrentTrack.volume = (MusicController.trackVolume*0.5f);
            triggered = true;
            StartCoroutine("PlayAndDelete");
        }
    }
    protected virtual IEnumerator PlayAndDelete()
    {
        yield return new WaitUntil(()=>!source.isPlaying);
        Destroy(gameObject);
    }
}
