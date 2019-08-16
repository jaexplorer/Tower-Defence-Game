using UnityEngine;
using System.Collections;
using DG.Tweening;
using DentedPixel;

public class CameraControl : MonoBehaviour
{
    [SerializeField] private Transform _anchorTransform;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _deccelerationDuration;
    [SerializeField] private float _zoomDuration;
    [SerializeField] private float _zoomStepDistance;
    [SerializeField] private float _zoomDistanceMin;
    [SerializeField] private float _zoomStepScale;
    [SerializeField] private float _zoomStepSpeedScale;
    [SerializeField] private int _zoomStepMax;
    [SerializeField] private int _zoomStepDefault;

    private Tweener _speedTweener;
    private Tweener _zoomTweener;
    private Vector3 _moveVec;
    private float _speedFadeMultiplyer;
    private float _speedZoomMultiplyer;
    private int _zoomStepCurrent;
    private LTDescr ltd;


    private void Start()
    {
        SetZoomLevel(_zoomStepDefault, true);
    }

    //EVENTS///////////////////////////////////////////////////
    private void Update()
    {
        Vector3 moveVec = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            moveVec += new Vector3(1f, 0f, 1f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            moveVec += new Vector3(-1f, 0f, -1f);
        }
        if (Input.GetKey(KeyCode.A))
        {
            moveVec += new Vector3(-1f, 0f, 1f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            moveVec += new Vector3(1f, 0f, -1f);
        }

        // Checking movement state.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            _moveVec = moveVec.normalized;
        }
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            _speedTweener.Pause();
            _speedFadeMultiplyer = 1f;
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            // if (ltd != null)
            // {
            //     ltd.cancel();
            // }
            // ltd = LeanTween.value(1, 0, 0.2f).setOnUpdate(x => _speedFadeMultiplyer = x).setIgnoreTimeScale(true);
            // if (_speedTweener != null)
            // {
            //     // ltd.resume();
            //     // LeanTween.cancel(ltd.id);
            //     // ltd.se;
            //     // _speedFadeMultiplyer = 1f;
            //     _speedTweener.Restart();
            //     _speedTweener.SetUpdate(true);
            // }
            // else
            // {
            //     // ltd = LeanTween.value(1, 0, 0.2f).setOnUpdate(x => _speedFadeMultiplyer = x);
            //     // _speedTweener.Kill();
            //     // _speedTweener = DOTween.To(() => _speedFadeMultiplyer, x => _speedFadeMultiplyer = x, 0f, _deccelerationDuration).SetUpdate(true);
            //     // _speedTweener.Kill();
            // }
            _speedTweener.Kill();
            _speedTweener = DOTween.To(() => _speedFadeMultiplyer, x => _speedFadeMultiplyer = x, 0f, _deccelerationDuration).SetUpdate(true);
            // _speedTweener = DOTween.To(() => _speedFadeMultiplyer, x => _speedFadeMultiplyer = x, 0f, _deccelerationDuration).SetUpdate(true);
        }

        // Moving.
        if (_speedFadeMultiplyer > 0)
        {
            // Debug.Log(_speedFadeMultiplyer);
            // Debug.Log(this.GetInstanceID());
            _anchorTransform.localPosition += _moveVec * (_moveSpeed * Time.unscaledDeltaTime * _speedFadeMultiplyer);// * _speedZoomMultiplyer);
        }
        // if (ltd != null)
        //     Debug.Log(ltd.lastVal);
        // if (ltd != null && ltd.lastVal > 0)
        // {

        //     _anchorTransform.localPosition += _moveVec * (_moveSpeed * Time.deltaTime * ltd.lastVal * _speedZoomMultiplyer);
        // }

        // Zoom input.
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            float zoomSign = Mathf.Sign(Input.GetAxis("Mouse ScrollWheel"));
            if ((_zoomStepCurrent < _zoomStepMax && zoomSign < 0f) || (_zoomStepCurrent > 0 && zoomSign > 0f))
            {
                if (zoomSign > 0)
                {
                    _zoomStepCurrent--;
                }
                else
                {
                    _zoomStepCurrent++;
                }
                SetZoomLevel(_zoomStepCurrent);
            }
        }
    }

    void SetFloat(float f)
    {
    }

    public void SetZoomLevel(int zoomLevel, bool snap = false)
    {
        if (zoomLevel >= 0 && zoomLevel <= _zoomStepMax)
        {
            _zoomStepCurrent = zoomLevel;
            float zoomDistance = _zoomDistanceMin - _zoomStepDistance * Mathf.Pow(_zoomStepScale, _zoomStepCurrent);
            if (snap)
            {
                _cameraTransform.localPosition = new Vector3(0f, 0f, zoomDistance);
            }
            else
            {
                if (_zoomTweener != null)
                {
                    _zoomTweener.Kill();
                }
                _zoomTweener = _cameraTransform.DOLocalMoveZ(_zoomDistanceMin - _zoomStepDistance * Mathf.Pow(_zoomStepScale, _zoomStepCurrent), _zoomDuration).SetUpdate(true);
            }
            _speedZoomMultiplyer = Mathf.Pow(_zoomStepSpeedScale, _zoomStepCurrent);
        }
    }
}