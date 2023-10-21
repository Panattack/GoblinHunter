using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public Sword()
    {
        IDLE = new AnimationClip(Animator.StringToHash("IdleSword"), 0f);
        EQUIP = new AnimationClip(Animator.StringToHash("EquipSword"), 0.43f);
        UNEQUIP = new AnimationClip(Animator.StringToHash("UnequipSword"), 0.43f);
        ATTACK = new AnimationClip(Animator.StringToHash("AttackSword"), 0.94f);
    }

    public override PlayerWeapon getWeapon()
    {
        return PlayerWeapon.SWORD;
    }
}
