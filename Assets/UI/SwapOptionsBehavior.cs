using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapOptionsBehavior : MonoBehaviour
{
    [SerializeField] private Image _frameLeft, _frameRight;
    [SerializeField] private Image _weaponLeft, _weaponRight;
    [SerializeField] private TMPro.TMP_Text _textLeft, _textRight;
    [SerializeField] Texture2D _starIcon, _swordIcon, _fanIcon, _dynamiteIcon;
    public enum SelectionOption {LEFT, RIGHT};
    public bool leftSideSelected;
    // Start is called before the first frame update
    void Start()
    {
        SelectOption(SelectionOption.LEFT);
    }

    public void SelectOption(SelectionOption option)
    {
        _frameLeft.enabled = option == SelectionOption.LEFT;
        _frameRight.enabled = option == SelectionOption.RIGHT;
        leftSideSelected = option == SelectionOption.LEFT;
    }

    public void UpdateChoiceVisuals(Weapon.WeaponType wepL, Weapon.WeaponType wepR,
                                    LevelControllerBehavior.NextSwapData.BoostableStat statL, LevelControllerBehavior.NextSwapData.BoostableStat statR,
                                    int magL, int magR)
    {

    }
}
