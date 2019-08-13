using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    GameObject character;
    GameObject camera;
    GameObject winUI;
    private void OnTriggerEnter(Collider other)
    {
        winUI = Resources.Load<GameObject>("Win");
        character = GameObject.FindGameObjectWithTag("Player");
        camera = character.GetComponent<MyCharacterController>().my_camera.gameObject;
        Instantiate(winUI, camera.transform.position, camera.transform.rotation);
        Destroy(character);
    }
    
}
