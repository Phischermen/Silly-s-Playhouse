using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Gun Info",fileName = "New gun info")]
public class GunInfo : ScriptableObject
{
    public new string name;
    public int ammo_capacity;
    public Sprite gun_sprite;
    public AudioClip[] shootSound;
    public AudioClip[] emptySound;
    public AudioClip[] errorSound;
}
