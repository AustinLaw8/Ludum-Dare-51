using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Star : Weapon
{
    [SerializeField] private GameObject StarProjectilePrefab;
    private float _flySpeed;
    //convert 3v to 2v create new v2  |  targetx-playerx , targety,playery
    protected override void Fire_Weapon(Vector3 targetLocation)
    {
        Transform _playerTransform = LevelControllerBehavior.levelController.playerBehavior.transform;
        GameObject newProjectile = GameObject.Instantiate(StarProjectilePrefab, _playerTransform) as GameObject;  
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