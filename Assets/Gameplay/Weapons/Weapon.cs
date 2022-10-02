using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public static float CRIT_MULTIPLIER = 1.75f;

    public enum WeaponType {SWORD, STAR};

    public Vector3 offset;

    // Weapon stats
    protected float _cooldown;
    protected float _baseAttack;
    protected float _baseCritRate;
    protected float _range;
    
    private float _cooldownTimer;
    public float getCooldownProportionCompleted {get { return Mathf.Max(_cooldownTimer / _cooldown, 1); } }

    protected Animator anim;

    void Start()
    {
        anim = this.gameObject.GetComponent<Animator>();
        _cooldownTimer = _cooldown;
    }

    void Update()
    {
        _cooldownTimer += Time.deltaTime;
    }

    public void Fire(Vector3 targetLocation)
    {
        if (offCooldown())
        {
            Fire_Weapon(targetLocation);
        }
    }

    public bool offCooldown() { return _cooldownTimer >= _cooldown; } 

    protected abstract void Fire_Weapon(Vector3 targetLocation);
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