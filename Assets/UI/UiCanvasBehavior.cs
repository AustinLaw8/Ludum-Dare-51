using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiCanvasBehavior : MonoBehaviour
{
    public static UiCanvasBehavior uiCanvasBehavior;
    // Boxes are "empty" GameObjects that are parents for a set of UI/Menu elements
    protected GameObject[] _exclusiveBoxes; // for setExclusiveBoxActive()

    public GameObject boxMainMenu, boxSettings, boxCredits, boxGameUI, boxDeathScreen; // different game menus
    public GameObject buttonPauseQuit; // button
    public Slider sliderSFXvolume;
    public float SFXvolumeValue;
    public Slider sliderMusicVolume;
    public float musicVolumeValue;
    public Slider sliderMasterVolume;
    public float masterVolumeValue;
    private bool _settingsNotPause = false; // denotes whether boxSettings was set active as settings menu or as pause menu
    public bool paused {get {return boxSettings.activeInHierarchy && !_settingsNotPause;}}
    public TMPro.TMP_Text scoreText;

    public void Start()
    {
        uiCanvasBehavior = this;
        _exclusiveBoxes = new GameObject[] {boxMainMenu, boxSettings, boxCredits, boxGameUI, boxDeathScreen};
        SetExclusiveBoxActive(boxMainMenu);
        SFXvolumeValue = 1f;
        sliderSFXvolume.value = 1f;
        musicVolumeValue = 1f;
        sliderMusicVolume.value = 1f;
        masterVolumeValue = 1f;
        sliderMasterVolume.value = 1f;
    }

    public void Update()
    {
        if (boxSettings.activeSelf)
        {
            SFXvolumeValue = sliderSFXvolume.value;
            musicVolumeValue = sliderMusicVolume.value;
            masterVolumeValue = sliderMasterVolume.value;
        }
        LevelControllerBehavior.levelController.menuThemeSource.volume = musicVolumeValue * masterVolumeValue;
        LevelControllerBehavior.levelController.battleThemeSource.volume = musicVolumeValue * masterVolumeValue;
        LevelControllerBehavior.levelController.sfxSource.volume = SFXvolumeValue * masterVolumeValue;
        LevelControllerBehavior.levelController.playerBehavior.gameObject.GetComponent<AudioSource>().volume = SFXvolumeValue * masterVolumeValue;
    }

    public void SetExclusiveBoxActive(GameObject box)
    {   
        foreach (GameObject exclusiveBox in _exclusiveBoxes)
        {
            exclusiveBox.SetActive(box == exclusiveBox);
        }
    }

    public void ButtonPlay()
    {
        SetExclusiveBoxActive(boxGameUI);
        LevelControllerBehavior.levelController.LevelStart();
    }

    public void ButtonSettings()
    {
        _settingsNotPause = true;
        SetExclusiveBoxActive(boxSettings);
        buttonPauseQuit.SetActive(false);
    }

    public void ButtonCredits()
    {
        SetExclusiveBoxActive(boxCredits);
    }

    public void ButtonQuit()
    {
        Application.Quit();
    }

    // back to main menu screen
    public void ButtonBack()
    {
        SetExclusiveBoxActive(boxMainMenu);
    }

    public void ButtonPause()
    {
        pause();
    }

    public void ButtonSettingsBack() // handles both settings menu and pause menu as they are the same box
    {
        if (_settingsNotPause)
        {
            SetExclusiveBoxActive(boxMainMenu);
        }
        else
        {
            SetExclusiveBoxActive(boxGameUI);
            LevelControllerBehavior.levelController._levelActive = true;
        }
    }
    
    public void ButtonPauseQuit()
    {
        LevelControllerBehavior.levelController._levelActive = false;
        LevelControllerBehavior.levelController._gameOver = true;
        LevelControllerBehavior.levelController.resetAudio();
        SetExclusiveBoxActive(boxMainMenu);
    }

    public void pause()
    {
        LevelControllerBehavior.levelController._levelActive = false;
        _settingsNotPause = false;
        SetExclusiveBoxActive(boxSettings);
        buttonPauseQuit.SetActive(true);
    }

    public void enterDeathScreen() // called elsewhere when Phemie Dies
    {
        scoreText.text = "You died!\nScore: " + LevelControllerBehavior.levelController.playerScore.ToString();
        LevelControllerBehavior.levelController._levelActive = false;
        SetExclusiveBoxActive(boxDeathScreen);
    }

    public void buttonDeathScreenExitToMenu()
    {   
        SetExclusiveBoxActive(boxMainMenu);
        LevelControllerBehavior.levelController.resetAudio(); // set to main menu music
    }
}