using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    public HashSet<GameObject> hits;
    private Sword _parentSword;

    void Start()
    {
        hits = new HashSet<GameObject>();
        _parentSword = this.transform.parent.GetComponent<Sword>();
    }

    void OnTriggerEnter2D(Collider2D hit) {
        GameObject hitObj = hit.gameObject;
        if (hitObj.tag == "Enemy" && !hits.Contains(hitObj)) {
            hits.Add(hitObj);
            hitObj.GetComponent<EnemyBehavior>().DamageEnemy(_parentSword.getDamage());
            Vector3 enemyPos = hitObj.transform.position, playerPos = LevelControllerBehavior.levelController.playerBehavior.transform.position;
            Vector3 knockbackDir = new Vector3(enemyPos.x - playerPos.x, enemyPos.y - playerPos.y, 0f);
            knockbackDir.Normalize();
            hitObj.transform.position += knockbackDir * _parentSword.knockback;
        }
    }
}
