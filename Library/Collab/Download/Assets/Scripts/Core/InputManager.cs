using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    private bool _mouseOverUI;

    public bool mouseOverUI
    {
        get
        {
            return _mouseOverUI;
        }
        set
        {
            _mouseOverUI = value;
            Debug.Log(_mouseOverUI);
        }
    }

    public void Update()
    {

    }

    // public void OnViewportClick()
    // {

    // }
}
