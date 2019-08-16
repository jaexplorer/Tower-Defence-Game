using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Hud : CustomBehaviour
{
    [SerializeField] private Builder _builder;
    [SerializeField] private GameObject _playButtons;
    [SerializeField] private GameObject _startWaveButton;
    [SerializeField] private GameObject _towerBar;
    [SerializeField] private RectTransform _hudBarTransform;
    [SerializeField] private TextMesh _portalPointsText;
    [SerializeField] private TextMesh _waveIndexText;
    [SerializeField] private TextMesh _energyText;

    //PROPERTIES///////////////////////////////////////////////    
    // public TextMeshProUGUI energyText { get { return _energyText; } 
    // public TextMeshProUGUI waveIndexText { get { return _waveIndexText; } }
    // public TextMeshProUGUI portalPointsText { get { return _portalPointsText; } }

    //EVENTS///////////////////////////////////////////////////
    protected override void OnLevelLoad()
    {
        _portalPointsText.text = Level.instance.portal.points.ToString();
    }

    //PRIVATE//////////////////////////////////////////////////
    // public void SetEnergy(int energy)
    // {
    //     _energyText.text = energy.ToString();
    // }

    // public void SetWaves(int current, int total)
    // {
    //     _waveIndexText.text = (current).ToString() + "/" + total.ToString(); //TODO: Fix garbage allocation.
    // }

    // public void GoToBuildMode()
    // {
    //     // _playButtons.SetActive(false);
    //     // _startWaveButton.SetActive(true);
    // }

    // public void GoToWaveMode()
    // {
    //     // _playButtons.SetActive(true);
    //     // _startWaveButton.SetActive(false);
    // }

    // public void OnPortalDamage()
    // {
    //     _portalPointsText.text = Level.instance.portal.points.ToString();
    // }

    // public void SetPortalPoints(int points)
    // {
    //     _portalPointsText.text = points.ToString();
    // }
}