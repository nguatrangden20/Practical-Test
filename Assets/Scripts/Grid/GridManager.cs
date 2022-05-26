using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager Instance { get { return _instance; } }

    public int width, height;

    [SerializeField]
    private Tile _tilePrefab;

    [SerializeField]
    private Circle _circlePrefab;

    [SerializeField]
    private Transform _cammera;

    [SerializeField]
    private Transform _circleParen;

    public Dictionary<Vector2Int, Tile> tiles;

    public int numberSpawn = 3;
    private const float MINIMUM_SIZE = 0.3f;
    public float largestSize = 0.7f;

    private void OnValidate()
    {
        
    }

    private void Awake()
    {
        if(_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            _instance = this;
        }
    }

    void Start()
    {
        tiles = new Dictionary<Vector2Int, Tile>();
        GenerateGrid();

        SpawnCircle(true);

        this.RegisterListener(EventID.OnStartTurn, (param) => SpawnCircle(false));
        this.RegisterListener(EventID.OnExplosion, (param) => ExplosionCircle((List<Tile>)param));
    }


    #region GenerateGrid
    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Tile spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, this.transform);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.gridLocation = new Vector2Int(x, y);

                tiles.Add(new Vector2Int(x,y), spawnedTile);
            }
        }
        
        float _margin = 0.5f;
        _cammera.position = new Vector3((float)width/2 - _margin, (float)height/2 - _margin, _cammera.position.z);
    }

    public Tile GetTileAtPosition(Vector2Int pos)
    {
        if(tiles.TryGetValue(pos, out Tile tile))
            return tile;
        
        return null;
    }
    #endregion

    #region Game Mechanic
    private void SpawnCircle(bool isLargesSize)
    {
        List<Vector2Int> listLocation = tiles.Keys.Where(x => !tiles[x].isBlocked).ToList();

        if (listLocation.Count <= 1 )
        {
            GameOver();
            return;
        }
        else if (listLocation.Count < numberSpawn)
        {
            numberSpawn = listLocation.Count;
        }

        listLocation.Shuffle();

        for (int i = 0; i < numberSpawn; i++)
        {
            Vector2Int location = listLocation[i];

            Circle spawnedCircle = Instantiate(_circlePrefab, new Vector3(location.x, location.y, _circlePrefab.transform.position.z), Quaternion.identity, _circleParen);
                   spawnedCircle.name = $"Circle {location.x} {location.y}";
                   spawnedCircle.location = location;

            if (isLargesSize)
            {
                spawnedCircle.transform.localScale = new Vector3(largestSize, largestSize, largestSize);
                spawnedCircle.largestSize = true;
            } else
            {
                spawnedCircle.transform.localScale = new Vector3(MINIMUM_SIZE, MINIMUM_SIZE, MINIMUM_SIZE);
                spawnedCircle.largestSize = false;
            }

            tiles[location].circle = spawnedCircle;

        }
    }

    public List<Tile> CheckScore(Tile currentTile)
    {
        List<Tile> listCheck = new List<Tile>();
        List<Tile> finishList = new List<Tile>();
        bool yang = true;
        bool yin = true;
        int x = currentTile.gridLocation.x;
        int y = currentTile.gridLocation.y;
        listCheck.Add(currentTile);
        int maxPoint = Mathf.Max(width, height);

        #region vertical
        for (int i = 1; i < maxPoint -1; i++)
        {
            Vector2Int yangNumber = new Vector2Int(x, y - i);
            Vector2Int yinNumber = new Vector2Int(x, y + i);

            if (tiles.ContainsKey(yangNumber) && yang)
            {
                if (tiles[yangNumber].circle != null && tiles[yangNumber].circle.color == currentTile.circle.color)
                {
                    listCheck.Add(tiles[yangNumber]);
                }
                else yang = false;
            }
            else yang = false;

            if (tiles.ContainsKey(yinNumber) && yin)
            {
                if (tiles[yinNumber].circle != null && tiles[yinNumber].circle.color == currentTile.circle.color)
                {
                    listCheck.Add(tiles[yinNumber]);
                }
                else yin = false;
            }
            else yin = false;

            if (!yin && !yang)
            {
                if (listCheck.Count >= 5)
                {
                    if (finishList.Contains(currentTile))
                        listCheck.Remove(currentTile);

                    finishList.AddRange(listCheck);
                    listCheck.Clear();                    
                }
                else
                    listCheck.Clear();

                break;
            }
        }
        #endregion

        #region horizontal
        listCheck.Add(currentTile);
        yang = true; yin = true;
        for (int i = 1; i < maxPoint -1; i++)
        {
            Vector2Int yangNumber = new Vector2Int(x - i, y);
            Vector2Int yinNumber = new Vector2Int(x + i, y);

            if (tiles.ContainsKey(yangNumber) && yang)
            {
                if (tiles[yangNumber].circle != null && tiles[yangNumber].circle.color == currentTile.circle.color)
                {
                    listCheck.Add(tiles[yangNumber]);
                }
                else yang = false;
            }
            else yang = false;

            if (tiles.ContainsKey(yinNumber) && yin)
            {
                if (tiles[yinNumber].circle != null && tiles[yinNumber].circle.color == currentTile.circle.color)
                {
                    listCheck.Add(tiles[yinNumber]);
                }
                else yin = false;
            }
            else yin = false;

            if (!yin && !yang)
            {
                if (listCheck.Count >= 5)
                {
                    if (finishList.Contains(currentTile))
                        listCheck.Remove(currentTile);

                    finishList.AddRange(listCheck);
                    listCheck.Clear();
                }
                else
                    listCheck.Clear();

                break;
            }
        }
        #endregion

        #region diagonal left
        listCheck.Add(currentTile);
        yang = true; yin = true;
        for (int i = 1; i < maxPoint -1; i++)
        {
            Vector2Int yangNumber = new Vector2Int(x - i, y + i);
            Vector2Int yinNumber = new Vector2Int(x + i , y - i);

            if (tiles.ContainsKey(yangNumber) && yang)
            {
                if (tiles[yangNumber].circle != null && tiles[yangNumber].circle.color == currentTile.circle.color)
                {
                    listCheck.Add(tiles[yangNumber]);
                }
                else yang = false;
            }
            else yang = false;

            if (tiles.ContainsKey(yinNumber) && yin)
            {
                if (tiles[yinNumber].circle != null && tiles[yinNumber].circle.color == currentTile.circle.color)
                {
                    listCheck.Add(tiles[yinNumber]);
                }
                else yin = false;
            }
            else yin = false;

            if (!yin && !yang)
            {
                if (listCheck.Count >= 5)
                {
                    if (finishList.Contains(currentTile))
                        listCheck.Remove(currentTile);

                    finishList.AddRange(listCheck);
                    listCheck.Clear();
                }
                else
                    listCheck.Clear();

                break;
            }
        }
        #endregion

        #region diagonal right
        listCheck.Add(currentTile);
        yang = true; yin = true;
        for (int i = 1; i < maxPoint -1; i++)
        {
            Vector2Int yangNumber = new Vector2Int(x + i, y + i);
            Vector2Int yinNumber = new Vector2Int(x - i , y - i);

            if (tiles.ContainsKey(yangNumber) && yang)
            {
                if (tiles[yangNumber].circle != null && tiles[yangNumber].circle.color == currentTile.circle.color)
                {
                    listCheck.Add(tiles[yangNumber]);
                }
                else yang = false;
            }
            else yang = false;

            if (tiles.ContainsKey(yinNumber) && yin)
            {
                if (tiles[yinNumber].circle != null && tiles[yinNumber].circle.color == currentTile.circle.color)
                {
                    listCheck.Add(tiles[yinNumber]);
                }
                else yin = false;
            }
            else yin = false;

            if (!yin && !yang)
            {
                if (listCheck.Count >= 5)
                {
                    if (finishList.Contains(currentTile))
                        listCheck.Remove(currentTile);

                    finishList.AddRange(listCheck);
                    listCheck.Clear();
                }
                else
                    listCheck.Clear();

                break;
            }
        }
        #endregion

        Common.Log(finishList.Count);
        return finishList;
    }

    private void ExplosionCircle(List<Tile> list)
    {
        foreach (var item in list)
        {
            Destroy(item.circle.gameObject);            
        }
    }

    private void GameOver()
    {
        Common.Log("GameOver");
    }
    #endregion
}
