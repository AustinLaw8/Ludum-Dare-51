using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamiteExplosionBehavior : MonoBehaviour
{
    public void DeleteWhenAnimationDoneEvent()
    {
        Destroy(gameObject);
    }
}