using UnityEngine;
using UnityEditor;

public class LevelEditorTool
{
    protected string _name;
    protected static LevelEditor _levelEditor;

    protected Color _selectedButtonColor = new Color(1f, 0.8f, 0f);
    protected Color _brushColor = new Color(1f, 1f, 0.2f);

    public static void Initiate(LevelEditor levelEditor)
    {
        _levelEditor = levelEditor;
    }

    public virtual void OnEnable()
    {

    }

    public virtual void OnDisable()
    {

    }

    public virtual void OnActivate()
    {

    }

    public virtual void DrawHandles()
    {

    }

    public virtual void DrawGUI()
    {

    }

    public virtual void HandleInput(Event e)
    {

    }

    public virtual void OnFocus()
    {

    }

    public virtual void OnLostFocus()
    {

    }

    public virtual void Update()
    {

    }

    public string GetName()
    {
        return _name;
    }

    protected void DrawBrush(int size, Color color)
    {
        TilePosition mouseTilePosition = _levelEditor.GetMouseTilePosition();
        // /Vector3 MOUSEwORLDpOSITION = _levelEditor.GET
        int tileX = mouseTilePosition.x;
        int tileZ = mouseTilePosition.z;
        if (tileX >= 0 && tileZ >= 0)
        {
            float tileY = 0f;
            // Serialize
            LevelData.Tile tile = _levelEditor.level.data.GetTile(mouseTilePosition);
            if (tile != null)
            {
                tileY += tile.level * 0.5f;
                if (tile.elevated)
                {
                    tileY += 0.25f;
                }
            }
            float xMin = Mathf.Max(tileX - size - 0.5f, -0.5f);
            float zMin = Mathf.Max(tileZ - size - 0.5f, -0.5f);
            float xMax = Mathf.Min(tileX + size + 0.5f, _levelEditor.level.sizeX - 0.5f);
            float zMax = Mathf.Min(tileZ + size + 0.5f, _levelEditor.level.sizeZ - 0.5f);
            Vector3 v1 = _levelEditor.WorldToScreenPoint(new Vector3(xMin, tileY, zMin));
            Vector3 v2 = _levelEditor.WorldToScreenPoint(new Vector3(xMin, tileY, zMax));
            Vector3 v3 = _levelEditor.WorldToScreenPoint(new Vector3(xMax, tileY, zMax));
            Vector3 v4 = _levelEditor.WorldToScreenPoint(new Vector3(xMax, tileY, zMin));

            Vector3 vt1 = _levelEditor.WorldToScreenPoint(new Vector3(xMin, 0, zMin));
            Vector3 vt2 = _levelEditor.WorldToScreenPoint(new Vector3(xMin, 0, zMax));
            Vector3 vt3 = _levelEditor.WorldToScreenPoint(new Vector3(xMax, 0, zMax));
            Vector3 vt4 = _levelEditor.WorldToScreenPoint(new Vector3(xMax, 0, zMin));

            if (xMin < xMax && zMin < zMax)
            {
                Handles.color = Color.white;
                Handles.DrawSolidRectangleWithOutline(new Vector3[4] { v1, v2, v3, v4 }, new Color(color.r, color.g, color.b, 0.2f), new Color(color.r, color.g, color.b, 1f));
                Handles.DrawSolidRectangleWithOutline(new Vector3[4] { vt1, vt2, vt3, vt4 }, new Color(color.r, color.g, color.b, 0.00f), new Color(color.r, color.g, color.b, 1f));
            }
        }
    }
}