using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapOptionsBehavior : MonoBehaviour
{
    [SerializeField] private Image _frameLeft, _frameRight;
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
}
