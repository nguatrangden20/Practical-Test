using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Pathfinding pathFinding;
    public int startX;
    public int startY;
    private List<Tile> path;
    void Start()
    {
        pathFinding = new Pathfinding();
        path = new List<Tile>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            Tile tile = hit.Value.collider.gameObject.GetComponent<Tile>();

            if (Input.GetMouseButtonDown(0))
            {
                path = pathFinding.FindPath(GridManager.Instance.GetTileAtPosition(new Vector2Int(startX, startY)) , tile);

                for (int i = 0; i < path.Count - 1; i++)
                {
                    Vector3 start = new Vector3(path[i].gridLocation.x, path[i].gridLocation.y);
                    Vector3 end = new Vector3(path[i + 1].gridLocation.x, path[i + 1].gridLocation.y);
                    Debug.DrawLine(start, end, Color.red, 2f);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            this.PostEvent(EventID.OnStartTurn);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            this.PostEvent(EventID.OnEndTurn);
        }
    }

    private static RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }
}
