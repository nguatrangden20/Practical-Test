using UnityEngine;

public class Tile : MonoBehaviour
{
    [HideInInspector]
    public int G = 0, H;
    public int F { get { return H + G; } }
    [SerializeField]
    public bool isBlocked { get { return (circle != null  && circle.largestSize)? true : false ; } }

    public Tile previous;

    public Vector2Int gridLocation;

    public Circle circle;


}
