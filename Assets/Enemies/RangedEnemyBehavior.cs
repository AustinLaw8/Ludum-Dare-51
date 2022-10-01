using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//HERE: got rid of using System.Math, it's Mathf instead and you already have access

public class RangedEnemyBehavior : EnemyBehavior 
{

    public void Attack()
    {
        _playerBehavior.DamagePlayer(attack);
    }
}