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
    private int _zoomStepCurrent;
    private LTDescr ltd;

    //EVENTS///////////////////////////////////////////////////
    private void Start()
    {
        SetZoomLevel(_zoomStepDefault, true);
    }

    private void Update()
    {
        Vector3 moveVec = Vector3.zero;

        if (InputManager.up)
        {
            moveVec += new Vector3(1f, 0f, 1f);
        }
        if (InputManager.down)
        {
            moveVec += new Vector3(-1f, 0f, -1f);
        }
        if (InputManager.left)
        {
            moveVec += new Vector3(-1f, 0f, 1f);
        }
        if (InputManager.right)
        {
            moveVec += new Vector3(1f, 0f, -1f);
        }

        // Check movement state.
        if (InputManager.up || InputManager.down || InputManager.left || InputManager.right)
        {
            _moveVec = moveVec.normalized;
            _speedTweener.Pause();
            _speedFadeMultiplyer = 1f;
        }
        else if (InputManager.up.ended || InputManager.down.ended || InputManager.left.ended || InputManager.right.ended)
        {
            _speedTweener.Kill();
            _speedTweener = DOTween.To(() => _speedFadeMultiplyer, x => _speedFadeMultiplyer = x, 0f, _deccelerationDuration).SetUpdate(true);
        }

        // Move.
        if (_speedFadeMultiplyer > 0)
        {
            _anchorTransform.localPosition += _moveVec * (_moveSpeed * Time.unscaledDeltaTime * _speedFadeMultiplyer);
        }

        // Zoom.
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

    //PUBLIC///////////////////////////////////////////////////
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
        }
    }
}