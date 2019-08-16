using UnityEngine;

public class Pointer : MonoBehaviour
{
    [SerializeField]
    private LayerMask _tileRaycastMask;
    private Tile _mouseTile;

    //PROPERTIES///////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public Tile mouseTile
    {
        get { return _mouseTile; }
    }

    //EVENTS///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    public void Update()
    {
        // Raycasting objects.
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 200f, _tileRaycastMask))
        {
            _mouseTile = Level.instance.GetTile(hit.collider.transform.localPosition);
        }
        else
        {
            _mouseTile = null;
        }
    }
}