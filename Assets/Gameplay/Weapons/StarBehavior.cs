using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehavior : MonoBehaviour
{
    private float starAttack;
    private float starCritRate;
    private float starTravelRange;
    private float starTravelSpeed;
    private Vector2 starDirectionUnit;
    private float distanceTraveled;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetUp(float starAttackDamage, float starCritStat, float starRange, float starSpeed, Vector2 starEnemyDirection)
    {
        starAttack = starAttackDamage;
        starCritRate = starCritStat;
        starTravelRange = starRange;
        starTravelSpeed = starSpeed;
        starEnemyDirection.Normalize();
        starDirectionUnit = starEnemyDirection;
    }
    // Update is called once per frame
    void Update()
    {
        // current position add starEnemyDirection *t *dt *speed
        Vector3 starDirectionUnitV3 = new Vector3(starDirectionUnit.x, starDirectionUnit.y, 0);
        Vector3 displacement = starDirectionUnitV3 * Time.deltaTime * starTravelSpeed;
        transform.position += displacement;
        distanceTraveled = Mathf.Sqrt(Mathf.Pow(displacement.x,2) + Mathf.Pow(displacement.y,2));
        if (distanceTraveled >= starTravelRange)
        {
            Destroy(gameObject);
        }
        LevelControllerBehavior.SetYDependentOrderInLayer(gameObject);
    }
}
