using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//HERE: got rid of using System.Math, it's Mathf instead and you already have access

public class MeleeEnemyBehavior : EnemyBehavior 
{
    public override void Attack()
    {
        if (cooldown <= 0) // attack ready
        {
            _playerBehavior.DamagePlayer(attack);
            Debug.Log(_playerBehavior.hp);
            cooldown = maxCD; // reset CD
        }   
    }

}