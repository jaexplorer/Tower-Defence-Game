using UnityEngine;

public class Portal : MonoBehaviour
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
        // Debug.Log(transform.position);
        // Debug.Log(_audioSource.spatialBlend);
        // _audioSource.volume = 0f;
        _audioSource.PlayDelayed(1);
        // _audioSource.volume = 1f;
    }

    //PUBLIC///////////////////////////////////////////////////
    public void Reset()
    {
        _points = POINTS_MAX;
        Hud.instance.SetPortalPoints(_points);
    }

    public void Damage(int amount)
    {
        _points -= amount;
        if (_points < 0)
        {
            _points = 0;
        }
        Hud.instance.SetPortalPoints(_points);
        if (_points == 0)
        {
            GameManager.instance.OnPortalDepleted();
        }
    }

    public void OnWaveLoad()
    {
        _points = ProfileManager.instance.saveData.currentPortalPoints;
        Hud.instance.SetPortalPoints(_points);
    }

    public void OnWaveSave()
    {
        ProfileManager.instance.saveData.currentPortalPoints = _points;
    }
}