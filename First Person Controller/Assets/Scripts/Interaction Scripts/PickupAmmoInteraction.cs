using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupAmmoInteraction : Interaction
{
    
    public GunController.gun_type gun;
    public int amount;

    public override void interact(Pickup pickup)
    {
        int oldAmount = amount;
        amount = pickup.gunController.addAmmoToGun(gun,amount);
        if(amount == 0)
        {
            Destroy(gameObject);
        }else if(oldAmount == amount)
        {
            pickup.e_button_exception.flash_text(get_exception(0), 3);
        }
    }
}
