using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UITransition : MonoBehaviour
{
    // [SerializeField]
    // private GameObject _gameObject;
    // [SerializeField]
    // private Transform _transform;
    [SerializeField]
    private TransitionType _transitionType;
    [SerializeField]
    private UITransition[] _replacedTransitions;
    [SerializeField]
    private Animation _animationIn;
    [SerializeField]
    private Animation _animationOut;

    private bool _active;

    public bool active
    {
        get { return _active; }
    }

    private void Awake()
    {
        _active = gameObject.activeSelf;
    }

    public void Activate()
    {
        if (!_active)
        {
            _active = true;
            switch (_transitionType)
            {
                case TransitionType.EnableDisable:
                    Enable();
                    break;
                case TransitionType.Slide:
                    SlideIn();
                    break;
                case TransitionType.Animation:
                    AnimateIn();
                    break;
            }
            for (int i = 0; i < _replacedTransitions.Length; i++)
            {
                _replacedTransitions[i].Deactivate();
            }
        }
    }

    public void Deactivate()
    {
        if (_active)
        {
            _active = false;
            switch (_transitionType)
            {
                case TransitionType.EnableDisable:
                    Disable();
                    break;
                case TransitionType.Slide:
                    SlideOut();
                    break;
                case TransitionType.Animation:
                    AnimateOut();
                    break;
            }
            for (int i = 0; i < _replacedTransitions.Length; i++)
            {
                _replacedTransitions[i].Deactivate();
            }
        }
    }

    protected virtual void Enable()
    {
        gameObject.SetActive(true);
    }

    protected virtual void Disable()
    {
        gameObject.SetActive(false);
    }

    protected virtual void SlideIn()
    {

    }

    protected virtual void SlideOut()
    {

    }

    protected virtual void AnimateIn()
    {
        _animationIn.Play();
    }

    protected virtual void AnimateOut()
    {
        _animationOut.Play();
    }

    private enum TransitionType
    {
        EnableDisable,
        Animation,
        Slide,
    }
}

