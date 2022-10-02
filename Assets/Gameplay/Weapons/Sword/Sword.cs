using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Sword
 *      Attacks in an arc in the target direction
 * 
 * Stats:
 *      - Damage:   10
 *      - Crit:     10%
 *      - CD:       .25
 * 
 * Sword Specific Stats:
 *      - Angle of attack:  90 degrees
 *      -
 */
public class Sword : Weapon
{
    private static string SWING_ANIMATION_NAME = "SwordSwing";
    private static float SWING_ANIMATION_TIME;
    private float _angle;
    private bool attacking;
    private BoxCollider2D bx;
    
    public Sword()
    {
        _cooldown = .25f;
        _baseAttack = 10f;
        _baseCritRate = .1f;
        _angle = 90f;
    }

    protected override void Fire_Weapon(Vector3 targetLocation)
    {
        targetLocation = new Vector3(targetLocation.x, targetLocation.y, 0f);
        Vector3 playerPosition = LevelControllerBehavior.levelController.playerBehavior.transform.position;
        Vector3 targetDirection = targetLocation - playerPosition;
        bool facingLeft = LevelControllerBehavior.levelController.playerBehavior.facingLeft;
        Vector3 vec = facingLeft ? Vector3.left : Vector3.right;
        float angle = targetLocation.y < playerPosition.y ? 360 - (Vector3.Angle(vec, targetDirection)) : angle = (Vector3.Angle(vec, targetDirection));
        Vector3 pivotPoint = new Vector3(
                LevelControllerBehavior.PLAYER_RADIUS * Mathf.Cos(Mathf.Deg2Rad * angle),
                LevelControllerBehavior.PLAYER_RADIUS * Mathf.Sin(Mathf.Deg2Rad * angle),
                0f
        );
        this.transform.localPosition = pivotPoint;
        this.transform.Rotate(new Vector3(0f, 0f, (_angle / 2 + angle) - 90));

        attacking = true;
        bx.enabled = true;
        anim.Play(SWING_ANIMATION_NAME);
    }

    protected override void WeaponSpecificSetup()
    {
        bx = this.transform.GetChild(0).GetComponent<BoxCollider2D>();
        bx.enabled = false;
    }

    protected override void update()
    {
        if(attacking)
        {
            bx.transform.Rotate(new Vector3 (0f, 0f, -90f / _cooldown * Time.deltaTime));
        }
    }

    protected override void reset()
    { 
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName(SWING_ANIMATION_NAME))
        {
            this.transform.localPosition = new Vector3(
                    LevelControllerBehavior.PLAYER_CENTER.x + LevelControllerBehavior.PLAYER_RADIUS,
                    LevelControllerBehavior.PLAYER_CENTER.y,
                    0f
            );
        }
        bx.gameObject.GetComponent<SwordController>().hits.Clear();
        this.transform.rotation = Quaternion.identity;
        bx.transform.rotation = Quaternion.identity;
        bx.enabled = false;
        attacking = false;
    }
}