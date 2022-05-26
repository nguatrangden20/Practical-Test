using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Circle : MonoBehaviour
{
    public Color color;

    private SpriteRenderer _spriteRenderer;

    public Vector2Int location;

    private ParticleSystem explosion;

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

    Action<object> _OnReceiveEventRef;

    void Start()
    {
        explosion = GetComponentInChildren<ParticleSystem>();

        _OnReceiveEventRef = (param) => GrowUp();

        color = _colorList[Random.Range(0, _colorList.Length)];

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = color;

        if(!largestSize)
        {
            this.RegisterListener(EventID.OnEndTurn, _OnReceiveEventRef);
        }
    }

    private void GrowUp()
    {
        Tile currentTile = GridManager.Instance.GetTileAtPosition(location);
        if (currentTile.isBlocked)
            Destroy(gameObject);

        largestSize = true;
        float size = GridManager.Instance.largestSize;
        LeanTween.scale(gameObject, new Vector3(size, size, size), 0.5f);

        List<Tile> listScore = GridManager.Instance.CheckScore(currentTile);
        if (listScore.Count >= 5)
        {
            this.PostEvent(EventID.OnExplosion, listScore);
        }

        this.RemoveListener(EventID.OnEndTurn, _OnReceiveEventRef);
    }

    private void OnDestroy()
    {
        this.RemoveListener(EventID.OnEndTurn, _OnReceiveEventRef);
    }

}
