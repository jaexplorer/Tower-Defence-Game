﻿using UnityEngine;
using UnityEngine.UI;

public class TowerButton : MonoBehaviour
{
    [SerializeField]
    private Builder _builder;
    [SerializeField]
    private Image _image;
    [SerializeField]
    private TextMesh _nameText;
    [SerializeField]
    private TextMesh _indexText;
    [SerializeField]
    private TextMesh _priceText;

    private TowerData _towerData;
    private int _index;

    //PUBLIC///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void OnPress()
    {
        _builder.SelectTower(_index);
    }

    public void Setup(int index, TowerData towerData)
    {
        _index = index;
        _indexText.text = (_index + 1).ToString();
        _towerData = towerData;
        _image.sprite = _towerData.buttonSprite;
        SetPrice(100);
    }

    public void SetPrice(int price)
    {
        _priceText.text = price.ToString();
    }
}