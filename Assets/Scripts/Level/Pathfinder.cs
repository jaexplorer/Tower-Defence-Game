using UnityEngine;
using System.Collections.Generic;

public class Pathfinder : CustomBehaviour
{
    [SerializeField] private Level _level;
    [SerializeField] private TileCurve _forwardCurve;
    [SerializeField] private TileCurve _backwardCurve;
    [SerializeField] private TileCurve _leftCurve;
    [SerializeField] private TileCurve _rightCurve;
    [SerializeField] private TileCurve _forwardLeftCurve;
    [SerializeField] private TileCurve _forwardRightCurve;
    [SerializeField] private TileCurve _backwardLeftCurve;
    [SerializeField] private TileCurve _backwardRightCurve;
    [SerializeField] private TileCurve _leftForwardCurve;
    [SerializeField] private TileCurve _leftBackwardCurve;
    [SerializeField] private TileCurve _rightForwardCurve;
    [SerializeField] private TileCurve _rightBackwardCurve;

    private Node[,] _nodes;
    private Node _portalNode;
    private Node _spawnerNode;
    private List<Node> _pathNodes = new List<Node>(100);
    private List<Node> _pathNodesTemp = new List<Node>(100);
    private PathState _pathState;

    public PathState pathState { get { return _pathState; } }

    //EVENTS///////////////////////////////////////////////////
    protected override void OnTilesChange()
    {
        FindPath();
    }

    protected override void OnLevelLoad()
    {
        // Initiating.
        _nodes = new Node[_level.sizeX, _level.sizeZ];

        // Creating nodes.
        for (int x = 0; x < _nodes.GetLength(0); x++)
        {
            for (int z = 0; z < _nodes.GetLength(1); z++)
            {
                Tile tile = _level.GetTile(x, z);
                if (tile != null)
                {
                    // Creating node.
                    Node node = new Node();
                    _nodes[x, z] = node;

                    // Connecting node to tile.
                    node.tile = tile;

                    // Looking for portal.
                    if (tile.isPortal)
                    {
                        _portalNode = node;
                    }

                    // Looking for spawner.
                    if (tile.isSpawner)
                    {
                        _spawnerNode = node;
                    }
                }
            }
        }
        if (_portalNode == null)
        {
            Debug.Log("PORTAL NOT FOUND");
        }
        if (_spawnerNode == null)
        {
            Debug.Log("SPAWNER NOT FOUND");
        }
    }

    //PUBLIC///////////////////////////////////////////////////
    public PathState TestPath(Tile blockTile)
    {
        return FindPath(false, blockTile);
    }

    public PathState FindPath()
    {
        ClearPath();
        PathState result = FindPath(true, null);
        // EventManager.onPathChanged.Invoke();
        // Debug.Log(result);
        DrawDebugPath();
        return result;
    }

    public void HidePath()
    {
        //_trail.Hide();
        // for (int i = 0; i < _pathNodes.Count; i++)
        // {
        //     _pathNodes[i].tile.gameObject.GetComponentInChildren<MeshRenderer>().material = _defaultMaterial;
        // }
    }

    public void ShowPath()
    {
        // for (int i = 0; i < _pathNodes.Count; i++)
        // {
        //     _pathNodes[i].tile.gameObject.GetComponentInChildren<MeshRenderer>().material = _highlightMaterial;
        // }
        // _showPath = true;
        // //_trail = PoolManager.Produce(_trailPrefab).GetComponent<Trail>();
        // //_trail.Show();
    }

    public void DrawDebugPath()
    {
        Node currentNode = _spawnerNode;
        while (currentNode != null)
        {
            // Debug.Log(currentNode.tile);
            if (currentNode.tile != null && currentNode.next != null && currentNode.next.tile)
            {
                Debug.DrawLine(currentNode.tile.position.ToVector3(), currentNode.next.tile.position.ToVector3(), Color.black, 100, false);
                currentNode = currentNode.next;
            }
            else
            {
                return;
            }
        }
    }



    // [MenuItem("MyMenu/Precalculate Tile Curves")]
    public static void Precalculate()
    {
        GameObject.FindObjectOfType<Pathfinder>().PrecalculateCurves();
        Debug.Log("Curves calculated.");
    }

    public void PrecalculateCurves()
    {
        _forwardCurve.PrecalculatePoints();
        _backwardCurve.PrecalculatePoints();
        _leftCurve.PrecalculatePoints();
        _rightCurve.PrecalculatePoints();
        _backwardCurve.PrecalculatePoints();
        _forwardLeftCurve.PrecalculatePoints();
        _forwardRightCurve.PrecalculatePoints();
        _backwardLeftCurve.PrecalculatePoints();
        _backwardRightCurve.PrecalculatePoints();
        _leftForwardCurve.PrecalculatePoints();
        _leftBackwardCurve.PrecalculatePoints();
        _rightForwardCurve.PrecalculatePoints();
        _rightBackwardCurve.PrecalculatePoints();
    }

    //PRIVATE//////////////////////////////////////////////////
    private PathState FindPath(bool apply, Tile blockTile)
    {
        // Result.
        PathState result = PathState.Unfinished;

        // Resetting all nodes.
        for (int x = 0; x < _nodes.GetLength(0); x++)
        {
            for (int z = 0; z < _nodes.GetLength(1); z++)
            {
                if (_nodes[x, z] != null)
                {
                    _nodes[x, z].Reset();
                }
            }
        }

        // Creating temporary node lists.
        List<Node> nodesToUpdate = new List<Node>(128);
        List<Node> newNodesToUpdate = new List<Node>(128);

        // Adding root node.
        nodesToUpdate.Add(_portalNode);

        // Distance from portal.
        int distance = 1;

        // Finding distances from portal to each accessible tile.
        while (nodesToUpdate.Count > 0)
        {
            // Getting next node.
            Node currentNode = nodesToUpdate[0];
            nodesToUpdate.RemoveAt(0);
            // Checking adjacent nodes and connecting.
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if ((x != 0 && z == 0) || (x == 0 && z != 0))
                    {
                        // Getting adjacent node.
                        Node adjacentNode = GetNode(currentNode.tile.position.x + x, currentNode.tile.position.z + z);

                        // If node is accessible, setting it up.
                        if (adjacentNode != null && !adjacentNode.isConnected && adjacentNode.tile.walkable && (blockTile == null || blockTile != adjacentNode.tile))
                        {
                            adjacentNode.isConnected = true;
                            adjacentNode.distance = distance;
                            newNodesToUpdate.Add(adjacentNode);

                            // Connecting teleport.
                            if (adjacentNode.tile.isTeleport)
                            {
                                Tile linkedTile = adjacentNode.tile.teleport.pair.tile;
                                Node linkedNode = _nodes[linkedTile.position.x, linkedTile.position.z];
                                if (!linkedNode.isConnected)
                                {
                                    adjacentNode.teleportNode = linkedNode;
                                    linkedNode.teleportNode = adjacentNode;
                                    newNodesToUpdate.Add(linkedNode);
                                    linkedNode.distance = distance;
                                    linkedNode.isConnected = true;
                                }
                            }
                        }
                    }
                }
            }
            // When all nodes are checked, taking next generation for the check.
            if (nodesToUpdate.Count <= 0)
            {
                nodesToUpdate.AddRange(newNodesToUpdate);
                newNodesToUpdate.Clear();
                distance++;
            }
        }

        // If spawner and portal are connected.
        if (_spawnerNode.isConnected)
        {
            result = PathState.Clear;
            // Building and validating path.
            Node currentNode = _spawnerNode;
            int order = 0;
            while (currentNode != null)
            {
                int nodeX = currentNode.tile.position.x;
                int nodeZ = currentNode.tile.position.z;

                // Getting adjecent nodes.
                Node forwardNode = GetNode(nodeX, nodeZ + 1);
                Node backwardNode = GetNode(nodeX, nodeZ - 1);
                Node leftNode = GetNode(nodeX - 1, nodeZ);
                Node rightNode = GetNode(nodeX + 1, nodeZ);

                int smallestDistance = 999;

                if (forwardNode != null && forwardNode.isConnected)
                {
                    smallestDistance = forwardNode.distance;
                    if (currentNode.pathDirection == Direction.None)
                    {
                        currentNode.pathDirection = Direction.Forward;
                    }
                }
                if (backwardNode != null && backwardNode.isConnected && backwardNode.distance < smallestDistance)
                {
                    smallestDistance = backwardNode.distance;
                    if (currentNode.pathDirection == Direction.None)
                    {
                        currentNode.pathDirection = Direction.Backward;
                    }
                }
                if (leftNode != null && leftNode.isConnected && leftNode.distance < smallestDistance)
                {
                    smallestDistance = leftNode.distance;
                    if (currentNode.pathDirection == Direction.None)
                    {
                        currentNode.pathDirection = Direction.Left;
                    }
                }
                if (rightNode != null && rightNode.isConnected && rightNode.distance < smallestDistance)
                {
                    smallestDistance = rightNode.distance;
                    if (currentNode.pathDirection == Direction.None)
                    {
                        currentNode.pathDirection = Direction.Right;
                    }
                }

                // Connecting teleport.
                if (currentNode != null && currentNode.teleportNode != null && currentNode.teleportNode.distance <= smallestDistance)
                {
                    currentNode.next = currentNode.teleportNode;
                    if (currentNode.pathDirection == Direction.Forward)
                    {
                        currentNode.tile.curve = _forwardCurve;
                    }
                    else if (currentNode.pathDirection == Direction.Backward)
                    {
                        currentNode.tile.curve = _backwardCurve;
                    }
                    else if (currentNode.pathDirection == Direction.Left)
                    {
                        currentNode.tile.curve = _leftCurve;
                    }
                    else if (currentNode.pathDirection == Direction.Right)
                    {
                        currentNode.tile.curve = _rightCurve;
                    }
                }
                // Finding next path node prefering same direction.
                else if (currentNode.pathDirection == Direction.Forward)
                {
                    //Debug.DrawLine(currentNode.tile.transform.position, currentNode.tile.transform.position + Vector3.forward, Color.yellow, 99999f, true);
                    if (forwardNode != null && forwardNode.distance == smallestDistance)
                    {
                        currentNode.next = forwardNode;
                        currentNode.next.pathDirection = Direction.Forward;
                        currentNode.tile.curve = _forwardCurve;
                    }
                    else if (leftNode != null && leftNode.distance == smallestDistance)
                    {
                        if (rightNode != null && rightNode.distance == smallestDistance)
                        {
                            // result = PathState.Split;
                            // print("PATH INVALID EQUAL");
                        }
                        currentNode.next = leftNode;
                        currentNode.next.pathDirection = Direction.Left;
                        currentNode.tile.curve = _forwardLeftCurve;
                    }
                    else if (rightNode != null)
                    {
                        currentNode.next = rightNode;
                        currentNode.next.pathDirection = Direction.Right;
                        currentNode.tile.curve = _forwardRightCurve;
                    }
                    else
                    {
                        currentNode.tile.curve = _forwardRightCurve;
                    }
                }
                else if (currentNode.pathDirection == Direction.Backward)
                {
                    //Debug.DrawLine(currentNode.tile.transform.position, currentNode.tile.transform.position + Vector3.back, Color.yellow, 99999f, true);
                    if (backwardNode != null && backwardNode.distance == smallestDistance)
                    {
                        currentNode.next = backwardNode;
                        currentNode.next.pathDirection = Direction.Backward;
                        currentNode.tile.curve = _backwardCurve;
                    }
                    else if (rightNode != null && rightNode.distance == smallestDistance)
                    {
                        if (leftNode != null && leftNode.distance == smallestDistance)
                        {
                            // result = PathState.Split;
                        }
                        currentNode.next = rightNode;
                        currentNode.next.pathDirection = Direction.Right;
                        currentNode.tile.curve = _backwardRightCurve;
                    }
                    else if (leftNode != null)
                    {
                        currentNode.next = leftNode;
                        currentNode.next.pathDirection = Direction.Left;
                        currentNode.tile.curve = _backwardLeftCurve;
                    }
                    else
                    {
                        currentNode.tile.curve = _backwardCurve;
                    }
                }
                else if (currentNode.pathDirection == Direction.Left)
                {
                    //Debug.DrawLine(currentNode.tile.transform.position, currentNode.tile.transform.position + Vector3.left, Color.yellow, 99999f, true);
                    if (leftNode != null && leftNode.distance == smallestDistance)
                    {
                        currentNode.next = leftNode;
                        currentNode.next.pathDirection = Direction.Left;
                        currentNode.tile.curve = _leftCurve;
                    }
                    else if (backwardNode != null && backwardNode.distance == smallestDistance)
                    {
                        if (forwardNode != null && forwardNode.distance == smallestDistance)
                        {
                            // result = PathState.Split;
                            // print("PATH INVALID EQUAL");
                        }
                        currentNode.next = backwardNode;
                        currentNode.next.pathDirection = Direction.Backward;
                        currentNode.tile.curve = _leftBackwardCurve;
                    }
                    else if (forwardNode != null)
                    {
                        currentNode.next = forwardNode;
                        currentNode.next.pathDirection = Direction.Forward;
                        currentNode.tile.curve = _leftForwardCurve;
                    }
                    else
                    {
                        currentNode.tile.curve = _leftCurve;
                    }
                }
                else if (currentNode.pathDirection == Direction.Right)
                {
                    //Debug.DrawLine(currentNode.tile.transform.position, currentNode.tile.transform.position + Vector3.right, Color.yellow, 99999f, true);
                    if (rightNode != null && rightNode.distance == smallestDistance)
                    {
                        currentNode.next = rightNode;
                        currentNode.next.pathDirection = Direction.Right;
                        currentNode.tile.curve = _rightCurve;
                    }
                    else if (forwardNode != null && forwardNode.distance == smallestDistance)
                    {
                        if (backwardNode != null && backwardNode.distance == smallestDistance)
                        {
                            // result = PathState.Split;
                            // print("PATH INVALID EQUAL");
                        }
                        currentNode.next = forwardNode;
                        currentNode.next.pathDirection = Direction.Forward;
                        currentNode.tile.curve = _rightForwardCurve;
                    }
                    else if (backwardNode != null)
                    {
                        currentNode.next = backwardNode;
                        currentNode.next.pathDirection = Direction.Backward;
                        currentNode.tile.curve = _rightBackwardCurve;
                    }
                    else
                    {
                        currentNode.tile.curve = _rightCurve;
                    }
                }
                if (currentNode.next != null)
                {
                    Debug.DrawLine(currentNode.tile.position.ToVector3() + Vector3.up * 0.2f, currentNode.next.tile.position.ToVector3() + Vector3.up * 0.2f, Color.black, 20, true);
                    currentNode.next.previous = currentNode;
                }

                if (apply)
                {
                    _pathNodes.Add(currentNode);
                    currentNode.tile.pathOrder = order;
                    currentNode.tile.isPath = true;
                    currentNode.tile.pathDirection = currentNode.pathDirection;
                    order++;

                    if (currentNode.next != null)
                    {
                        currentNode.tile.next = currentNode.next.tile;
                    }
                    if (currentNode.previous != null)
                    {
                        currentNode.tile.previous = currentNode.previous.tile;
                    }
                }
                else
                {
                    _pathNodesTemp.Add(currentNode);
                }
                currentNode = currentNode.next;
            }

        }
        else
        {
            result = PathState.Blocked;
        }
        if (apply)
        {

            _pathState = result;
        }
        // Debug.Log(pathState);
        return result;
    }

    private void ClearPath()
    {
        for (int i = 0; i < _pathNodes.Count; i++)
        {
            _pathNodes[i].tile.ClearPath();
        }
        _pathNodes.Clear();
    }

    private Node GetNode(int x, int z)
    {
        if (x >= 0 && x < _level.sizeX && z >= 0 && z < _level.sizeZ)
        {
            return _nodes[x, z];
        }
        else
        {
            return null;
        }
    }

    private class Node
    {
        public Tile tile;
        public Node next;
        public Node previous;
        public Node blink;
        public Node teleportNode;
        public Direction pathDirection;
        public int distance;
        public bool isConnected;
        public bool isPath;

        public void Reset()
        {
            next = null;
            previous = null;
            blink = null;
            pathDirection = Direction.None;
            distance = 0;
            isConnected = false;
            isPath = false;
            teleportNode = null;
        }
    }
}

// public class Path
// {
//     public List<Tile> _tiles;
//     public List<Tile> _nodeTiles;
// }

public enum PathState
{
    Unfinished,
    Clear,
    Blocked,
    Split,
}