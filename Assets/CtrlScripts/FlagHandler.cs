using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagHandler : MonoBehaviour
{
    private enum Flags {
        One, Two, Three, Four
    }

    [SerializeField] private GameObject flagTextBox;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text flagText;
    private HashSet<Flags> flagsAcquired;
    private PlayerBehavior player;

    private Dictionary<Flags, string> flagToNameMap = new Dictionary<Flags, string>{
        {Flags.One, "SSBhbSBBdG9taWM="},
        {Flags.Two, "QXNzYXNzaW4="},
        {Flags.Three, "VG9vIFRhbmt5"},
        {Flags.Four, "RVogTW9kZQ=="},
    };

    private Dictionary<Flags, string> flagToStringMap = new Dictionary<Flags, string>{
        {Flags.One, "aXRzX292ZXJfOTAwMA=="},
        {Flags.Two, "bXlfbmluamFfd2F5X2lzX2hhY2t6"},
        {Flags.Three, "c29tZXRoaW5nX2Fib3V0X25vdF9keWluZwo="},
        {Flags.Four, "d2hlcmVzX3RoZV9lbmVtaWVz"},
    };

    void Start()
    {
        flagsAcquired = new HashSet<Flags>();
        flagTextBox.SetActive(false);
        player = LevelControllerBehavior.levelController.playerBehavior;
    }

    public void CheckFlags()
    {
        if (!flagsAcquired.Contains(Flags.One) && player.attack > 9000)
        {
            DisplayFlagText(Flags.One);
        }
        if (!flagsAcquired.Contains(Flags.Two) && player.critRate >= 100)
        {
            DisplayFlagText(Flags.Two);
        }
        if (!flagsAcquired.Contains(Flags.Three) && player.hp >= 9999 && player.maxHp >= 9999)
        {
            DisplayFlagText(Flags.Three);
        }
        if (!flagsAcquired.Contains(Flags.Four) && !LevelControllerBehavior.levelController.didUpdate)
        {
            DisplayFlagText(Flags.Four);
        }
    }

    public void DisableFlagDisplay()
    {
        nameText.text = "";
        flagText.text = "";
        flagTextBox.SetActive(false);
        LevelControllerBehavior.levelController._levelActive = true;
    }

    public bool IsFlagDisplayed() { return flagText.text != ""; }

    private void DisplayFlagText(Flags flag)
    {
        flagsAcquired.Add(flag);
        flagTextBox.SetActive(true);
        nameText.text = Decode(flagToNameMap[flag]);
        flagText.text = $"flag{{{Decode(flagToStringMap[flag])}}}";
        LevelControllerBehavior.levelController._levelActive = false;
    }

    private static string Decode(string str) {
        return System.Text.Encoding.UTF8.GetString(System.Convert.FromBase64String(str));
    }
}
