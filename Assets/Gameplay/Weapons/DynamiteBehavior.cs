using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteBehavior : MonoBehaviour
{
    [SerializeField] private GameObject DynamiteExplosionPrefab;
    private float dynamiteAttack;
    private float dynamiteCritRate;
    private float dynamiteTravelRange = 1f;
    private float dynamiteTravelSpeed;
    private Vector2 dynamiteDirectionUnit;
    private float distanceTraveled;
    private float blastRadius;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetUp(float dynamiteAttackDamage, float dynamiteCritStat, float dynamiteRange, float dynamiteSpeed, Vector2 dynamiteEnemyDirection, float dynamiteRadius)
    {
        transform.position = LevelControllerBehavior.levelController.playerBehavior.transform.position;
        dynamiteAttack = dynamiteAttackDamage;
        dynamiteCritRate = dynamiteCritStat;
        dynamiteTravelRange = dynamiteRange;
        dynamiteTravelSpeed = dynamiteSpeed;
        dynamiteEnemyDirection.Normalize();
        dynamiteDirectionUnit = dynamiteEnemyDirection;
        blastRadius = dynamiteRadius;

        if (dynamiteDirectionUnit.x < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        // current position add dynamiteEnemyDirection *t *dt *speed
        Vector3 dynamiteDirectionUnitV3 = new Vector3(dynamiteDirectionUnit.x, dynamiteDirectionUnit.y, 0);
        Vector3 displacement = dynamiteDirectionUnitV3 * Time.deltaTime * dynamiteTravelSpeed;

        transform.position += displacement;
        distanceTraveled += Mathf.Sqrt(Mathf.Pow(displacement.x,2) + Mathf.Pow(displacement.y,2));
        if (distanceTraveled >= dynamiteTravelRange)
        {
            GameObject tempGameObject = new GameObject();
            CircleCollider2D dynamiteCollider = tempGameObject.AddComponent<CircleCollider2D>();
            dynamiteCollider.transform.position = transform.position;
            dynamiteCollider.radius = blastRadius;
            Collider2D[] enemiesBlasted = new Collider2D[50];
            ContactFilter2D filter = new ContactFilter2D();
            filter.NoFilter();
            dynamiteCollider.OverlapCollider(filter, enemiesBlasted);
            foreach (Collider2D enemyBlasted in enemiesBlasted)
            {
                if (!enemyBlasted) {break;}
                EnemyBehavior hitBehavior = enemyBlasted.transform.gameObject.GetComponent<EnemyBehavior>();
                if (hitBehavior)
                {
                    Weapon.DamageEnemy(dynamiteAttack, dynamiteCritRate, hitBehavior);
                }
            }
            GameObject newProjectile = GameObject.Instantiate(DynamiteExplosionPrefab) as GameObject;
            newProjectile.transform.position = transform.position;
            Destroy(dynamiteCollider);
            Destroy(tempGameObject);
            Destroy(gameObject);
        }
        LevelControllerBehavior.SetYDependentOrderInLayer(gameObject);
    }
}