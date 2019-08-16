using UnityEngine;

public class Portal : CustomBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    private int _points;
    private Tile _tile;

    private const int POINTS_MAX = 100;

    //PROPERTIES///////////////////////////////////////////////
    public int points { get { return _points; } }
    public Tile tile { get { return _tile; } set { _tile = value; } }

    //EVENTS///////////////////////////////////////////////////
    private void Start()
    {
        _audioSource.PlayDelayed(1);
    }

    protected override void OnWaveLoad()
    {
        _points = ProfileManager.instance.saveData.currentPortalPoints;
    }

    protected override void OnWaveSave()
    {
        ProfileManager.instance.saveData.currentPortalPoints = _points;
    }

    //PUBLIC///////////////////////////////////////////////////
    public void Reset()
    {
        _points = POINTS_MAX;
    }

    public void Damage(int amount)
    {
        _points -= amount;
        if (_points < 0)
        {
            _points = 0;
        }
        if (_points == 0)
        {
            EventManager.onWaveLost.Invoke();
        }
    }
}