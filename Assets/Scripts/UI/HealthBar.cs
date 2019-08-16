using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform _transform;
    [SerializeField] private RectTransform _healthTransform;
    [SerializeField] private RectTransform _hitTransform;

    private Queue<int> _damageQueue = new Queue<int>(5);
    private Tweener _tweener;
    private Enemy _enemy;
    private float _unit;
    private float _hitSize;
    private int _healthLastFrame;
    private int _accumulatedDamage;

    private static Camera _camera;
    private static int _hitFadeDelay = 4;

    //PROPERTIES///////////////////////////////////////////////
    new public Transform transform
    {
        get { return _transform; }
    }

    //EVENTS///////////////////////////////////////////////////
    private void Update()
    {
        _transform.position = _camera.WorldToScreenPoint(_enemy.transform.position + _enemy.data.healthBarPoint);
    }

    private void FixedUpdate()
    {

        int newDamage = _healthLastFrame - _enemy.health;
        _healthLastFrame = _enemy.health;
        _damageQueue.Enqueue(newDamage);
        if (newDamage > 0)
        {
            _accumulatedDamage += newDamage;
            _hitSize += newDamage * _unit;
            _hitSize = _accumulatedDamage * _unit;
        }
        int fadingDamage = _damageQueue.Dequeue();
        if (fadingDamage > 0 || newDamage > 0)
        {
            _healthTransform.sizeDelta = new Vector2(_unit * _enemy.health, _transform.rect.height);
            _accumulatedDamage -= fadingDamage;
            _tweener.Kill();
            _tweener = DOTween.To(() => _hitSize, x => _hitSize = x, _accumulatedDamage * _unit, 0.2f).SetUpdate(UpdateType.Fixed);
        }
        _hitTransform.sizeDelta = new Vector2(_hitSize, _transform.rect.height);
    }

    //PUBLIC///////////////////////////////////////////////////
    public void Reset(Enemy enemy)
    {
        _camera = Camera.main;
        _enemy = enemy;
        _enemy.onDeath.AddListener(OnEnemyDeath);
        _unit = _transform.rect.width / _enemy.data.health;
        _hitSize = 0f;
        _healthLastFrame = _enemy.health;
        _accumulatedDamage = 0;
        _hitTransform.sizeDelta = new Vector2(0, _transform.rect.height);
        _healthTransform.sizeDelta = new Vector2(_unit * _enemy.health, _transform.rect.height);
        _damageQueue.Clear();
        for (int i = 0; i < _hitFadeDelay; i++)
        {
            _damageQueue.Enqueue(0);
        }
    }

    public void OnEnemyDeath()
    {
        PoolManager.Recycle(gameObject);
    }
}
