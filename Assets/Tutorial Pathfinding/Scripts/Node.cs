using TMPro;
using UnityEngine;

public class Node : MonoBehaviour
{
    public int G = 0, H = 0;
    public int F { get { return G + H; } }

    public Vector2Int location;

    public bool isWalkable;

    public Node previousNode;

    public TextMeshPro posDebug, GDebug, FDebug, HDebug;

    private void OnValidate()
    {
        Common.Warning(posDebug, "Please set PosDebug reference in Node Prefab");
        Common.Warning(GDebug, "Please set GDebug reference in Node Prefab");
        Common.Warning(FDebug, "Please set FDebug reference in Node Prefab");
        Common.Warning(HDebug, "Please set HDebug reference in Node Prefab");
    }
}
