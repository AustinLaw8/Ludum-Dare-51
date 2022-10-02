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

    protected override void Fire_Weapon(Vector3 targetLocation)
    {
        // anim.Play(SWORD_ATTACK_ANIMATION_NAME);
        targetLocation = new Vector3(targetLocation.x, targetLocation.y, 0f);
        Vector3 playerPosition = LevelControllerBehavior.levelController.playerBehavior.transform.position;
        Vector3 targetDirection = targetLocation - playerPosition;
        // Vector3 counterclockwiseBound = (EnemyBehavior.RotateVector(targetDirection, -_angle / 2)).normalized;
        // Vector3 clockwiseBound = (EnemyBehavior.RotateVector(targetDirection, _angle / 2)).normalized;
        float angle;
        Vector3 vec = LevelControllerBehavior.levelController.playerBehavior.facingLeft ? Vector3.left : Vector3.right;
        if (targetLocation.y < playerPosition.y) {
            angle = 360 - (Vector3.Angle(vec, targetDirection));
        } else {
            angle = (Vector3.Angle(vec, targetDirection));
        }
        Vector3 pivotPoint = new Vector3(
                LevelControllerBehavior.PLAYER_RADIUS * Mathf.Cos(Mathf.Deg2Rad * angle),
                LevelControllerBehavior.PLAYER_RADIUS * Mathf.Sin(Mathf.Deg2Rad * angle),
                0f
        ) + offset;
        this.transform.localPosition = pivotPoint;
        Debug.Log($"{angle}");
        this.transform.RotateAround(pivotPoint, Vector3.forward, -(90 - (_angle / 2 + angle)));
    }
    //     Collider2D[] hits = Physics2D.OverlapCircleAll(playerPosition, _range);
    //         if (inSector(playerPosition, counterclockwiseBound, clockwiseBound, enemy.transform.position)) {
    //     foreach (var enemy in hits) {
    //             float baseDamage = _baseAttack * LevelControllerBehavior.levelController.playerBehavior.attack;
    //             // enemy.gameObject.GetComponent<EnemyBehavior>().DamageEnemy(baseDamage * Random.value <= _baseCritRate ? CRIT_MULTIPLIER : 1);
    //             Debug.Log("Enemy hit with Sword");
    //         }
    //     }

    protected override void reset()
    {
        this.transform.localPosition = new Vector3(
                LevelControllerBehavior.PLAYER_CENTER.x + LevelControllerBehavior.PLAYER_RADIUS,
                LevelControllerBehavior.PLAYER_CENTER.y,
                0f
        ) + offset;
        this.transform.rotation = Quaternion.identity;
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