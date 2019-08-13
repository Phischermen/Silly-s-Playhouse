using System.Collections;
using System.Collections.Generic;
using UnityEngine.Video;
using UnityEngine;

public class PickupMatchGunInteraction : Interaction
{
    public GameObject lights;
    public AudioSource startTrack;
    public VideoPlayer videoPlayer;
    private GunController gun_controller;
    public InputTutorial inputTutorial;
    public Animator wall;

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
    public override void interact(Pickup obj)
    {
        isBusy = true;
        gun_controller = GameObject.FindGameObjectWithTag("GunController").GetComponent<GunController>();
        gun_controller.addGunToArsenalAndEquip(GunController.gun_type.match);
        GetComponent<MeshRenderer>().enabled = false;
        MusicController.CurrentTrack = null;
        lights.SetActive(true);
        videoPlayer.Play();
        current_prompt = 1;
        obj.refresh_text();
        StartCoroutine("WaitForVideoThenSave");
        /*
        inputTutorial.TriggerTutorial();
        MusicController.CurrentTrack = startTrack;
        SaveContainer.AutoSave();
        wall.SetBool("drop", true);
        Destroy(gameObject);
        */
    }
    IEnumerator WaitForVideoThenSave()
    {
        yield return new WaitUntil(() => videoPlayer.isPlaying);
        yield return new WaitUntil(() => !videoPlayer.isPlaying);
        inputTutorial.TriggerTutorial();
        MusicController.CurrentTrack = startTrack;
        SaveContainer.AutoSave();
        wall.SetBool("drop", true);
        Destroy(gameObject);
    }
}
