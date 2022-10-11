using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Fan
 *      Fires a windslash in an cone in the target direction.
 *      The windslash pushes enemies back
 * 
 * Stats:
 *      - Damage:   15
 *      - Crit:     15%
 *      - CD:       .5
 * 
 * Fan Specific Stats:
 */
public class Fan : Weapon
{
    private static Vector3 FAN_OFFSET = new Vector3(0f,-.35f,0f);
    private static string SWING_ANIMATION_NAME = "FanSwing";
    [SerializeField] private AudioClip SWING_SFX;
    [SerializeField] private GameObject WINDSLASH_PREFAB;

    protected override void Fire_Weapon(Vector3 targetLocation)
    {
        Transform _playerTransform = LevelControllerBehavior.levelController.playerBehavior.transform;
        GameObject newProjectile = GameObject.Instantiate(WINDSLASH_PREFAB);  
        newProjectile.GetComponent<Windslash>().init(_playerTransform.position, targetLocation, _baseAttack, _baseCritRate);
        anim.Play("FanSwing");
        LevelControllerBehavior.levelController.SFX(SWING_SFX);
    }

    public Fan()
    {
        _cooldown = .15f;
        _baseAttack = 1.6f;
        _baseCritRate = 0.0f;
    }

    protected override void reset() 
    {
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(SWING_ANIMATION_NAME))
        {
            anim.Play("FanIdle");
        } 
    }
    
    protected override void WeaponSpecificSetup()
    {
        this.transform.position += FAN_OFFSET;
    }
}