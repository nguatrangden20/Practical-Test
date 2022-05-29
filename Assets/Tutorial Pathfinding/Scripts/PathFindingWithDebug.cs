using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PathFindingWithDebug
{
    /// <summary>
	/// Return the path from start to end
	/// </summary>
	/// <param name="start">Start node</param>
	/// <param name="end">End node</param>
	/// <param name="hasDiagonal">If the path has diagonal or not</param>
	/// <param name="debug">Show debug</param>
	/// <param name="callBack">Call back where the path return</param>
    public IEnumerator FindPath(Node start, Node end, bool hasDiagonal, bool debug = false, Action<List<Node>> callBack = null)
    {
        if (debug)
        {
            start.GDebug.text = "0";
            start.HDebug.text = GetDistanceNode(start, end, hasDiagonal).ToString();
            start.FDebug.text = start.HDebug.text;

            start.GDebug.transform.parent.gameObject.SetActive(true);
            start.posDebug.gameObject.SetActive(false);
        }

        WaitForSeconds delaytime = new WaitForSeconds(0.2f);
        
        List<Node> openList = new List<Node>() { start };
        List<Node> closeList = new List<Node>();

        while (openList.Count > 0)
        {
            Node currentNode = openList.OrderBy(x => x.F).First();

            openList.Remove(currentNode);
            closeList.Add(currentNode);

            if (debug)
            {
                // Change Color Node in Open List
                currentNode.GetComponent<SpriteRenderer>().color = Color.green;

                // Change Color Node in Close List
                if (currentNode.previousNode != null)
                currentNode.previousNode.GetComponent<SpriteRenderer>().color = Color.red;

                yield return delaytime;
            }

            if (currentNode == end)
            {
                if (callBack != null)
                    callBack(GetFinishedList(start, end));

                // Change Color Finish Path
                if (debug)
                {
                    foreach (var item in GetFinishedList(start, end))
                    {
                        item.GetComponent<SpriteRenderer>().color = Color.magenta;
                        yield return new WaitForSeconds(0.1f);
                    }
                }

                break;
            }

            foreach (Node node in GetnNeighbors(currentNode, hasDiagonal))
            {
                if (!node.isWalkable || closeList.Contains(node)) continue;

                node.G = GetDistanceNode(currentNode, node, hasDiagonal);
                node.H = GetDistanceNode(end, node, hasDiagonal);
                node.previousNode = currentNode;

                //NeighBors Node
                if (debug)
                {
                    node.GDebug.transform.parent.gameObject.SetActive(true);
                    node.posDebug.gameObject.SetActive(false);

                    node.GDebug.text = node.G.ToString();
                    node.HDebug.text = node.H.ToString();
                    node.FDebug.text = node.F.ToString();

                    node.GetComponent<SpriteRenderer>().color = Color.blue;
                    yield return delaytime;
                }

                if(!openList.Contains(node))
                {
                    openList.Add(node);
                }

                yield return null;
            }
            
            yield return null;
        }
    }

    private List<Node> GetFinishedList(Node start, Node end)
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
        Debug.Log(start);
        Debug.Log(end);

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

    private List<Node> GetnNeighbors(Node currentNode, bool hasDiagonal)
    {
        Dictionary<Vector2Int, Node> listNode = GameManager.listNode;
        List<Node> listNeighbors = new List<Node>();
        Vector2Int location;

        //right
        location = new Vector2Int(currentNode.location.x - 1, currentNode.location.y);
        if(listNode.ContainsKey(location))
        {
            listNeighbors.Add(listNode[location]);
        }
        //left
        location = new Vector2Int(currentNode.location.x + 1, currentNode.location.y);
        if(listNode.ContainsKey(location))
        {
            listNeighbors.Add(listNode[location]);
        }

        //top
        location = new Vector2Int(currentNode.location.x, currentNode.location.y + 1);
        if(listNode.ContainsKey(location))
        {
            listNeighbors.Add(listNode[location]);
        }
        //bottom
        location = new Vector2Int(currentNode.location.x, currentNode.location.y - 1);
        if(listNode.ContainsKey(location))
        {
            listNeighbors.Add(listNode[location]);
        }

        if(hasDiagonal)
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
