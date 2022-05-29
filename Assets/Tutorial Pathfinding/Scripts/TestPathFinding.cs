using System.Collections.Generic;
using UnityEngine;

public class TestPathFinding : MonoBehaviour
{
    private PathFindingWithDebug pathFindingWithDebug;

    private PathFinding pathFinding;

    private Node start, end;
    
    public bool hasDiagonal;

    private bool hasCreateBlock;

    public bool debug;

    private void Start()
    {
        pathFindingWithDebug = new PathFindingWithDebug();
        pathFinding = new PathFinding();
    }

    void Update()
    {
        RaycastHit2D hit = GetFocusedOnTile();

        if (hit.collider != null)
        {
            //TestFindPath(hit);

            TestFindPathWithDebug(hit);

            AddAndRemoveBlock(hit);
        }
    }

    private void TestFindPathWithDebug(RaycastHit2D hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Node node = hit.collider.GetComponent<Node>();
            if (start == null)
            {
                start = node;
                start.posDebug.text = "Start";

                ResetListNode();
            }
            else
            {
                end = hit.collider.GetComponent<Node>();

                StartCoroutine(pathFindingWithDebug.FindPath(start, end, hasDiagonal, debug, (x) =>
                {
                    if (x != null)
                    {
                        Debug.Log("Finish Find Path");
                        for (int i = 0; i < x.Count - 1; i++)
                        {
                            Debug.DrawLine(((Vector3Int)x[i].location), ((Vector3Int)x[i + 1].location), Color.red, 2f);
                        }
                    }
                    else Debug.Log("Can't Find Path");

                }));

                start.posDebug.text = $"{start.location.x}, {start.location.y}";
                start = null;
            }
        }
    }

    private void TestFindPath(RaycastHit2D hit)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Node node = hit.collider.GetComponent<Node>();
            if (start == null)
            {
                start = node;
                start.posDebug.text = "Start";
            }
            else
            {
                end = hit.collider.GetComponent<Node>();

                List<Node> path = pathFinding.FindPath(start, end, hasDiagonal);

                if (path != null)
                {
                    Debug.Log("Finish Find Path");
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Debug.DrawLine(((Vector3Int)path[i].location), ((Vector3Int)path[i + 1].location), Color.red, 2f);
                    }
                }
                else Debug.Log("Can't Find Path");

                start.posDebug.text = $"{start.location.x}, {start.location.y}";
                start = null;
            }
        }
    }

    private void ResetListNode()
    {
        foreach (var item in GameManager.listNode.Values)
        {
            if (item.GetComponent<SpriteRenderer>().color != Color.black)
                item.GetComponent<SpriteRenderer>().color = Color.white;

            item.previousNode = null;
            item.GDebug.transform.parent.gameObject.SetActive(false);
            item.posDebug.gameObject.SetActive(true);
        }
    }

    private void AddAndRemoveBlock(RaycastHit2D hit)
    {
        if (Input.GetMouseButtonDown(1))
        {
            Node node = hit.collider.GetComponent<Node>();
            if (node.isWalkable)
            {
                hasCreateBlock = true;
            }
            else hasCreateBlock = false;
        }

        if (Input.GetMouseButton(1))
        {
            Node node = hit.collider.GetComponent<Node>();
            if (hasCreateBlock)
            {
                node.isWalkable = false;
                node.GetComponent<SpriteRenderer>().color = Color.black;
            }
            if (!hasCreateBlock)
            {
                node.isWalkable = true;
                node.GetComponent<SpriteRenderer>().color = Color.white;
            }
        }
    }

    private static RaycastHit2D GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

        return hit;
    }
}
