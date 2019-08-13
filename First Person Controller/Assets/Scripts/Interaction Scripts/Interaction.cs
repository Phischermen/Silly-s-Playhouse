using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
    
    public int current_prompt = 0;
    public PromtAndExceptionInfo prompts_and_exceptions;
    public bool isBusy = false;
    public virtual void interact(Pickup pickup)
    {
        return;
    }
    public virtual string get_prompt(int index = -1)
    {
        if(index == -1)
        {
            return prompts_and_exceptions.prompts[current_prompt];
        }
        else
        {
            return prompts_and_exceptions.prompts[index];
        }
    }
    public virtual string get_exception(int index)
    {
        return prompts_and_exceptions.exceptions[index];
    }
    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    */
}
