using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomResetterInteraction : Interaction
{
    public ChildrenResetter Controller;
    public static bool canReset = true; 
    public override void interact(Pickup pickup)
    {
        if (canReset)
        {
            GetComponentInChildren<Animator>().Play("ButtonPress");
            Controller.ResetRoom();
            pickup.characterController.dealDamage(2);
            canReset = false;
        }
        else
        {
            pickup.e_button_exception.flash_text(get_exception(0), 1);
        }
        
    }
}
