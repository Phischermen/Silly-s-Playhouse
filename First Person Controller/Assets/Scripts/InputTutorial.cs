using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InputTutorial : Tutorial
{
    public enum inputType
    {
        button,
        axisPos,
        axisNeg,
        axisBoth,
    }
    [System.Serializable]
    public class InputItem
    {
        public string inputName;
        public inputType type;
        [TextArea(5, 10)]
        public string textDisplay;
    }
    public InputItem[] inputItems;
    private bool[] inputPressed;

    public override void Start()
    {
        base.Start();
        inputPressed = new bool[inputItems.Length];
        for (int i = 0; i < inputPressed.Length; ++i)
        {
            inputPressed[i] = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
        if (ready)
        {
            string message = tutorialHeader + "\n";
            bool hasAllInputBeenPressed = true;
            for (int i = 0;i < inputItems.Length; ++i)
            {
                switch (inputItems[i].type)
                {
                    case inputType.axisPos:
                        if (Input.GetAxis(inputItems[i].inputName) > 0f)
                        {
                            inputPressed[i] = true;
                        }
                        break;
                    case inputType.axisNeg:
                        if (Input.GetAxis(inputItems[i].inputName) < 0f)
                        {
                            inputPressed[i] = true;
                        }
                        break;
                    case inputType.axisBoth:
                        if (Input.GetAxis(inputItems[i].inputName) != 0f)
                        {
                            inputPressed[i] = true;
                        }
                        break;
                    case inputType.button:
                        if (Input.GetButtonDown(inputItems[i].inputName))
                        {
                            inputPressed[i] = true;
                        }
                        break;
                }
                if(inputPressed[i] == false)
                {
                    message += inputItems[i].textDisplay + "\n";
                    hasAllInputBeenPressed = false;
                }
                else
                {
                    message += "<color=#b2e3ff>" + inputItems[i].textDisplay + "</color>\n";
                }
            }
            if(hasAllInputBeenPressed == true)
            {
                done = true;
                if(nextTutorial != null)
                {
                    nextTutorial.TriggerTutorial();
                }
                
            }
            tutorialUITextFader.uiText.text = message;
        }
            
        
    }

    
}
