using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Astar
{
    private List<Node> openList;
    private List<Node> closedList;
    private List<Vector2Int> path;
    private Node[,] checkedNodes;
    private int width, height;
    private Node currentNode;
    private List<Node> neighbours;

    [SerializeField] private GameObject maze;

    /// <summary>
    /// TODO: Implement this function so that it returns a list of Vector2Int positions which describes a path
    /// Note that you will probably need to add some helper functions
    /// from the startPos to the endPos
    /// </summary>
    /// <param name="startPos"></param>
    /// <param name="endPos"></param>
    /// <param name="grid"></param>
    /// <returns></returns>
    public List<Vector2Int> FindPathToTarget(Vector2Int startPos, Vector2Int endPos, Cell[,] grid)
    {
        openList = new List<Node>();
        closedList = new List<Node>();
        path = new List<Vector2Int>();

        width = grid.GetLength(0);
        height = grid.GetLength(1);
        checkedNodes = new Node[width, height];
        
        openList.Add(new Node(startPos, null, 0, (endPos - startPos).magnitude));

        while (openList.Count > 0)
        {
            currentNode = GetLowestFNode();
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            if (currentNode.position == endPos)
            {
                return Path();
            }

            neighbours = new List<Node>();
            if (!grid[currentNode.position.x, currentNode.position.y].HasWall(Wall.DOWN)) {
                Vector2Int downNeigbourPos = currentNode.position + new Vector2Int(0, -1);
                if (checkedNodes[downNeigbourPos.x, downNeigbourPos.y] == null)
                {
                    checkedNodes[downNeigbourPos.x, downNeigbourPos.y] = new Node(downNeigbourPos, null,Mathf.Infinity, (endPos - downNeigbourPos).magnitude);
                }
                neighbours.Add(checkedNodes[downNeigbourPos.x, downNeigbourPos.y]);
            }
            if (!grid[currentNode.position.x, currentNode.position.y].HasWall(Wall.UP)) {
                Vector2Int upNeigbourPos = currentNode.position + new Vector2Int(0, 1);
                if (checkedNodes[upNeigbourPos.x, upNeigbourPos.y] == null)
                {
                    checkedNodes[upNeigbourPos.x, upNeigbourPos.y] = new Node(upNeigbourPos, null, Mathf.Infinity, (endPos - upNeigbourPos).magnitude);
                }
                neighbours.Add(checkedNodes[upNeigbourPos.x, upNeigbourPos.y]);
            }
            if (!grid[currentNode.position.x, currentNode.position.y].HasWall(Wall.RIGHT)) {
                Vector2Int rightNeigbourPos = currentNode.position + new Vector2Int(1, 0);
                if (checkedNodes[rightNeigbourPos.x, rightNeigbourPos.y] == null)
                {
                    checkedNodes[rightNeigbourPos.x, rightNeigbourPos.y] = new Node(rightNeigbourPos, null, Mathf.Infinity, (endPos - rightNeigbourPos).magnitude);
                }
                neighbours.Add(checkedNodes[rightNeigbourPos.x, rightNeigbourPos.y]);
            }
            if (!grid[currentNode.position.x, currentNode.position.y].HasWall(Wall.LEFT)) {
                Vector2Int leftNeigbourPos = currentNode.position + new Vector2Int(-1, 0);
                if (checkedNodes[leftNeigbourPos.x, leftNeigbourPos.y] == null)
                {
                    checkedNodes[leftNeigbourPos.x, leftNeigbourPos.y] = new Node(leftNeigbourPos, null, Mathf.Infinity, (endPos - leftNeigbourPos).magnitude);
                }
                neighbours.Add(checkedNodes[leftNeigbourPos.x, leftNeigbourPos.y]);
            }

            foreach (Node neighbour in neighbours)
            {
                float GTentative = currentNode.GScore + 1;
                if (GTentative < neighbour.GScore)
                {
                    neighbour.parent = currentNode;
                    neighbour.GScore = GTentative;
                    if (!openList.Contains(neighbour) && !closedList.Contains(neighbour))
                    {
                        openList.Add(neighbour);
                    }
                }
            }

        }

        return null;
    }

    private Node GetLowestFNode()
    {
        Node _lowestFNode = new Node();
        float _lowestF = Mathf.Infinity;

        foreach (Node node in openList)
        {
            if (node.FScore < _lowestF)
            {
                _lowestFNode = node;
                _lowestF = node.FScore;
            }
        }
        return _lowestFNode;
    }

    private List<Vector2Int> Path()
    {
        do
        {
            path.Add(currentNode.position);
            currentNode = currentNode.parent;
        }
        while (currentNode.parent != null);

        path.Reverse();
        return path;
    }

    /// <summary>
    /// This is the Node class you can use this class to store calculated FScores for the cells of the grid, you can leave this as it is
    /// </summary>
    public class Node
    {
        public Vector2Int position; //Position on the grid
        public Node parent; //Parent Node of this node

        public float FScore { //GScore + HScore
            get { return GScore + HScore; }
        }
        public float GScore; //Current Travelled Distance
        public float HScore; //Distance estimated based on Heuristic

        public Node() { }
        public Node(Vector2Int position, Node parent, float GScore, float HScore)
        {
            this.position = position;
            this.parent = parent;
            this.GScore = GScore;
            this.HScore = HScore;
        }
    }
}
