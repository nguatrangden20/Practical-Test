using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding 
{
    public List<Tile> FindPath(Tile start, Tile end)
    {
        List<Tile> openList = new List<Tile>();
        List<Tile> closedList = new List<Tile>();

        openList.Add(start);

        if (end.isBlocked)
        {
            return new List<Tile>();
        }

        while (openList.Count > 0)
        {
            Tile currentTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == end)
            {
                return GetFinishedList(start, end);
            }

            if (!start.circle.isGhost)
            {
                foreach (var tile in GetNeighbourTile(currentTile))
                {
                    if (tile.isBlocked || closedList.Contains(tile)) continue;

                    tile.G = GetManhattenDistance(currentTile, tile) + currentTile.G;
                    tile.H = GetManhattenDistance(end, tile);

                    tile.previous = currentTile;

                    if (!openList.Contains(tile))
                    {
                        openList.Add(tile);
                    }
                }
            }
            else
            {
                foreach (var tile in GetNeighbourTile(currentTile))
                {
                    if (closedList.Contains(tile)) continue;

                    tile.G = GetManhattenDistance(start, tile);
                    tile.H = GetManhattenDistance(end, tile);

                    tile.previous = currentTile;

                    if (!openList.Contains(tile))
                    {
                        openList.Add(tile);
                    }
                }

            }
        }

        return new List<Tile>();

    } 

    private int GetManhattenDistance(Tile start, Tile tile)
    {
        return Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - tile.gridLocation.y);
    }

    private List<Tile> GetNeighbourTile(Tile currentTile)
    {
        var listTile = GridManager.Instance.tiles;

        List<Tile> neighbours = new List<Tile>();

        //right
        Vector2Int locationToCheck = new Vector2Int(currentTile.gridLocation.x + 1, currentTile.gridLocation.y);
        if (listTile.ContainsKey(locationToCheck))
        {
            neighbours.Add(listTile[locationToCheck]);
        }

        //left
        locationToCheck = new Vector2Int(currentTile.gridLocation.x - 1, currentTile.gridLocation.y);
        if (listTile.ContainsKey(locationToCheck))
        {
            neighbours.Add(listTile[locationToCheck]);
        }
        //top
        locationToCheck = new Vector2Int(currentTile.gridLocation.x, currentTile.gridLocation.y + 1);
        if (listTile.ContainsKey(locationToCheck))
        {
            neighbours.Add(listTile[locationToCheck]);
        }
        //bottom
        locationToCheck = new Vector2Int(currentTile.gridLocation.x, currentTile.gridLocation.y - 1);
        if (listTile.ContainsKey(locationToCheck))
        {
            neighbours.Add(listTile[locationToCheck]);
        }

        return neighbours;
    }

    private List<Tile> GetFinishedList(Tile start, Tile end)
    {
        List<Tile> finishedList = new List<Tile>();
        Tile currentTile = end;

        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.previous;
        }
        finishedList.Add(start);

        finishedList.Reverse();

        return finishedList;
    }
}
