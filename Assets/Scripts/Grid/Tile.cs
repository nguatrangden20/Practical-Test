using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int G, H;
    public int F { get { return H + G; } }

    public bool isBlocked = false;

    public Tile previous;

    public Vector2Int gridLocation;



    private void OnMouseDown() 
    {   
       // Debug.Log(gameObject.name);
    }
}
