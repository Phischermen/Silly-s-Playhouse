using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GunController : MonoBehaviour
{
    public Pickup pickup_controller;
    public Image ammoBar;
    public Text ammo;
    public enum gun_type
    {
        match,
        debug,
        none
    }
    [HideInInspector]
    public Gun equipped_gun;
    [System.Serializable]
    public class ArsenalItem
    {
        public bool is_in_arsenal;
        public Gun gun;
    }
    
    public ArsenalItem[] arsenal = new ArsenalItem[2];
    [SerializeField]
    private gun_type starting_gun;
    [HideInInspector]
    public int equipped_gun_index;
    [HideInInspector]
    public int arsenal_size;
    public bool can_fire = true;
    public Image ui_gun_icon;
    private void Awake()
    {
        pickup_controller = GetComponentInParent<Pickup>();
    }
    void Start()
    {
        arsenal_size = evaluateArsenalSize();
        if(MyCharacterController.firstSpawnInScene)
            equip_gun(starting_gun);
    }

    // Update is called once per frame
    void Update()
    {
        can_fire = (pickup_controller.state != Pickup.pickup_state.carrying_object && equipped_gun_index != (int)gun_type.none &&equipped_gun.canFire() == true && Cursor.lockState == CursorLockMode.Locked);
        if (can_fire)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                Ray ray = new Ray(transform.parent.position, transform.parent.rotation * Vector3.forward);
                equipped_gun.fire1(ray);
            }
            if (Input.GetButtonDown("Fire2"))
            {
                Ray ray = new Ray(transform.parent.position, transform.parent.rotation * Vector3.forward);
                equipped_gun.fire2(ray);
            }
            ammoBar.fillAmount = Mathf.Lerp(ammoBar.fillAmount,equipped_gun.getAmmoFill(),0.1f);
            ammo.text = equipped_gun.ammo.ToString();
        }
        //Only allow weapon scrolling/ number selecting if arsenal is greater than 1
        if (arsenal_size > 0)
        {
            float scroll_delta = Input.GetAxis("Mouse ScrollWheel");
            if (scroll_delta != 0)
            {
                int old_gun_index = equipped_gun_index;
                while(true)
                {
                    do
                    {
                        //Find a gun that is in the arsenal
                        equipped_gun_index = (int)(equipped_gun_index + Mathf.Sign(scroll_delta));
                        if (equipped_gun_index < 0)
                        {
                            equipped_gun_index = arsenal.Length - 1;
                        }
                        else if (equipped_gun_index == arsenal.Length)
                        {
                            equipped_gun_index = 0;
                        }
                        if (equipped_gun_index == old_gun_index)
                        {
                            break;
                        }
                    }
                    while (arsenal[equipped_gun_index].is_in_arsenal == false);
                    //Equip the gun
                    if (equipped_gun_index == old_gun_index)
                    {
                        break;
                    }
                    else
                    {
                        equip_gun((gun_type)equipped_gun_index);
                        if (equipped_gun.canFire())
                        {
                            break;
                        }
                    }
                }
                
                
                //Debug.Log(equipped_gun.gun_info.name);
                
            }
        }
    }
    public void add_gun_to_arsenal(gun_type gun)
    {
        if (arsenal[(int)gun].is_in_arsenal == false)
        {
            arsenal[(int)gun].is_in_arsenal = true;
            arsenal_size += 1;
        }
    }
    public void equip_gun(gun_type gun)
    {
        equipped_gun_index = (int)gun;
        if (gun != gun_type.none)
        {
            showAmmoInfo();
            ArsenalItem new_item = arsenal[(int)gun];
            if (new_item.is_in_arsenal)
            {
                Gun old_gun = equipped_gun;
                equipped_gun = new_item.gun;
                //Swap which game objects are active
                if (old_gun != null)
                {
                    old_gun.gameObject.SetActive(false);
                }
                equipped_gun.gameObject.SetActive(true);
                ui_gun_icon.sprite = equipped_gun.gun_info.gun_sprite;
            }
        }
        else
        {
            if(equipped_gun != null)
            {
                equipped_gun.gameObject.SetActive(false);
            }
            hideAmmoInfo();
        }
        
    }
    private int evaluateArsenalSize()
    {
        int size = 0;
        foreach (ArsenalItem gun in arsenal)
        {
            size += (gun.is_in_arsenal) ? (1) : (0);
        }
        return size;
    }
    public void addGunToArsenalAndEquip(gun_type gun)
    {
        add_gun_to_arsenal(gun);
        equip_gun(gun);
    }
    public int addAmmoToGun(gun_type gun,int amount)
    {
        return arsenal[(int)gun].gun.addAmmo(amount);
    }
    private void hideAmmoInfo()
    {
        ammo.canvasRenderer.SetAlpha(0);
        ammoBar.canvasRenderer.SetAlpha(0);
        ui_gun_icon.canvasRenderer.SetAlpha(0);
    }
    private void showAmmoInfo()
    {
        ammo.canvasRenderer.SetAlpha(1);
        ammoBar.canvasRenderer.SetAlpha(1);
        ui_gun_icon.canvasRenderer.SetAlpha(1);
    }
}
