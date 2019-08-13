using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentInteraction : Interaction
{
    [SerializeField]
    private bool is_locked;
    [SerializeField]
    private bool is_open;
    [HideInInspector]
    public Animator animator;
    private void Start()
    {
        
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        isBusy = animator.IsInTransition(0);
    }
    public void unlock_door()
    {
        is_locked = false;
        animator.SetBool("open", is_open);

    }
    public void lock_door()
    {
        is_locked = true;

    }
    public override void interact(Pickup obj)
    {
        if (!isBusy)
        {
            if (is_locked && !is_open)
            {
                obj.e_button_exception.flash_text(get_exception(0), 3);
            }
            else if (!is_locked && !is_open)
            {
                animator.SetBool("open", true);
                is_open = true;
                isBusy = true;
                current_prompt = 1;
            }
            else if (is_open)
            {
                animator.SetBool("open", false);
                is_open = false;
                current_prompt = 0;
            }
        }
    }
}
