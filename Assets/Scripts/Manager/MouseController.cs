using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    private void Start()
    {
        pathFinding = new Pathfinding();
        path = new List<Tile>();
    }

    void Update()
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
                }
                else if(!onProgress)
                {
                    onProgress = true;
                    _endTile = currentTile;
                    path = pathFinding.FindPath(GridManager.Instance.GetTileAtPosition(_circle.location) , currentTile);

                    if (path.Count == 0)
                    {
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

                    }
                }

            }

        }
    }

    private void LateUpdate()
    {
        if (path.Count > 0)
        {
            MoveAlongPath();

            if (path.Count == 0)
            {
                GridManager.Instance.GetTileAtPosition(_circle.location).circle = null;
                _circle.location = _endTile.gridLocation;
                GridManager.Instance.GetTileAtPosition(_endTile.gridLocation).circle = _circle;
                _circle = null;
                onProgress = false;
            }
        }
    }

    private void MoveAlongPath()
    {
        var step = speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;
        _circle.transform.position = Vector2.MoveTowards(_circle.transform.position, path[0].transform.position, step);
        _circle.transform.position = new Vector3(_circle.transform.position.x, _circle.transform.position.y, zIndex);

        if (Vector2.Distance(_circle.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(path[0]);
            path.RemoveAt(0);
        }
    }

    private void PositionCharacterOnLine(Tile tile)
    {
        _circle.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        _circle.GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
    }

}
