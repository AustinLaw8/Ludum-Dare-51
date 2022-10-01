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

    public GameObject boxMainMenu, boxSettings, boxCredits, boxGameUI, boxPause;
    public Slider sliderSFXvolume;
    public float SFXvolumeValue;
    public Slider sliderMusicVolume;
    public float musicVolumeValue;
    public Slider sliderMasterVolume;
    public float masterVolumeValue;
    public void Start()
    {
        uiCanvasBehavior = this;
        _exclusiveBoxes = new GameObject[] {boxMainMenu, boxSettings, boxCredits, boxGameUI, boxPause};
        SetExclusiveBoxActive(boxMainMenu);
        SFXvolumeValue = 0.5f;
        musicVolumeValue = 0.5f;
        masterVolumeValue = 0.5f;
    }

    public void Update()
    {
        if (boxSettings.activeSelf || boxPause.activeSelf)
        {
            SFXvolumeValue = sliderSFXvolume.value;
            musicVolumeValue = sliderMusicVolume.value;
            masterVolumeValue = sliderMasterVolume.value;
        }
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
        SetExclusiveBoxActive(boxSettings);
    }
    public void ButtonCredits()
    {
        SetExclusiveBoxActive(boxCredits);
    }
    public void ButtonQuit()
    {
        Application.Quit();
    }
    public void ButtonBack()
    {
        SetExclusiveBoxActive(boxMainMenu);
    }
    public void ButtonPause()
    {
        pause();
    }
    public void ButtonPauseBack()
    {
        SetExclusiveBoxActive(boxGameUI);
        LevelControllerBehavior.levelController._levelActive = true;
    }
    public void ButtonPauseQuit()
    {
        LevelControllerBehavior.levelController._levelActive = false;
        SetExclusiveBoxActive(boxMainMenu);
    }
    public void pause()
    {
        LevelControllerBehavior.levelController._levelActive = false;
        SetExclusiveBoxActive(boxPause);
    }

}