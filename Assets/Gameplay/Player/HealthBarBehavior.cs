using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Notably, this script also manages the cooldown bar

public class HealthBarBehavior : MonoBehaviour
{
    public Image healthBarInner;
    public Image coolDownBarInner;

    private PlayerBehavior _behaviorPlayer;
    private Transform _thisTransform;

    // Start is called before the first frame update
    void Start()
    {
        // Setup bars
        healthBarInner.type = Image.Type.Filled;
        healthBarInner.fillMethod = Image.FillMethod.Horizontal;
        healthBarInner.fillOrigin = (int)Image.OriginHorizontal.Left;
        healthBarInner.fillAmount = 1f;

        coolDownBarInner.type = Image.Type.Filled;
        coolDownBarInner.fillMethod = Image.FillMethod.Horizontal;
        coolDownBarInner.fillOrigin = (int)Image.OriginHorizontal.Left;
        coolDownBarInner.fillAmount = 1f;

        // Cache data references
        _behaviorPlayer = LevelControllerBehavior.levelController.playerBehavior;
        _thisTransform = transform;
    }

    // Update is called once per frame
    void Update()
    {
        // Update health bar fill ratio
        if (LevelControllerBehavior.levelController._levelActive)
        {
            // Update inner health bar fill ratio   
            healthBarInner.fillAmount = Mathf.Clamp(_behaviorPlayer.hp / _behaviorPlayer.maxHp, 0f, 1f);
            coolDownBarInner.fillAmount = LevelControllerBehavior.levelController.currentWeapon.getCooldownProportionCompleted;
        }
    }

    // called in Update() of PlayerBehavior. Prevents health bar from flipping around. Can't put in own update due to call order (causes noticable 1-frame delay)
    public void FlipToMaintainBillboard()
    {
        _thisTransform.localScale = new Vector3(-1f * _thisTransform.localScale.x, _thisTransform.localScale.y, _thisTransform.localScale.z);
    }
}
