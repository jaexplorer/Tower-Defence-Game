using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GradientMap", menuName = "Data/GradientMap", order = 1)]
public class GradientMap : ScriptableObject
{
    [SerializeField] private GradientData[] _gradientData = new GradientData[32];
    [SerializeField] private Texture2D _texture;
    [SerializeField] private int _columns = 2;
    [SerializeField] private int _rows = 16;

    public Texture2D texture { get { return _texture; } set { _texture = value; } }
    public int columns { get { return _columns; } set { _columns = value; } }
    public int rows { get { return _rows; } set { _rows = value; } }

    public GradientData GetGradientData(int x, int y)
    {
        return _gradientData[FlatIndex(x, y, _columns)];
    }

    public int GetGradientIndex(int x, int y)
    {
        return FlatIndex(x, y, _columns);
    }

    public void SaveTexture()
    {
        int gradientHeight = _texture.height / _rows;
        int gradientWidth = _texture.width / _columns;
        float pixelwidth = 1 / gradientWidth;
        for (int y = 0; y < _rows; y++)
        {
            for (int x = 0; x < _columns; x++)
            {
                Gradient gradient = _gradientData[FlatIndex(x, y, _columns)].gradient;
                Color[] pixels = new Color[gradientWidth * gradientHeight];
                for (int px = 0; px < _columns; px++)
                {
                    Color color = Color.black;
                    if (gradient != null)
                    {
                        color = gradient.Evaluate(pixelwidth * px);
                    }
                    for (int py = 0; py < _rows; py++)
                    {
                        pixels[FlatIndex(px, py, gradientWidth)] = color;
                    }
                }
                _texture.SetPixels(gradientWidth * x, gradientHeight * y, gradientWidth, gradientHeight, pixels);
            }
        }
    }

    public void MoveBorders(int left, int right, int top, int bottom)
    {
        // Finding new array size.
        int newSizeX = _columns + right + left;
        int newSizeY = _rows + top + bottom;

        // Checking if size is correct.
        if (newSizeX > 0 && newSizeY > 0)
        {
            // Creating new array.
            GradientData[] newGradients = new GradientData[newSizeX * newSizeY];

            // Finding overlapping range which will be copied to the new array.
            int overlapXMin = Mathf.Max(-left, 0);
            int overlapXMax = Mathf.Min(_columns + right, _columns);
            int overlapZMin = Mathf.Max(-bottom, 0);
            int overlapZMax = Mathf.Min(_rows + top, _rows);

            // Copying overlapping tiles to the new array.
            for (int x = overlapXMin, newX = x + left; x < overlapXMax; x++, newX = x + left)
            {
                for (int z = overlapZMin, newZ = z + bottom; z < overlapZMax; z++, newZ = z + bottom)
                {
                    newGradients[FlatIndex(newX, newZ, newSizeX)] = _gradientData[FlatIndex(x, z, _columns)];
                }
            }

            // Applying changes.
            _gradientData = newGradients;
            _columns = newSizeX;
            _rows = newSizeY;
        }
    }

    private int FlatIndex(int x, int y, int sizeX)
    {
        return y * sizeX + x;
    }

    [System.SerializableAttribute]
    public class GradientData
    {
        public Gradient gradient;
        public string name;
    }
}
