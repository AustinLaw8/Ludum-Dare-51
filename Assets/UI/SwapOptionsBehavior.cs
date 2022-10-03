using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapOptionsBehavior : MonoBehaviour
{
    [SerializeField] private Image _frameLeft, _frameRight;
    [SerializeField] private Image _weaponLeft, _weaponRight;
    [SerializeField] private TMPro.TMP_Text _textLeft, _textRight;
    [SerializeField] Sprite _starIcon, _swordIcon, _fanIcon, _dynamiteIcon;
    [SerializeField] private TMPro.TMP_Text _statsText, _scoreText;
    public enum SelectionOption {LEFT, RIGHT};
    public bool leftSideSelected;
    // Start is called before the first frame update
    void Start()
    {
        SelectOption(SelectionOption.LEFT);
    }

    void Update()
    {
        if (LevelControllerBehavior.levelController.enabled)
        {
            PlayerBehavior playerBehavior = LevelControllerBehavior.levelController.playerBehavior;
            _statsText.text = ("Atk:            " + Mathf.RoundToInt(playerBehavior.attack).ToString() 
                            + "\nHP:      " + (playerBehavior.hp < 100f ? " " : "") + (playerBehavior.hp < 10f ? " " : "") + Mathf.RoundToInt(playerBehavior.hp).ToString() + "/" + Mathf.RoundToInt(playerBehavior.maxHp).ToString()
                            + "\nCrit Rate: " + (playerBehavior.critRate < 10f ? " " : "") + "+" + Mathf.RoundToInt(playerBehavior.critRate).ToString() + "%"
                            + "\nSpeed:        " + Mathf.RoundToInt(playerBehavior.speedStat).ToString());

            _scoreText.text = "Score: " + LevelControllerBehavior.levelController.playerScore.ToString();
        }
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
        switch (wepL)
        {
            case Weapon.WeaponType.SWORD:
                _weaponLeft.sprite = _swordIcon;
                break;
            case Weapon.WeaponType.STAR:
                _weaponLeft.sprite = _starIcon;
                break;
            case Weapon.WeaponType.FAN:
                _weaponLeft.sprite = _fanIcon;
                break;
            case Weapon.WeaponType.DYNAMITE:
                _weaponLeft.sprite = _dynamiteIcon;
                break;
        }
        switch (wepR)
        {
            case Weapon.WeaponType.SWORD:
                _weaponRight.sprite = _swordIcon;
                break;
            case Weapon.WeaponType.STAR:
                _weaponRight.sprite = _starIcon;
                break;
            case Weapon.WeaponType.FAN:
                _weaponRight.sprite = _fanIcon;
                break;
            case Weapon.WeaponType.DYNAMITE:
                _weaponRight.sprite = _dynamiteIcon;
                break;
        }
        
        switch (statL)
        {
            case LevelControllerBehavior.NextSwapData.BoostableStat.ATTACK:
                _textLeft.text = "+" + magL + " Atk";
                break;
            case LevelControllerBehavior.NextSwapData.BoostableStat.HP:
                _textLeft.text = "+" + magL + " HP";
                break;
            case LevelControllerBehavior.NextSwapData.BoostableStat.CRITRATE:
                _textLeft.text = "+" + magL + " Crit";
                break;
            case LevelControllerBehavior.NextSwapData.BoostableStat.SPEED:
                _textLeft.text = "+" + magL + " Spd";
                break;
        }
        switch (statR)
        {
            case LevelControllerBehavior.NextSwapData.BoostableStat.ATTACK:
                _textRight.text = "+" + magR + " Atk";
                break;
            case LevelControllerBehavior.NextSwapData.BoostableStat.HP:
                _textRight.text = "+" + magR + " HP";
                break;
            case LevelControllerBehavior.NextSwapData.BoostableStat.CRITRATE:
                _textRight.text = "+" + magR + " Crit";
                break;
            case LevelControllerBehavior.NextSwapData.BoostableStat.SPEED:
                _textRight.text = "+" + magR + " Spd";
                break;
        }
    }
}
