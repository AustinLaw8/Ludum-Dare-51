using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagHandler : MonoBehaviour
{
    private enum Flags {
        Attack
    }

    [SerializeField] private GameObject flagTextBox;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text flagText;
    private HashSet<Flags> flagsAcquired;
    private PlayerBehavior player;

    private Dictionary<Flags, string> flagToNameMap = new Dictionary<Flags, string>{
        {Flags.Attack, "Temp Name"}
    };
    private Dictionary<Flags, string> flagToStringMap = new Dictionary<Flags, string>{
        {Flags.Attack, "its_over_9000"}
    };

    void Start()
    {
        flagsAcquired = new HashSet<Flags>();
        flagTextBox.SetActive(false);
        player = LevelControllerBehavior.levelController.playerBehavior;
    }

    // Update is called once per frame
    public void CheckFlags()
    {
        // Attack 
        if (!flagsAcquired.Contains(Flags.Attack) && player.attack > 9000)
        {
            DisplayFlagText(Flags.Attack);
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
        nameText.text = flagToNameMap[flag];
        flagText.text = $"flag{{{flagToStringMap[flag]}}}";
        LevelControllerBehavior.levelController._levelActive = false;
    }
}
