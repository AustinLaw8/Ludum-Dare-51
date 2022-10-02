using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windslash : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float growthRate;
    [SerializeField] private float knockback;

    private Vector3 origin;
    private Vector3 direction;
    private float attack;
    private float critRate;
    private HashSet<GameObject> hits;    

    // Start is called before the first frame update
    void Start()
    {
        hits = new HashSet<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromOrigin = Vector3.Distance(origin, this.transform.position);
        if (distanceFromOrigin >= Weapon.WEAPON_MAX_RANGE) Destroy(this.gameObject);
        float nextSize = (1 + distanceFromOrigin) * growthRate;
        this.transform.localScale = new Vector3(nextSize / 4, nextSize, this.transform.localScale.z);
        this.transform.position += direction * speed * Time.deltaTime;
    }

    public void init(Vector3 pos, Vector3 loc, float attack, float critRate) {
        bool facingLeft = LevelControllerBehavior.levelController.playerBehavior.facingLeft;

        this.direction = (loc - pos).normalized;
        float angle = this.direction.y < 0 ? 360 - (Vector3.Angle(Vector3.right, this.direction)) : angle = (Vector3.Angle(Vector3.right, this.direction));
        this.transform.position = pos;
        this.origin = this.transform.position;
        this.transform.Rotate(new Vector3(0f, 0f, angle));
        this.attack = attack;
        this.critRate = critRate;
    }

    void OnTriggerEnter2D(Collider2D hit) {
        GameObject hitObj = hit.gameObject;
        if (hitObj.tag == "Enemy" && !hits.Contains(hitObj)) {
            hitObj.GetComponent<EnemyBehavior>().DamageEnemy(Weapon.getDamage(attack, critRate));
            hitObj.transform.position+= this.direction * knockback;
            hits.Add(hitObj);
        }
    }
}
