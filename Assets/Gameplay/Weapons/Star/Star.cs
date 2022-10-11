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
    private float _flySpeed, _stunDuration;
    private int _pierce;

    protected override void Fire_Weapon(Vector3 targetLocation)
    {
        Transform _playerTransform = LevelControllerBehavior.levelController.playerBehavior.transform;
        GameObject newProjectile = GameObject.Instantiate(StarProjectilePrefab) as GameObject;  
        Vector2 starPath = targetLocation - _playerTransform.position;   
        newProjectile.GetComponent<StarBehavior>().SetUp(_baseAttack, _baseCritRate, _flySpeed, starPath, _pierce, _stunDuration);
        GetComponent<SpriteRenderer>().enabled = false;
        LevelControllerBehavior.levelController.SFX(THROW_SFX);
    }

    public Star()
    {
        _cooldown = .333f;
        _baseAttack = 1f;
        _baseCritRate = 0.0f;
        _flySpeed = 20f;
        _pierce = 100000;
        _stunDuration = 0.5f;
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