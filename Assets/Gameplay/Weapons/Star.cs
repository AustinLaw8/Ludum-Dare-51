using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Star : Weapon
{
    [SerializeField] private GameObject StarProjectilePrefab;
    private float _flySpeed;
    protected override void Fire_Weapon(Vector3 targetLocation)
    {
        Transform _playerTransform = LevelControllerBehavior.levelController.playerBehavior.transform;
        GameObject newProjectile = GameObject.Instantiate(StarProjectilePrefab) as GameObject;  
        Vector2 starPath = targetLocation - _playerTransform.position;   
        newProjectile.GetComponent<StarBehavior>().SetUp(_baseAttack, _baseCritRate, _range, _flySpeed, starPath);
    }
    public Star()
    {
        _cooldown = .35f;
        _baseAttack = 3f;
        _baseCritRate = .2f;
        _range = 50f;
        _flySpeed = 20f;
    }
}