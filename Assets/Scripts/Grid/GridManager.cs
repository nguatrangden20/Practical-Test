using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    [SerializeField]
    private int _width, _height;
    [SerializeField]
    private Tile _tilePrefab;
    [SerializeField]
    private Transform _cammera;

    private Dictionary<Vector2, Tile> _tiles;

    void Start()
    {
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

                _tiles.Add(new Vector2(x,y), spawnedTile);
            }
        }
        
        float _margin = 0.5f;
        _cammera.position = new Vector3((float)_width/2 - _margin, (float)_height/2 - _margin, _cammera.position.z);
    }

    public Tile GetTileAtPosition(Vector2 pos)
    {
        if(_tiles.TryGetValue(pos, out Tile tile))
            return tile;
        
        return null;
    }
}
