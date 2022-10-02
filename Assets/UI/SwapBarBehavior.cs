using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Notably, this script also manages the cooldown bar

public class SwapBarBehavior : MonoBehaviour
{
    public Image swapBarInner;
    public TMPro.TMP_Text timerNumber;

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
        timerNumber.text = "1";
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelControllerBehavior.levelController._levelActive)
        {
            float dimlessTimeTilSwap = LevelControllerBehavior.levelController.dimensionlessClampedTimeTilNextSwap;
            // Update inner health bar fill ratio
            swapBarInner.fillAmount = dimlessTimeTilSwap;
            
            // Update timer text
            timerNumber.text = "<b>" + (Mathf.FloorToInt(dimlessTimeTilSwap * 10f) + 1).ToString() + "</b>";
        }
    }
}
