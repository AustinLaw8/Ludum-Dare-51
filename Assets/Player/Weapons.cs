using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public enum WeaponType {MELEE, RANGED};
    public static WeaponType currentWeapon;
    private static float _timeLastFired;
    public static bool offCooldown {get {return Time.time - _timeLastFired > _cooldown;}}

    // public static IDictionary<Weapon>()

    // Specific to each weapon
    protected float _cooldown, _baseAttack, _baseCritRate;

    public void SwitchWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
        _timeLastFired = -1f; // negative cooldown allows you to fire immediately after swap
    }

    public void Fire()
    {
        if (offCooldown)
        {
            Fire_WeaponSpecific();
            _timeLastFired = Time.time;
        }
    }
    // Only called once guaranteed off cooldown
    protected abstract void Fire_WeaponSpecific();
}
