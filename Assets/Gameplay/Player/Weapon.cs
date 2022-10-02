using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon
{
    public static float CRIT_MULTIPLIER = 1.75f;

    public enum WeaponType {SWORD, STAR};
    public static WeaponType currentWeapon;
    public static Weapon currentWeaponObject {get {return WeaponDict[currentWeapon];}}
    private static float _timeLastFired;

    // dictionary storing possible weapons
    public static Dictionary<WeaponType, Weapon> WeaponDict = new Dictionary<WeaponType, Weapon>()
    {
        { WeaponType.SWORD, new Sword() }
    };

    // Specific to each weapon
    protected float _cooldown, _baseAttack, _baseCritRate, _range;
    public float getCooldown() { return _cooldown; }
    
    // 1 when off cooldown, 0 when ability recently used 
    public float getCooldownProportionCompleted {get {return Mathf.Clamp((Time.time - _timeLastFired) / _cooldown, 0f, 1f);}}
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
    private float _angle;

    public Sword()
    {
        _cooldown = 1f;
        _baseAttack = 10f;
        _baseCritRate = .1f;
        _range = 5f;
        _angle = 90f;
    }

    protected override void Fire_WeaponSpecific(Vector3 targetLocation)
    {
        Debug.Log("Sword attack");
    //     Vector3 playerPosition = LevelControllerBehavior.levelController.playerBehavior.transform.position;
    //     Vector3 targetDirection = (targetLocation - playerPosition).normalized;
    //     Vector3 counterclockwiseBound = EnemyBehavior.RotateVector(targetDirection, -_angle / 2);
    //     Vector3 clockwiseBound = EnemyBehavior.RotateVector(targetDirection, _angle / 2);
    //     Debug.Log($"{playerPosition}, {targetDirection}, {counterclockwiseBound}, {clockwiseBound}");
    //     Collider2D[] hits = Physics2D.OverlapCircleAll(playerPosition, _range);
    //     foreach (var enemy in hits) {
    //         if (inSector(playerPosition, counterclockwiseBound, clockwiseBound, enemy.transform.position)) {
    //             float baseDamage = _baseAttack * LevelControllerBehavior.levelController.playerBehavior.attack;
    //             // enemy.gameObject.GetComponent<EnemyBehavior>().DamageEnemy(baseDamage * Random.value <= _baseCritRate ? CRIT_MULTIPLIER : 1);
    //             Debug.Log("Enemy hit with Sword");
    //         }
    //     }
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

public class Star : Weapon
{
    [SerializeField] private GameObject StarProjectilePrefab;
    private float _flySpeed;
    private Vector2 _targetLocation;
    //convert 3v to 2v create new v2  |  targetx-playerx , targety,playery
    protected override void Fire_WeaponSpecific(Vector3 targetLocation)
    {
        //LevelControllerBehavior.levelController.playerBehavior.transform.position;

        //GameObject newProjectile = GameObject.Instantiate(StarProjectilePrefab, /* insert spawn position here */) as GameObject;
        
    }
    public Star()
    {
        _cooldown = .35f;
        _baseAttack = 3f;
        _baseCritRate = .2f;
        _range = 50f;
        _flySpeed = 10f;
    }
}