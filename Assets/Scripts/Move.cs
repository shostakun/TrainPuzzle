using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 1;
    private Board board;
    protected string direction;
    protected TrackPath path;
    protected Transform node;
    protected Vector3 goal;
    protected Tile currentTile;

    void Start()
    {
        board = FindFirstObjectByType<Board>();
        SetGoal(transform);
    }

    protected Quaternion GetGoalRotation()
    {
        Vector3 direction = goal - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void SetGoal(Transform transform_)
    {
        if (transform_ == null) return;
        node = transform_;
        goal = node.position;
        goal.z = transform.position.z;
        Debug.Log($"SetGoal: {goal} {direction}");
    }

    public void SetCurrentTile(Tile tile)
    {
        if (tile == null) return;
        string dir = Board.GetOpposite(direction);
        TrackPath p = tile.GetPathFrom(dir);
        if (p == null) return;
        if (currentTile != null) currentTile.isInUse = false;
        currentTile = tile;
        tile.isInUse = true;
        path = p;
        direction = path.end;
        SetGoal(path.GetNext());
    }

    public void SetStartTile(Tile tile)
    {
        currentTile = tile;
        tile.isInUse = true;
        path = tile.GetPathFrom("C");
        direction = path.end;
        SetGoal(path.GetNext());
        transform.rotation = GetGoalRotation();
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, goal, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, goal) < 0.01f)
        {
            Transform next = path.GetNext(node);
            Debug.Log($"Update: {next}");
            if (next != null)
            {
                Debug.Log($"Update with Transform: {next.position}");
                node = next;
                SetGoal(next);
            }
            else
            {
                Debug.Log($"Update with end direction: {direction}");
                SetCurrentTile(board.GetNeighbor(currentTile, direction));
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, GetGoalRotation(), 180 * Time.deltaTime);
        }
    }
}
