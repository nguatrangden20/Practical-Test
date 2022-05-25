using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public Color color;

    private SpriteRenderer _spriteRenderer;

    private Color[] _colorList = new Color[]
    {
        Color.cyan,
        Color.blue,
        Color.green,
        Color.red,
        Color.yellow,
        Color.magenta
    };

    void Start()
    {
        color = _colorList[Random.Range(0, _colorList.Length)];

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = color;
    }

}
