using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [TextArea(5, 10)]
    public string tutorialHeader;
    
    protected TextFader tutorialUITextFader;
    protected TutorialTextController tutorialTextController;

    protected bool triggered = false;
    public bool ready = false;
    public bool done = false;
    public Tutorial nextTutorial;

    // Start is called before the first frame update
    public virtual void Start()
    {
        GetUIComponents(GameObject.FindGameObjectWithTag("Player"));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (triggered == false && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            TriggerTutorial();

        }
    }
    public virtual void TriggerTutorial()
    {
        triggered = true;
        tutorialTextController.pushTutorial(this);
    }
    public void GetUIComponents(GameObject Player)
    {
        GameObject tutorialUI = Player.transform.Find("Crosshair UI/Tutorial Text").gameObject;
        tutorialUITextFader = tutorialUI.GetComponent<TextFader>();
        tutorialTextController = tutorialUI.GetComponent<TutorialTextController>();
    }
}
