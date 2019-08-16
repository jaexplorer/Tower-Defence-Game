using UnityEngine;
using System.Collections.Generic;

public class TowerMenu : MonoBehaviour
{
    [SerializeField]
    private Transform _buttonParentTransform;
    // [SerializeField]
    // private Builder _builder;
    [SerializeField]
    private GameObject _buttonOriginal;
    [SerializeField]
    private RectTransform _hudBarTransform;
    [SerializeField]
    private int _hudBarNominalWidth;
    [SerializeField]
    private int _buttonWidth;

    private List<TowerButton> _buttons = new List<TowerButton>(16);

    //PUBLIC///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void GenerateButtons(List<TowerData> availableTowersData)
    {
        for (int i = 0; i < _buttons.Count; i++)
        {
            PoolManager.Recycle(_buttons[i].gameObject);
        }
        _buttons.Clear();

        _buttonOriginal.SetActive(true);
        for (int i = 0; i < availableTowersData.Count; i++)
        {
            TowerButton newButton = PoolManager.Produce(_buttonOriginal).GetComponent<TowerButton>();
            newButton.Setup(i, availableTowersData[i]);
            newButton.transform.SetParent(_buttonParentTransform, false);
            _buttons.Add(newButton);
        }
        _buttonOriginal.SetActive(false);

        _hudBarTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _hudBarNominalWidth + _buttonWidth * _buttons.Count);
    }

    public void SetPrice(int index, int price)
    {
        _buttons[index].SetPrice(price);
    }
}