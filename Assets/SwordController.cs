using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordController : MonoBehaviour
{
    private HashSet<GameObject> hits;

    void Start()
    {
        hits = new HashSet<GameObject>();
    }

    void OnTriggerEnter2D(Collider2D hit) {
        GameObject hitObj = hit.gameObject;
        if (hitObj.tag == "Enemy" && !hits.Contains(hitObj)) {
            hits.Add(hitObj);
            hitObj.GetComponent<EnemyBehavior>().DamageEnemy(this.transform.parent.GetComponent<Weapon>().getDamage());
        }
    }
    // void OnCollisionStay2D(Collision2D hit) {
    //     Debug.Log("hit urself");
    //     GameObject hitObj = hit.gameObject;
    //     if (!hits.Contains(hitObj)) {
    //         hits.Add(hitObj);
    //         hitObj.GetComponent<EnemyBehavior>().DamageEnemy(this.transform.parent.GetComponent<Weapon>().getDamage());
    //     }
    // }
    // void OnCollisionExit2D(Collision2D hit) {
    //     Debug.Log("hit urself");
    //     GameObject hitObj = hit.gameObject;
    //     if (!hits.Contains(hitObj)) {
    //         hits.Add(hitObj);
    //         hitObj.GetComponent<EnemyBehavior>().DamageEnemy(this.transform.parent.GetComponent<Weapon>().getDamage());
    //     }
    // }
}
