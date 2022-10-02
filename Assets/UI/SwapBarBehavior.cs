using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Notably, this script also manages the cooldown bar

public class SwapBarBehavior : MonoBehaviour
{
    public Image swapBarInner;

    private PlayerBehavior _behaviorPlayer;
    private Transform _thisTransform;

    // Start is called before the first frame update
    void Start()
    {
        // Setup bars
        swapBarInner.type = Image.Type.Filled;
        swapBarInner.fillMethod = Image.FillMethod.Horizontal;
        swapBarInner.fillOrigin = (int)Image.OriginHorizontal.Left;
        swapBarInner.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // Update health bar fill ratio
        if (LevelControllerBehavior.levelController._levelActive)
        {
            // Update inner health bar fill ratio
            swapBarInner.fillAmount = LevelControllerBehavior.levelController.dimensionlessClampedTimeTilNextSwap;
        }
    }
}
