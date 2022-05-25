using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour
{
    public Color color;

    private SpriteRenderer _spriteRenderer;

    public Vector2Int location;

    private Color[] _colorList = new Color[]
    {
        Color.cyan,
        Color.blue,
        Color.green,
        Color.red,
        Color.yellow,
        Color.magenta
    };

    public bool largestSize;

    void Start()
    {
        color = _colorList[Random.Range(0, _colorList.Length)];

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = color;

        if(!largestSize)
            this.RegisterListener(EventID.OnEndTurn, (param) => GrowUp());
    }

    private void GrowUp()
    {
        largestSize = true;
        float size = GridManager.Instance.largestSize;
        LeanTween.scale(gameObject, new Vector3(size, size, size), 0.5f);

        this.RemoveListener(EventID.OnEndTurn, (param) => GrowUp());
    }

}
