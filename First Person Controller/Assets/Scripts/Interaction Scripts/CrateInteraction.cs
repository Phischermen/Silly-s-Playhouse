using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateInteraction : Interaction
{
    public override void interact(Pickup obj)
    {
        if (Physics.CheckBox(transform.position + new Vector3(0.5f, 1.26f, 0.5f), new Vector3(0.25f, 0.25f, 0.25f),Quaternion.identity,LayerMask.GetMask("InteractSolid")))
        {
                obj.e_button_exception.flash_text(get_exception(1), 3);
        }
        else
        {
            
            obj.pickup_object(gameObject);
        }
        
    }
}
