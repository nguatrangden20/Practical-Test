using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Circle : MonoBehaviour
{
    [HideInInspector]
    public Color color;

    private SpriteRenderer _spriteRenderer;

    [HideInInspector]
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

    public bool isGhost;

    [HideInInspector]
    public bool largestSize;

    Action<object> _OnReceiveEventRef;

    void Start()
    {
        if (Random.Range(1, 10) == 1)
        {
            isGhost = true;
        }
        explosion = GetComponentInChildren<ParticleSystem>();

        _OnReceiveEventRef = (param) => GrowUp();

        color = _colorList[Random.Range(0, _colorList.Length)];

        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.color = color;

        if(!largestSize)
        {
            this.RegisterListener(EventID.OnEndTurn, _OnReceiveEventRef);
        }

        if (isGhost)
        {
            SpriteRenderer r = gameObject.GetComponent<SpriteRenderer>();

            LeanTween.value(gameObject, 0, 1, 1).setOnUpdate((float val) =>
            {
                Color c = r.color;
                c.a = val;
                r.color = c;
            }).setLoopPingPong();
        }
    }

    bool hasDestroy = false;
    private void GrowUp()
    {
        this.RemoveListener(EventID.OnEndTurn, _OnReceiveEventRef);

        CheckCircleTile();

        if (hasDestroy) return;

        largestSize = true;
        float size = GridManager.Instance.largestSize;
        LeanTween.scale(gameObject, new Vector3(size, size, size), 0.5f);

        Tile currentTile = GridManager.Instance.GetTileAtPosition(location);
        List<Tile> listScore = GridManager.Instance.CheckScore(currentTile);
        if (listScore.Count >= 5)
        {
            this.PostEvent(EventID.OnExplosion, listScore);
        }
    }

    private void CheckCircleTile()
    {
        Tile currentTile = GridManager.Instance.GetTileAtPosition(location);
        if (currentTile.isBlocked)
        {
            hasDestroy = true;
            Destroy(gameObject);
        }
    }
}
