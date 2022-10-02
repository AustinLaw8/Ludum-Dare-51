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
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetUp(float starAttackDamage, float starCritStat, float starRange, float starSpeed )
    {
        starAttack = starAttackDamage;
        starCritRate = starCritStat;
        starTravelRange = starRange;
        starTravelSpeed = starSpeed;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
