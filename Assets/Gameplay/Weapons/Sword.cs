using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Sword weapon
 * Attacks in an arc in the target direction
 * 
 * _range determines radius of the circle that creates the arc
 * _angle determines angle of arc within the circle
 */
public class Sword : Weapon
{
    private static string SWORD_ATTACK_ANIMATION_NAME = "SwordSwing";
    private float _angle;
    private float swingTime=1f;

    public Sword()
    {
        _cooldown = 1f;
        _baseAttack = 10f;
        _baseCritRate = .1f;
        _range = 5f;
        _angle = 90f;
    }

    void Update()
    {

    }
    
    protected override void Fire_Weapon(Vector3 targetLocation)
    {
        // anim.Play(SWORD_ATTACK_ANIMATION_NAME);
        Vector3 playerPosition = LevelControllerBehavior.levelController.playerBehavior.transform.position;
        Vector3 targetDirection = (targetLocation - playerPosition).normalized;
        Vector3 counterclockwiseBound = (EnemyBehavior.RotateVector(targetDirection, -_angle / 2)).normalized;
        Vector3 clockwiseBound = (EnemyBehavior.RotateVector(targetDirection, _angle / 2)).normalized;
        Debug.Log($"{playerPosition}, {targetDirection}, {counterclockwiseBound}, {clockwiseBound}");
        this.transform.RotateAround(this.transform.position, Vector3.forward, _angle);

    //     Collider2D[] hits = Physics2D.OverlapCircleAll(playerPosition, _range);
    //     foreach (var enemy in hits) {
    //         if (inSector(playerPosition, counterclockwiseBound, clockwiseBound, enemy.transform.position)) {
    //             float baseDamage = _baseAttack * LevelControllerBehavior.levelController.playerBehavior.attack;
    //             // enemy.gameObject.GetComponent<EnemyBehavior>().DamageEnemy(baseDamage * Random.value <= _baseCritRate ? CRIT_MULTIPLIER : 1);
    //             Debug.Log("Enemy hit with Sword");
    //         }
    //     }
    }

    // private bool inSector(Vector3 playerPosition, Vector3 counterclockwiseBound, Vector3 clockwiseBound, Vector3 point)
    // {
    //     return (
    //         -clockwiseBound.x*point.y + clockwiseBound.y*point.x <= 0 &&
    //         -counterclockwiseBound.x*point.y + counterclockwiseBound.y*point.x > 0 &&
    //         Vector3.Distance(playerPosition, point) <= _range * _range
    //     );
    // }
}