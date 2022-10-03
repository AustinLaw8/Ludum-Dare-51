using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Star
 *      Throws a star that deals damage to all enemies it hits
 * 
 * Stats:
 *      - Damage:   3
 *      - Crit:     20%
 *      - CD:       .35
 * 
 * Star Specific Stats:
 *      - Flight speed: 20f
 *      - Flight time: 2.5 (50 range)
 */
public class Star : Weapon
{
    [SerializeField] private GameObject StarProjectilePrefab;
    [SerializeField] private AudioClip THROW_SFX;
    private float _flySpeed;

    protected override void Fire_Weapon(Vector3 targetLocation)
    {
        Transform _playerTransform = LevelControllerBehavior.levelController.playerBehavior.transform;
        GameObject newProjectile = GameObject.Instantiate(StarProjectilePrefab) as GameObject;  
        Vector2 starPath = targetLocation - _playerTransform.position;   
        newProjectile.GetComponent<StarBehavior>().SetUp(_baseAttack, _baseCritRate, _flySpeed, starPath);
        GetComponent<SpriteRenderer>().enabled = false;
        LevelControllerBehavior.levelController.SFX(THROW_SFX);
    }

    public Star()
    {
        _cooldown = .35f;
        _baseAttack = 3f;
        _baseCritRate = .2f;
        _flySpeed = 20f;
    }

    protected override void reset() 
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }

    protected override void WeaponSpecificSetup()
    {
        transform.localPosition *= 1.5f;
    }
}