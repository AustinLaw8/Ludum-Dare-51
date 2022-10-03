using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BambooBarBehavior : MonoBehaviour
{
    [SerializeField] private Image _bambooFillImage;
    private Slider _thisSlider;

    void Start()
    {
        _thisSlider = GetComponent<Slider>();
    }

    public void sliderChanged()
    {
        if (_bambooFillImage && _thisSlider)
        {
            _bambooFillImage.fillAmount = _thisSlider.value;
        }
    }
}
