using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class DeadCharacterController : MonoBehaviour
{
    SaveContainer saveContainer;
    // Start is called before the first frame update
    void Start()
    {
        saveContainer = GameObject.FindGameObjectWithTag("SaveContainer").GetComponent<SaveContainer>();    
    }

    // Update is called once per frame
    void Update()
    {
        //Restarting scene
        if (Input.anyKeyDown)
        {
            saveContainer.LoadLatest();
            Destroy(gameObject);
        }
    }
}
