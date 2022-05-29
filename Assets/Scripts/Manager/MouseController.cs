using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{
    public Circle _circle;
    public Tile _endTile;
    public Tile _startTile;
    public float speed;

    private bool onProgress = false;

    private Pathfinding pathFinding;
    private List<Tile> path;
    float size;

    bool stopInteract = false;

    private void Start()
    {
        pathFinding = new Pathfinding();
        path = new List<Tile>();
        size = GridManager.Instance.largestSize;

        this.RegisterListener(EventID.OnGameOver, x => stopInteract = true);
    }

    void Update()
    {
        if (!stopInteract)
            MouseInteract();
    }

    private void MouseInteract()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                Tile currentTile = hit.collider.GetComponent<Tile>();

                if (_circle == null)
                {
                    if (!currentTile.isBlocked)
                    {
                        return;
                    }
                    _circle = currentTile.circle;

                    LeanTween.scale(_circle.gameObject, new Vector3(size + 0.3f, size + 0.3f, size + 0.3f), 0.5f).setLoopPingPong();

                    AudioManager.instance.Play("TouchCircle");
                }
                else if (!onProgress)
                {
                    onProgress = true;
                    _endTile = currentTile;

                    path = pathFinding.FindPath(GridManager.Instance.GetTileAtPosition(_circle.location), currentTile);

                    if (path.Count == 1 || path.Count == 0)
                    {
                        LeanTween.cancel(_circle.gameObject);
                        LeanTween.scale(_circle.gameObject, new Vector3(size, size, size), 0.25f);

                        FadeInOut(_circle);

                        AudioManager.instance.Play("CancelSlect");
                        path.Clear();
                        _circle = null;
                        onProgress = false;
                    }
                    else
                    {
                        for (int i = 0; i < path.Count - 1; i++)
                        {
                            Vector3 start = new Vector3(path[i].gridLocation.x, path[i].gridLocation.y);
                            Vector3 end = new Vector3(path[i + 1].gridLocation.x, path[i + 1].gridLocation.y);
                            Debug.DrawLine(start, end, Color.red, 2f);
                        }

                        LeanTween.cancel(_circle.gameObject);
                        LeanTween.scale(_circle.gameObject, new Vector3(size - 0.5f, size - 0.5f, size - 0.5f), 0.25f);

                        AudioManager.instance.Play("Move");
                    }
                }
            }
        }
    }
    private void FadeInOut(Circle _circle)
    {
        if (_circle.isGhost)
        {
            SpriteRenderer r = _circle.gameObject.GetComponent<SpriteRenderer>();

            LeanTween.value(_circle.gameObject, 0, 1, 1).setOnUpdate((float val) =>
            {
                Color c = r.color;
                c.a = val;
                r.color = c;
            }).setLoopPingPong();
        }
    }

    private void LateUpdate()
    {
        MoveCircle();
    }

    private void MoveCircle()
    {
        if (path.Count > 0)
        {
            MoveAlongPath();

            if (path.Count == 0)
            {
                LeanTween.cancel(_circle.gameObject);
                LeanTween.scale(_circle.gameObject, new Vector3(size, size, size), 0.25f);

                FadeInOut(_circle);

                GridManager.Instance.GetTileAtPosition(_circle.location).circle = null;

                _circle.location = _endTile.gridLocation;
                Tile currentTile = GridManager.Instance.GetTileAtPosition(_endTile.gridLocation);
                currentTile.circle = _circle;

                List<Tile> listScore = GridManager.Instance.CheckScore(currentTile);
                if (listScore.Count >= 5)
                {
                    this.PostEvent(EventID.OnExplosion, listScore);
                }
                _circle = null;
                onProgress = false;

                this.PostEvent(EventID.OnEndTurn);
                this.PostEvent(EventID.OnStartTurn);
            }
        }
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        _circle.transform.position = Vector2.MoveTowards(_circle.transform.position, path[0].transform.position, step);

        if (Vector2.Distance(_circle.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(path[0]);
            path.RemoveAt(0);
        }
    }

    private void PositionCharacterOnLine(Tile tile)
    {
        _circle.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, -5);
        _circle.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder + 1;
    }

}
