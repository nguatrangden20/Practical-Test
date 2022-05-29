using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Node _nodePrefab;

    [SerializeField]
    private int _width, _height;

    [SerializeField]
    private Transform _camera;

    /// <summary>
    ///  Store all Node
    /// </summary>
    public static Dictionary<Vector2Int, Node> listNode;

    private void OnValidate()
    {
        Common.Warning(_nodePrefab, "Please set Node Prefab reference in GameManger");
        Common.Warning(_camera, "Please set _camera reference in GameManger");        
    }

    void Start()
    {
        listNode = new Dictionary<Vector2Int, Node>();
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Node spawnNode = Instantiate(_nodePrefab, new Vector2(x, y), Quaternion.identity, transform);
                spawnNode.name = $"Node {x},{y}";
                spawnNode.location = new Vector2Int(x, y);
                spawnNode.isWalkable = true;

                if (spawnNode.posDebug)
                    spawnNode.posDebug.text = $"{x}, {y}";
                else
                {
                    spawnNode.posDebug = spawnNode.GetComponentInChildren<TextMeshPro>();
                    spawnNode.posDebug.text = $"{x}, {y}";
                }

                listNode.Add(new Vector2Int(x, y) ,spawnNode);
            }
        }

        float _margin = 0.5f;
        _camera.position = new Vector3((float)_width / 2 - _margin, (float)_height / 2 - _margin, _camera.position.z);
    }
}
