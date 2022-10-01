using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public static float CRIT_MULTIPLIER = 1.75f;

    public enum WeaponType {MELEE, RANGED};
    public static WeaponType currentWeapon;
    private static float _timeLastFired;

    // dictionary storing possible weapons
    public static Dictionary<WeaponType, Weapon> WeaponDict = new Dictionary<WeaponType, Weapon>()
    {
        
    };

    // Specific to each weapon
    protected float _cooldown, _baseAttack, _baseCritRate, _range;
    public bool offCooldown {get {return Time.time - _timeLastFired > _cooldown;}}

    public static void SwitchWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;
        _timeLastFired = -1f; // negative cooldown allows you to fire immediately after swap
    }

    public static void Fire(Vector3 targetLocation)
    {
        if (WeaponDict[currentWeapon].offCooldown)
        {
            WeaponDict[currentWeapon].Fire_WeaponSpecific(targetLocation);
            _timeLastFired = Time.time;
        }
    }

    // Only called once guaranteed off cooldown
    protected abstract void Fire_WeaponSpecific(Vector3 targetLocation);
}


//#region <================================== MELEE WEAPON ======================================>
public class Sword : Weapon
{
    /**
     * Sword
     * Attacks in an arc in the direction
     * 
     * _range determines radius of the circle that creates the arc
     * _angle determines angle of arc within the circle
     */
    private float angle;

    public Sword()
    {
        _cooldown = 10f;
        _baseAttack = 10f;
        _baseCritRate = .1f;
        _range = 1f;
    }

    protected override void Fire_WeaponSpecific(Vector3 targetLocation)
    {
        Vector3 playerPosition = LevelControllerBehavior.levelController.playerBehavior.transform.position;
        Vector3 targetDirection = (targetLocation - playerPosition).normalized;
        Vector3 counterclockwiseBound = EnemyBehavior.RotateVector(targetDirection, -angle / 2);
        Vector3 clockwiseBound = EnemyBehavior.RotateVector(targetDirection, angle / 2);
        Collider2D[] hits = Physics2D.OverlapCircleAll(playerPosition, _range);
        foreach (var enemy in hits) {
            if (inSector(playerPosition, counterclockwiseBound, clockwiseBound, enemy.transform.position)) {
                float baseDamage = _baseAttack * LevelControllerBehavior.levelController.playerBehavior.attack;
                enemy.gameObject.GetComponent<EnemyBehavior>().DamageEnemy(baseDamage * Random.value <= _baseCritRate ? CRIT_MULTIPLIER : 1);
            }
        }
    }

    private bool inSector(Vector3 playerPosition, Vector3 counterclockwiseBound, Vector3 clockwiseBound, Vector3 point)
    {
        return (
            -clockwiseBound.x*point.y + clockwiseBound.y*point.x <= 0 &&
            -counterclockwiseBound.x*point.y + counterclockwiseBound.y*point.x > 0 &&
            Vector3.Distance(playerPosition, point) <= _range * _range
        );
    }
}

//#region < ================================== RANGED WEAPON ======================================>

public class RangedWeapon : Weapon
{
    protected override void Fire_WeaponSpecific(Vector3 targetLocation)
    {

    }
}