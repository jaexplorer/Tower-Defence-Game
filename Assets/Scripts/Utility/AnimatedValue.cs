using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Not implimented, might come back
/// </summary>

[System.SerializableAttribute]
public class AnimatedFloat
{
    [SerializeField]
    private AnimationCurve _curve;
    [SerializeField]
    private float _duration;
    [SerializeField]
    private float _startValue;
    [SerializeField]
    private float _endValue;

    private float _value;
    private float _startTime;
    private bool _playing;
    private bool _initiated;

    //PROPERTIES///////////////////////////////////////////////
    public float value
    {
        get { return _value; }
    }

    //PUBLIC///////////////////////////////////////////////////
    public void Reset()
    {
        if (!_initiated)
        {
            Keyframe startFrame = new Keyframe();
            startFrame.value = _startValue;
            startFrame.outTangent = _curve.keys[0].outTangent;
            _curve.MoveKey(0, startFrame);

            Keyframe endFrame = new Keyframe();
            endFrame.time = _duration;
            endFrame.value = _endValue;
            endFrame.outTangent = _curve.keys[1].outTangent;
            _curve.MoveKey(1, endFrame);

            _initiated = true;
        }
        _startTime = Time.time;
        _value = _startValue;
        //Debug.Log("r");
    }

    public void Update()
    {
        if (_value != _endValue)
        {
            _value = _curve.Evaluate((Time.time - _startTime));
        }
        //Debug.Log(_curve.Evaluate((Time.time - _startTime)));
    }

    public void IsAnimating()
    {

    }

    // public float GetValue()
    // {
    //     return _curve.Evaluate((Time.time - _startTime));
    // }
}
