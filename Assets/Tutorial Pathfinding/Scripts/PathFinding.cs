using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFinding
{
    /// <summary>
	/// Return the path from start to end
	/// </summary>
	/// <param name="start">Start node</param>
	/// <param name="end">End node</param>
	/// <param name="hasDiagonal">If the path has diagonal or not</param>
    public List<Node> FindPath(Node startNode, Node endNode, bool hasDiagonal)
    {
        List<Node> openList = new List<Node>() { startNode };
        List<Node> closeList = new List<Node>();

        while (openList.Count > 0)
        {
            Node currentNode = openList.OrderBy(x => x.F).First();

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            if (currentNode == endNode)
            {
                return GetFinishedPath(startNode, endNode);
            }

            foreach (Node node in GetNeighbors(currentNode, hasDiagonal))
            {
                if (!node.isWalkable || closeList.Contains(node)) continue;

                node.G = GetDistanceNode(currentNode, node, hasDiagonal);
                node.H = GetDistanceNode(endNode, node, hasDiagonal);
                node.previousNode = currentNode;

                if (!openList.Contains(node))
                {
                    openList.Add(node);
                }
            }
        }
        return null;
    }

    private List<Node> GetFinishedPath(Node start, Node end)
    {
        List<Node> finishedList = new List<Node>();
        Node currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previousNode;
        }

        finishedList.Add(start);

        finishedList.Reverse();

        return finishedList;
    }

    private int GetDistanceNode(Node start, Node end, bool hasDiagonal)
    {
        int xDistance = Mathf.Abs(start.location.x - end.location.x);
        int yDistace = Mathf.Abs(start.location.y - end.location.y);

        int moveStraightCost = 10;
        int moveDiagonalCost = 14;

        switch (hasDiagonal)
        {
            case false:
                return xDistance + yDistace;

            case true:
                return moveStraightCost * Mathf.Abs(xDistance - yDistace) + moveDiagonalCost * Mathf.Min(xDistance, yDistace);

        }
    }

    private List<Node> GetNeighbors(Node currentNode, bool hasDiagonal)
    {
        Dictionary<Vector2Int, Node> listNode = GameManager.listNode;
        List<Node> listNeighbors = new List<Node>();
        Vector2Int location;

        //right
        location = new Vector2Int(currentNode.location.x - 1, currentNode.location.y);
        if (listNode.ContainsKey(location))
        {
            listNeighbors.Add(listNode[location]);
        }
        //left
        location = new Vector2Int(currentNode.location.x + 1, currentNode.location.y);
        if (listNode.ContainsKey(location))
        {
            listNeighbors.Add(listNode[location]);
        }

        //top
        location = new Vector2Int(currentNode.location.x, currentNode.location.y + 1);
        if (listNode.ContainsKey(location))
        {
            listNeighbors.Add(listNode[location]);
        }
        //bottom
        location = new Vector2Int(currentNode.location.x, currentNode.location.y - 1);
        if (listNode.ContainsKey(location))
        {
            listNeighbors.Add(listNode[location]);
        }

        if (hasDiagonal)
        {
            //Right Top
            location = new Vector2Int(currentNode.location.x + 1, currentNode.location.y + 1);
            if (listNode.ContainsKey(location))
            {
                listNeighbors.Add(listNode[location]);
            }

            //Left Bottom
            location = new Vector2Int(currentNode.location.x - 1, currentNode.location.y - 1);
            if (listNode.ContainsKey(location))
            {
                listNeighbors.Add(listNode[location]);
            }

            //Right Bottom
            location = new Vector2Int(currentNode.location.x + 1, currentNode.location.y - 1);
            if (listNode.ContainsKey(location))
            {
                listNeighbors.Add(listNode[location]);
            }

            //Left Bottom
            location = new Vector2Int(currentNode.location.x - 1, currentNode.location.y + 1);
            if (listNode.ContainsKey(location))
            {
                listNeighbors.Add(listNode[location]);
            }
        }

        return listNeighbors;
    }
}
