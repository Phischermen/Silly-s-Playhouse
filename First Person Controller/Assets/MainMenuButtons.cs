using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    Animator anim;

    public void StartGame()
    {
        anim = GetComponentInParent<Animator>();
        anim.SetBool("Fade", true);
        StartCoroutine("StartGameAfterFade");
    }
    public IEnumerator StartGameAfterFade()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("Level1");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
