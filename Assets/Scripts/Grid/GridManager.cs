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
    List<Vector2Int> listLocation;

    public const int NUMBER_SPAWN = 3;

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
        listLocation = new List<Vector2Int>(tiles.Keys);

        FirstTurn();
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
    private void FirstTurn()
    {
        listLocation.Shuffle();

        for (int i = 0; i < NUMBER_SPAWN; i++)
        {
            Vector2Int location = listLocation[i];

            Circle spawnedCircle = Instantiate(_circlePrefab, new Vector3(location.x, location.y, _circlePrefab.transform.position.z), Quaternion.identity, _circleParen);
                   spawnedCircle.name = $"Circle {location.x} {location.y}";

            tiles[location].circle = spawnedCircle;
        }
    }
    #endregion
}
