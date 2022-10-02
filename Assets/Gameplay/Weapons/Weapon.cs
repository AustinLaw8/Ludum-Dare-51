using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public static float CRIT_MULTIPLIER = 1.75f;

    public enum WeaponType {SWORD, STAR, DYNAMITE};

    // Weapon stats
    protected float _cooldown;
    protected float _baseAttack;
    protected float _baseCritRate;
    protected float _range;
    
    private float _cooldownTimer;
    public float getCooldownProportionCompleted {get { return Mathf.Min(_cooldownTimer / _cooldown, 1); } }

    protected Animator anim;

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        _cooldownTimer = _cooldown;
        WeaponSpecificSetup();
    }

    void Update()
    {
        update();
        _cooldownTimer += Time.deltaTime;
        if (offCooldown()) 
        {
            reset();
        }
    }

    public void Fire(Vector3 targetLocation)
    {
        if (offCooldown())
        {
            Fire_Weapon(targetLocation);
            _cooldownTimer = 0f;
        }
    }

    public bool offCooldown() { return _cooldownTimer >= _cooldown; } 

    protected abstract void Fire_Weapon(Vector3 targetLocation);

    public static void DamageEnemy(float baseDamage, float baseCritRate, EnemyBehavior enemyBehavior)
    {
        bool isCrit = (UnityEngine.Random.Range(0f,1f) < baseCritRate + LevelControllerBehavior.levelController.playerBehavior.critRate / 100f);
        float damageTaken = baseDamage * LevelControllerBehavior.levelController.playerBehavior.attack / 100f;
        if (isCrit)
        {
            damageTaken *= Weapon.CRIT_MULTIPLIER;
        }
        enemyBehavior.DamageEnemy(damageTaken);
    }

    public float getDamage() {
        PlayerBehavior player = LevelControllerBehavior.levelController.playerBehavior;
        float baseDamage = _baseAttack * player.attack / 100f;
        return Random.value <= (_baseCritRate + player.critRate) ? baseDamage * CRIT_MULTIPLIER : baseDamage;
    }

    protected virtual void update() {} 
    protected virtual void reset() {}
    protected virtual void WeaponSpecificSetup() {}
}

/*
    public Star()
    {
        _cooldown = .35f;
        _baseAttack = 3f;
        _baseCritRate = .2f;
        _range = 50f;
        _flySpeed = 10f;
    }
*/