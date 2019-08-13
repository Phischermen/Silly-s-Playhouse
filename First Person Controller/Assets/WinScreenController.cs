using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScreenController : MonoBehaviour
{
    CanvasGroup thanksForPlaying;
    CanvasGroup credits;

    private IEnumerator Start()
    {
        
        thanksForPlaying = transform.Find("Thanks").GetComponent<CanvasGroup>();
        credits = transform.Find("Credits").GetComponent<CanvasGroup>();
        thanksForPlaying.alpha = 0;
        credits.alpha = 0;
        for(float i = 0;i < 1; i += 0.02f)
        {
            thanksForPlaying.alpha = i;
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        for (float i = 1; i > 0; i -= 0.02f)
        {
            thanksForPlaying.alpha = i;
            yield return new WaitForSeconds(0.01f);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        for (float i = 0; i < 1; i += 0.02f)
        {
            credits.alpha = i;
            yield return new WaitForSeconds(0.02f);
        }
        yield return new WaitForSeconds(1f);
    }
}
