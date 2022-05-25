using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static GridManager _instance;
    public static GridManager Instance { get { return _instance; } }

    [SerializeField]
    private int _width, _height;

    [SerializeField]
    private Tile _tilePrefab;

    [SerializeField]
    private Transform _cammera;

    public Dictionary<Vector2, Tile> tiles;

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
        tiles = new Dictionary<Vector2, Tile>();
        GenerateGrid();
    }


    private void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile spawnedTile = Instantiate(_tilePrefab, new Vector3(x, y), Quaternion.identity, this.transform);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.gridLocation = new Vector2Int(x, y);

                tiles.Add(new Vector2Int(x,y), spawnedTile);
            }
        }
        
        float _margin = 0.5f;
        _cammera.position = new Vector3((float)_width/2 - _margin, (float)_height/2 - _margin, _cammera.position.z);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if(tiles.TryGetValue(pos, out Tile tile))
            return tile;
        
        return null;
    }
}
