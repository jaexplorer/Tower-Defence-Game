using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Frost : ManagedObject
{
    [SerializeField] private Transform _transform;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    private FrostTower _tower;
    private Tile _tile;
    private bool _isShown;
    private int _distanceFromFog = 10;
    private List<FrostTower> _towers = new List<FrostTower>(2);

    static private CompoundMesh _compoundMesh;
    // private static Vector3[] _tileVectors = new Vector3[] { new Vector3(0, 0f, 1), new Vector3(0, 0f, -1), new Vector3(-1, 0f, 0), new Vector3(1, 0f, 0) };
    private Tweener _tweener;
    // private Tweener _hideTweener;
    float _delay = 0.2f;

    //PROPERTIES///////////////////////////////////////////////
    public FrostTower tower { get { return _tower; } set { _tower = value; } }
    public Tile tile { get { return _tile; } set { _tile = value; } }
    public int distanceFromTower { get { return _distanceFromFog; } set { _distanceFromFog = value; } }
    public bool isShown { get { return _isShown; } }// set { _isShown = value; }}

    //EVENTS///////////////////////////////////////////////////
    protected override void OnInstantiate()
    {
        if (_compoundMesh == null)
        {
            GameObject go = new GameObject("FogCompoundMesh");
            _compoundMesh = go.AddComponent<CompoundMesh>();
            _compoundMesh.SetMaterial(_meshRenderer.sharedMaterial);
        }
        _compoundMesh.AddMesh(_meshFilter);
        _compoundMesh.HideMesh(_meshFilter);
    }

    protected override void OnProduce()
    {
        _transform.localPosition = new Vector3(_transform.localPosition.x, -0.2f, _transform.localPosition.z);

        //tile.onEnemyEnter.AddListener(AddFreezeEffect);
    }

    public void AddFreezeEffect()
    {
        // tile.enemies.ForEach(e => e.effects.AddEffect(new FreezeEffect()));
    }

    protected override void OnRecycle()
    {
        _isShown = false;
        _compoundMesh.HideMesh(_meshFilter);
        _tweener.Kill();
        _towers.Clear();
        _tile.fog = null;
        _distanceFromFog = 10;
        _compoundMesh.HideMesh(_meshFilter);
    }

    //PRIVATE//////////////////////////////////////////////////
    public void AddTower(FrostTower tower, int distance, bool showImmediately = false)
    {
        _towers.Add(tower);
        _tower = _towers[0];
        if (showImmediately && !_isShown)
        {
            ShowImmediately();
        }
        else if (_towers.Count == 1 || distance < _distanceFromFog)
        {
            _distanceFromFog = distance;
            Show();
        }
    }

    public void RemoveTower(FrostTower tower)
    {
        _towers.Remove(tower);
        if (_towers.Count == 0)
        {
            Hide();
        }
    }

    private void Show()
    {
        _compoundMesh.HideMesh(_meshFilter);// TODO: Optimize, cull.
        _meshRenderer.enabled = true;
        _tweener.Kill();
        _tweener = DOTween.To((x) => _transform.localPosition = new Vector3(_transform.localPosition.x, x, _transform.localPosition.z), _transform.localPosition.y, 0f, 0.3f).OnComplete(OnShowComplete).SetSpeedBased().SetDelay(_delay * _distanceFromFog);
    }

    private void Hide()
    {
        _compoundMesh.HideMesh(_meshFilter);
        _meshRenderer.enabled = true;
        _tweener.Kill();
        _tweener = DOTween.To((x) => _transform.localPosition = new Vector3(_transform.localPosition.x, x, _transform.localPosition.z), _transform.position.y, -0.2f, 0.3f).OnComplete(Recycle).SetSpeedBased().SetDelay(_delay * _distanceFromFog).SetEase(Ease.InCubic);
    }

    private void ShowImmediately()
    {
        _transform.localPosition = new Vector3(_transform.localPosition.x, 0f, _transform.localPosition.z);
        _compoundMesh.UpdatePositionAndShow(_meshFilter);
        OnShowComplete();
    }

    private void OnShowComplete()
    {
        _isShown = true;
        _compoundMesh.UpdatePositionAndShow(_meshFilter);
        _meshRenderer.enabled = false;
    }


}