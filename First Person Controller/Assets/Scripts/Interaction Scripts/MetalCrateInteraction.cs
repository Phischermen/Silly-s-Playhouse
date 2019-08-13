using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalCrateInteraction : Interaction
{
    public int weight = 2;

    public override void interact(Pickup obj)
    {
        obj.e_button_exception.GetComponent<Flash>().flash_text(get_exception(0),3);
    }
}
