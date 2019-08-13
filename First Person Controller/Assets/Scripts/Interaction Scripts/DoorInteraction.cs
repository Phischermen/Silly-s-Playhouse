using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : Interaction
{
    public BoxCollider obstructionDetection;
    private LayerMask mask = new LayerMask();
    [SerializeField]
    private bool locked;
    [SerializeField]
    private bool open;
    public bool isLocked { get; private set; }
    public bool isOpen { get; private set; }
    [HideInInspector]
    public Animator animator;
    private void Start()
    {
        mask = LayerMask.GetMask("Player","InteractSolid");
        animator = GetComponent<Animator>();
        if (open)
        {
            swingDoor(true, false);
        }
        twistLock(locked);
    }
    private void Update()
    {
        isBusy = animator.IsInTransition(0);
    }
    public void twistLock(bool locked)
    {
        isLocked = locked;
    }
    public void swingDoor(bool open,bool animated = true)
    {
        animator.SetBool("open", open);
        if (!animated)
        {
            if(open)
                animator.Play("DoorOpen", 0, 1f);
            else
                animator.Play("DoorClose", 0, 1f);
        }
        isOpen = open;
        current_prompt = (isOpen) ? (1) : (0);
    }
    
    public override void interact(Pickup obj)
    {
        if (!isBusy)
        {
            if (isLocked && !isOpen)
            {
                obj.e_button_exception.flash_text(get_exception(0),3);
            }
            else if (!isLocked && !isOpen)
            {
                swingDoor(true);
            }
            else if (isOpen)
            {
                Vector3 offset = transform.rotation * obstructionDetection.center;
                Vector3 pos = obstructionDetection.transform.position + offset;
                //Debug.DrawRay(pos, offset,Color.red,5f);
                
                if (!Physics.CheckBox(pos, obstructionDetection.size * 0.5f, transform.rotation, mask))
                {
                    swingDoor(false);
                }
                else
                {
                    obj.e_button_exception.flash_text(get_exception(1), 3);
                }
            }
        }
    }
}
