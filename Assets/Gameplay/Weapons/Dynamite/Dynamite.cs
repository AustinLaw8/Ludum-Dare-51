using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Dynamite : Weapon
{
    [SerializeField] private GameObject DynamiteProjectilePrefab;
    [SerializeField] private AudioClip FIRE_SFX;

    private float _flySpeed;
    private float _baseRadius;
    protected override void Fire_Weapon(Vector3 targetLocation)
    {
        Transform _playerTransform = LevelControllerBehavior.levelController.playerBehavior.transform;
        GameObject newProjectile = GameObject.Instantiate(DynamiteProjectilePrefab) as GameObject;  
        Vector2 dynamitePath = targetLocation - _playerTransform.position;   
        newProjectile.GetComponent<DynamiteBehavior>().SetUp(_baseAttack, _baseCritRate, Mathf.Min(_range, Mathf.Sqrt(Mathf.Pow(targetLocation.x - _playerTransform.position.x, 2) + Mathf.Pow(targetLocation.y - _playerTransform.position.y, 2))), _flySpeed, dynamitePath, _baseRadius);
        GetComponent<SpriteRenderer>().enabled = false;
        LevelControllerBehavior.levelController.SFX(FIRE_SFX, .17f);
    }

    public Dynamite()
    {
        _cooldown = .8f;
        _baseAttack = 19f;
        _baseCritRate = 0.0f;
        _range = 20f;
        _flySpeed = 5f;
        _baseRadius = 3f;
    }

    protected override void reset() 
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
}