using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrotInteraction : Interaction
{
    public Interaction parrot;

    public override string get_prompt(int index = -1)
    {
        return parrot.get_prompt();
    }
    public override void interact(Pickup obj)
    {
        parrot.interact(obj);
    }
}
