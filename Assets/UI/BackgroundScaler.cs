// By Joshua Denk, with heavy reference of https://sharpcoderblog.com/blog/unity-3d-create-main-menu-with-ui-canvas, accessed 09/20/2022
// This script scales the background to always fit the screen.
// It uses ExecuteAlways which can cause MAJOR ISSUES with prefabs, so if possible:
//   - Avoid using with prefabs unless you know EXACTLY what you are doing
//   - If you want to add further code that affects the associated GameObject, make and concurrently attach another script
// NOTE: I *think* that this works correctly, BUT there is extra space on each side when run in-editor. 
//       I think just a quirk of the editor, but need to fix later if not
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class BackgroundScaler : MonoBehaviour
{
    private Image _backgroundImage;
    private RectTransform _imageRectTransform;
    private float _rawImageRatio_XtoY; // width to height

    // Start is called before the first frame update
    void Start()
    {
        _backgroundImage = GetComponent<Image>();
        _imageRectTransform = _backgroundImage.rectTransform;
        _rawImageRatio_XtoY = _backgroundImage.sprite.bounds.size.x / _backgroundImage.sprite.bounds.size.y;
    }

    // Update is called once per frame
    void Update()
    {
        if (Screen.height * _rawImageRatio_XtoY >= Screen.width)
        {
            _imageRectTransform.sizeDelta = new Vector2(Screen.height * _rawImageRatio_XtoY, Screen.height);
        }
        else
        {
            _imageRectTransform.sizeDelta = new Vector2(Screen.width, Screen.width / _rawImageRatio_XtoY);
        }
    }
}

