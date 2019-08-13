using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTextController : MonoBehaviour
{
    public Tutorial activeTutorial;
    private TextFader textFader;
    public Stack<Tutorial> tutorialStack = new Stack<Tutorial>();
    public bool isBusy;
    Coroutine coroutine;

    private void Start()
    {
        textFader = GetComponent<TextFader>();
    }
    private void Update()
    {
        if(activeTutorial != null && activeTutorial.done == true)
        {
            popTutorial();
        }
    }
    public void pushTutorial(Tutorial tutorial)
    {
        tutorialStack.Push(tutorial);
        coroutine = StartCoroutine(switchTutorial(tutorial));
    }
    public void popTutorial()
    {
        tutorialStack.Pop();
        Tutorial tutorial = null;
        if (tutorialStack.Count > 0)
        {
            tutorial = tutorialStack.Peek();
        }
        coroutine = StartCoroutine(switchTutorial(tutorial));
    }
    IEnumerator switchTutorial(Tutorial newTutorial)
    {
        if (activeTutorial != null)
        {
            if (activeTutorial.done == true)
            {
                Destroy(activeTutorial.gameObject);
            }
            else
            {
                activeTutorial.gameObject.SetActive(false);
            }
        }
        textFader.fade_out_text();
        yield return new WaitUntil(isTextFadedOut);
        if(newTutorial != null)
        {
            textFader.fade_in_text(newTutorial.tutorialHeader);
            activeTutorial = newTutorial;
            activeTutorial.ready = true;
            activeTutorial.gameObject.SetActive(true);
        }
        StopCoroutine(coroutine);
        yield return null;
    }

    bool isTextFadedOut()
    {
        return textFader.state == TextFader.prompt_state.faded_out;
    }
}
