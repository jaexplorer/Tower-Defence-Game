using UnityEngine;
using System.Collections.Generic;

/// Combinds objects together, their meshes.For hiding, moving, etc. By default unity, sends individual meshes to render. This creates compound meshes for performance reasons
public class CompoundMesh : MonoBehaviour
{
    [SerializeField] private Material _material;
    private List<Submesh> _submeshes = new List<Submesh>(4);
    // private List<Element> _emptyElements = new List<Element>(16);

    private Dictionary<MeshFilter, Element> _dictionary = new Dictionary<MeshFilter, Element>(2000);

    private const int MESH_SIZE_MAX = 64000;

    //EVENTS///////////////////////////////////////////////////
    private void OnRender()
    {

    }

    //PUBLIC///////////////////////////////////////////////////
    public void SetMaterial(Material material)
    {
        _material = material;
        for (int i = 0; i < _submeshes.Count; i++)
        {
            _submeshes[i].SetMaterial(material);
        }
    }

    public void AddMesh(Mesh mesh, Vector3 position)
    {

    }

    public void AddMesh(MeshFilter meshFilter)
    {
        Mesh mesh = meshFilter.sharedMesh;
        Submesh submesh = null;
        for (int i = 0; i < _submeshes.Count; i++)
        {
            if (mesh.vertexCount < _submeshes[i].capacity)
            {
                submesh = _submeshes[i];
                break;
            }
        }
        if (submesh == null)
        {
            submesh = new Submesh(this, _material);
            _submeshes.Add(submesh);
        }

        Element element = new Element(meshFilter, submesh);
        _dictionary.Add(meshFilter, element);
    }

    public void RemoveMesh(MeshFilter meshFilter)
    {
        Element element = GetElement(meshFilter);
        if (element != null)
        {
            element.Remove();
        }
    }

    public void UpdatePositionAndShow(MeshFilter meshFilter)
    {
        Element element = GetElement(meshFilter);
        if (element != null)
        {
            element.UpdatePositionAndShow();
        }
    }

    public void UpdatePosition(MeshFilter meshFilter)
    {
        Element element = GetElement(meshFilter);
        if (element != null)
        {
            element.UpdatePosition();
        }
    }

    public void HideMesh(MeshFilter meshFilter)
    {
        Element element = GetElement(meshFilter);
        if (element != null)
        {
            element.Hide();
        }
    }

    public void ShowMesh(MeshFilter meshFilter)
    {
        Element element = GetElement(meshFilter);
        if (element != null)
        {
            element.Show();
        }
    }

    public void Clear()
    {
        for (int i = 0; i < _submeshes.Count; i++)
        {
            _submeshes[i].Clear();
        }
    }

    private Element GetElement(MeshFilter meshFilter)
    {
        Element element;
        _dictionary.TryGetValue(meshFilter, out element);
        return element;
    }

    private struct Gap
    {
        int vertexMin;
        int vertexMax;
    }

    private class Element
    {
        private Submesh _submesh;
        private Transform _transform;
        private Vector3 _position;
        private List<Vector3> _vertices;
        private List<Vector3> _normals;
        private List<Vector2> _uv;
        private List<int> _triangles;
        private int _firstVertexIndex;
        private int _firstTriangleIndex;
        private int _index;
        private bool _hidden;

        public List<Vector3> vertices
        {
            get { return _vertices; }
        }

        public List<Vector3> normals
        {
            get { return _normals; }
        }

        public List<Vector2> uv
        {
            get { return _uv; }
        }

        public List<int> triangles
        {
            get { return _triangles; }
        }

        public int firstVertexIndex
        {
            get { return _firstVertexIndex; }
            set { _firstVertexIndex = value; }
        }

        public int firstTriangleIndex
        {
            get { return _firstTriangleIndex; }
            set { _firstTriangleIndex = value; }
        }

        public int index
        {
            get { return _index; }
            set { _index = value; }
        }

        public Vector3 position
        {
            get { return _position; }
        }

        public Element(MeshFilter meshFilter, Submesh submesh)
        {
            _submesh = submesh;
            _transform = meshFilter.transform;
            _position = _transform.position;
            Mesh mesh = meshFilter.sharedMesh;
            _vertices = new List<Vector3>(mesh.vertices);
            _normals = new List<Vector3>(mesh.normals);
            _uv = new List<Vector2>(mesh.uv);
            _triangles = new List<int>(mesh.triangles);
            _submesh.AddElement(this);
        }

        public void Remove()
        {
            _submesh.Remove(this);
        }

        public void Hide()
        {
            if (!_hidden)
            {
                _submesh.Hide(this);
                _hidden = true;
            }
        }

        public void Show()
        {
            if (_hidden)
            {
                _submesh.Show(this);
                _hidden = false;
            }
        }

        public void UpdatePositionAndShow()
        {
            _position = _transform.position;
            _submesh.UpdatePositionAndShow(this);
            _hidden = false;
        }

        public void UpdateUVs()
        {
            _submesh.UpdateUV(this);
        }

        public void UpdatePosition()
        {
            _position = _transform.position;
            _submesh.UpdatePosition(this);
        }
    }

    private class Submesh
    {
        private GameObject _gameObject;
        private MeshFilter _meshFilter;
        private MeshRenderer _meshRenderer;
        private Mesh _mesh;
        private List<Element> _elements = new List<Element>(256);
        private List<Vector3> _vertices = new List<Vector3>(MESH_SIZE_MAX);
        private List<Vector3> _normals = new List<Vector3>(MESH_SIZE_MAX);
        private List<Vector2> _uv = new List<Vector2>(MESH_SIZE_MAX);
        private List<int> _triangles = new List<int>(MESH_SIZE_MAX * 3);
        private int _capacity = MESH_SIZE_MAX;

        public int capacity
        {
            get { return _capacity; }
        }

        public Submesh(CompoundMesh compoundMesh, Material material)
        {
            _gameObject = new GameObject("Submesh");
            _gameObject.transform.parent = compoundMesh.transform;
            _meshFilter = _gameObject.AddComponent<MeshFilter>();
            _meshRenderer = _gameObject.AddComponent<MeshRenderer>();
            _meshRenderer.material = material;
            _mesh = new Mesh();
            _mesh.hideFlags = HideFlags.DontSave;
            _mesh.MarkDynamic();
            _meshFilter.mesh = _mesh;
        }

        public void SetMaterial(Material material)
        {
            _meshRenderer.material = material;
        }

        public void AddElement(Element element)
        {
            element.index = _elements.Count;
            _elements.Add(element);
            int firstVertexIndex = _vertices.Count;
            element.firstVertexIndex = firstVertexIndex;
            element.firstTriangleIndex = _triangles.Count;
            List<int> triangles = element.triangles;
            for (int i = 0; i < triangles.Count; i++)
            {
                _triangles.Add(triangles[i] + firstVertexIndex);
            }
            List<Vector3> vertices = element.vertices;
            for (int i = 0; i < vertices.Count; i++)
            {
                _vertices.Add(vertices[i] + element.position);
            }
            _uv.AddRange(element.uv);
            _normals.AddRange(element.normals);
            _mesh.SetVertices(_vertices);
            _mesh.SetUVs(0, _uv);
            _mesh.SetTriangles(_triangles, 0);
            _mesh.SetNormals(_normals);
            _mesh.RecalculateBounds();
            _capacity -= _vertices.Count;
        }

        public void Remove(Element element)
        {
            // _elements.RemoveAt(element.index);//UNFINISHED
            Debug.Log("NOT IMPLEMENTED");
        }

        public void Hide(Element element)
        {
            for (int i = 0; i < element.triangles.Count; i++)
            {
                _triangles[i + element.firstTriangleIndex] = 0;
            }
            _mesh.SetTriangles(_triangles, 0);
        }

        public void UpdatePositionAndShow(Element element)
        {
            UpdatePosition(element);
            Show(element);
            _mesh.RecalculateBounds();
        }

        public void UpdatePosition(Element element)
        {
            for (int i = 0; i < element.vertices.Count; i++)
            {
                _vertices[i + element.firstVertexIndex] = element.vertices[i] + element.position;
            }
            _mesh.SetVertices(_vertices);
        }

        public void Show(Element element)
        {
            for (int i = 0; i < element.triangles.Count; i++)
            {
                _triangles[i + element.firstTriangleIndex] = element.triangles[i] + element.firstVertexIndex;
            }
            _mesh.SetTriangles(_triangles, 0);
        }

        public void UpdateUV(Element element)
        {
            for (int i = 0; i < element.uv.Count; i++)
            {
                _uv[i + element.firstVertexIndex] = element.uv[i];
            }
            _mesh.SetUVs(0, _uv);
        }

        public void Clear()
        {
            Debug.Log("NOT IMPLEMENTED");
        }
    }
}