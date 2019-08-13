using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    
    public GunInfo gun_info;
    
    public int ammoCapacity;
    public int ammo;
    protected AudioSource gunAudioSource;

    private void Start()
    {
        gunAudioSource = GetComponentInParent<AudioSource>();
    }
    public virtual void fire1(Ray ray)
    {

    }
    public virtual void fire2(Ray ray)
    {

    }

    public virtual float getAmmoFill()
    {
        return Mathf.Clamp(ammo / (float)ammoCapacity,0f,1f);
    }

    public virtual bool canFire()
    {
        return true;
    }

    public virtual int addAmmo(int amount)
    {
        ammo = ammo + amount;
        int leftover = Mathf.Max(ammo - ammoCapacity, 0);
        ammo -= leftover;
        return leftover;
    }

    protected void playFireSound(int index)
    {
        gunAudioSource.clip = gun_info.shootSound[index];
        gunAudioSource.Play();
    }

    protected void playEmptySound(int index)
    {
        gunAudioSource.clip = gun_info.emptySound[index];
        gunAudioSource.Play();
    }

    protected void playErrorSound(int index)
    {
        gunAudioSource.clip = gun_info.errorSound[index];
        gunAudioSource.Play();
    }
}
