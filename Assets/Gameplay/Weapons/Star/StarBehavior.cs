using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehavior : MonoBehaviour
{
    HashSet<EnemyBehavior> enemiesAlreadyHit = new HashSet<EnemyBehavior>();
    private float starAttack;
    private float starCritRate;
    private float starTravelSpeed;
    private float starStunDuration;
    private Vector2 starDirectionUnit;
    private float distanceTraveled;
    private int enemiesHit, pierce;

    public void SetUp(float starAttackDamage, float starCritStat, float starSpeed, Vector2 starEnemyDirection, int passedPierce, float passedStunDuration)
    {
        transform.position = LevelControllerBehavior.levelController.playerBehavior.transform.position;
        starAttack = starAttackDamage;
        starCritRate = starCritStat;
        starTravelSpeed = starSpeed;
        starEnemyDirection.Normalize();
        starDirectionUnit = starEnemyDirection;
        enemiesHit = 0;
        pierce = passedPierce;
        starStunDuration = passedStunDuration;
    }
    // Update is called once per frame
    void Update()
    {
        if (LevelControllerBehavior.levelController._gameOver)
        {
            Destroy(gameObject);
        }
        
        if (LevelControllerBehavior.levelController._levelActive)
        {
            if (distanceTraveled >= Weapon.WEAPON_MAX_RANGE || enemiesHit >= pierce)
            {
                Destroy(gameObject);
            }

            // current position add starEnemyDirection *t *dt *speed
            Vector3 starDirectionUnitV3 = new Vector3(starDirectionUnit.x, starDirectionUnit.y, 0);
            Vector3 displacement = starDirectionUnitV3 * Time.deltaTime * starTravelSpeed;

            RaycastHit2D[] starHits = Physics2D.CircleCastAll(transform.position, 0.37f, starDirectionUnitV3, displacement.magnitude);
            foreach (RaycastHit2D starHit in starHits)
            {
                if (enemiesHit >= pierce)
                {
                    Destroy(gameObject);
                }
                EnemyBehavior hitBehavior = starHit.transform.gameObject.GetComponent<EnemyBehavior>();
                if (hitBehavior && !enemiesAlreadyHit.Contains(hitBehavior))
                {
                    Weapon.DamageEnemy(starAttack, starCritRate, hitBehavior);
                    hitBehavior.StunEnemy(starStunDuration);
                    enemiesAlreadyHit.Add(hitBehavior);
                    enemiesHit++;
                }
            }

            transform.position += displacement;
            distanceTraveled += Mathf.Sqrt(Mathf.Pow(displacement.x,2) + Mathf.Pow(displacement.y,2));
            LevelControllerBehavior.SetYDependentOrderInLayer(gameObject);
        }
    }
}
