using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lightning : MonoBehaviour
{
    public float _nodeOffset;
    public float _width;
    public float _jitter1;
    public float _jitter2;
    public float _noise1;
    public float _noise2;
    public float _speed;
    public float _lifetimeMax;

    //protected Vector3[] verts;
    protected MeshFilter _meshFilter;
    protected float _lifetime;

    public Transform _targetTransform;//TEST
    public Transform _transform;//TEST

    static private Quaternion normalRotationQuat;// = Quaternion.AngleAxis(90, Vector3.right);

    public GameObject testVert;

    ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////EVENTS
    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        //Launch(_targetTransform.position - transform.position);//TEST
        //GenerateMesh();
        Destroy(gameObject, _lifetimeMax);
    }

    void Update()
    {
        if (_targetTransform)
        {
            GenerateMesh();
            _lifetime += Time.deltaTime;
        }
        else
            Destroy(gameObject);
        //		Vector3 direction = _target.position;
        //		float distance = direction.magnitude;
        //		int nodesCount = (int)(distance / _nodeOffset + 2);
        //		_nodes = new Vector3[nodesCount];
        //		_nodeOffsets = new Vector3[nodesCount];
        //		_lineRenderer.SetVertexCount(nodesCount);
        //		//_nodes[nodes.countnodesCount] = _target;
        //		//_lineRenderer.SetPosition(i, _nodes[i] + _nodeOffsets[i]);
        //		
        //		for (int i = 0; i < nodesCount; i++)
        //		{
        //			_nodes[i] = _sourceVec + (direction.normalized * (i * _nodeOffset));
        //			if (i != 0 && i < nodesCount - 1)
        //			{
        //				float _offset = Mathf.PerlinNoise(_nodes[i].x * _arc, _nodes[i].y * _arc) * _jitter * ( 1-Mathf.Abs(_target.position.magnitude/2 - _nodes[i].magnitude) / _target.position.magnitude / 2);
        //				_nodeOffsets[i] = new Vector3(0f, _offset, 0f);
        //			}
        //			else if (i == nodesCount - 1)
        //			{
        //				_nodeOffsets[i] = Vector3.zero;
        //				_nodes[i] = _target.position;
        //			}
        //			_lineRenderer.SetPosition(i, _nodes[i] + _nodeOffsets[i]);
        //		}
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////EXTERNAL
    public void Launch(Transform sourceTransform, Transform targetTransform)
    {
        _transform.parent = sourceTransform;
        _targetTransform = targetTransform;
        //_target.position = target;
        enabled = true;
        //Invoke("Disable", _lifetimeMax);
    }

    /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////INTERNAL
    void GenerateMesh()
    {
        normalRotationQuat = Quaternion.AngleAxis(90, Vector3.forward);
        int nodesCount = (int)((_targetTransform.position - _transform.position).magnitude / _nodeOffset + 2);
        Vector3[] verts = new Vector3[nodesCount * 2];
        Vector2[] uv = new Vector2[nodesCount * 2];
        int[] tris = new int[nodesCount * 6];
        _transform.rotation = Quaternion.identity;
        for (int i = 0; i < nodesCount; i++)
        {
            verts[i] = Vector3.right * (i * _nodeOffset);
            float _offset = (Mathf.PerlinNoise(_lifetime * _speed, verts[i].x * _noise1) * _jitter1 - (_jitter1 * 0.5f)) * (1 - Mathf.Abs((nodesCount / 2f - i) / (nodesCount / 2f)));
            _offset += Mathf.PerlinNoise(_lifetime * _speed, verts[i].x * _noise2) * _jitter2;
            verts[i] += new Vector3(0f, _offset, 0f);
            if ((i / 2) - (int)(i / 2) == 0)
            {
                uv[i] = new Vector2(0f, 0f);
                uv[i] = new Vector2(0f, 1f);
            }
            else
            {
                uv[i + nodesCount] = new Vector2(1f, 0f);
                uv[i + nodesCount] = new Vector2(1f, 1f);
            }
            if (i < nodesCount - 1)
            {
                tris[i * 6 + 0] = i;
                tris[i * 6 + 1] = i + 1;
                tris[i * 6 + 2] = i + nodesCount;
                tris[i * 6 + 3] = i + nodesCount;
                tris[i * 6 + 4] = i + 1;
                tris[i * 6 + 5] = i + nodesCount + 1;
            }
        }
        for (int i = 0; i < nodesCount; i++)
        {
            if (i == 0)
                verts[i + nodesCount] = verts[i] + normalRotationQuat * (verts[i] - verts[i + 1]).normalized * _width;//  GetWidthVector(verts[i] - verts[i + 1], verts[i], verts[i + 1], _width);
            else if (i == nodesCount - 1)
                verts[i + nodesCount] = verts[i] - normalRotationQuat * (verts[i] - verts[i - 1]).normalized * _width;//verts[i + nodesCount] = GetWidthVector(verts[i - 1], verts[i], verts[i] - verts[i - 1], _width);
            else
            {
                //Vector3 rot =  ((GetWidthVector(verts[i - 1], verts[i], verts[i + 1], _width) + new Vector3(0f, 0f, verts[i].z)) - verts[i]);
                verts[i + nodesCount] = GetWidthVector(verts[i - 1], verts[i], verts[i + 1], _width) + new Vector3(0f, 0f, verts[i].z);
            }
        }
        _meshFilter.mesh.vertices = verts;
        _meshFilter.mesh.SetTriangles(tris, 0);
        _meshFilter.mesh.uv = uv;

        _transform.LookAt(_targetTransform, -Camera.main.transform.forward);// Camera.main.transform.position); //Camera.main.transform.position - transform.position);
        _transform.Rotate(60f, -90f, 0f);
        //_transform.rotation = new Quaternion(60f, _transform.rotation.y, _transform.rotation.z, _transform.rotation.w); 
        //_transform.LookAt(_transform.position + _transform.forward, _transform.up + Camera.main.transform.up); //Camera.main.transform.position - transform.position);
        //_transform.position += verts[0] - _transform.position;
    }

    private Vector3 GetWidthVector(Vector3 v1, Vector3 v2, Vector3 v3, float width)
    {
        Vector3 n1 = normalRotationQuat * (v1 - v2).normalized;
        Vector3 n2 = normalRotationQuat * (v2 - v3).normalized;
        return Crossing(v1 + n1 * width, v2 + n1 * width, v2 + n2 * width, v3 + n2 * width);
    }

    private Vector3 Crossing(Vector3 v11, Vector3 v12, Vector3 v21, Vector3 v22)
    {
        float Z = (v12.y - v11.y) * (v21.x - v22.x) - (v21.y - v22.y) * (v12.x - v11.x);
        // float Ca = (v12.y-v11.y)*(v21.x-v11.x)-(v21.y-v11.y)*(v12.x-v11.x);
        float Cb = (v21.y - v11.y) * (v21.x - v22.x) - (v21.y - v22.y) * (v21.x - v11.x);
        if (Z == 0)
            return v12;
        return new Vector3(v11.x + (v12.x - v11.x) * Cb / Z, v11.y + (v12.y - v11.y) * Cb / Z);
    }
}