using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 1;
    public float turnSpeed = 1;
    protected string direction;
    protected TrackPath path;
    protected Transform node;
    protected Vector3 goal;
    protected Track currentTrack;

    void Start()
    {
        SetGoal(transform);
    }

    protected Quaternion GetGoalRotation()
    {
        Vector3 direction = goal - transform.position;
        float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        return Quaternion.AngleAxis(angle, Vector3.up);
    }

    public void SetGoal(Transform transform_)
    {
        if (transform_ == null) return;
        node = transform_;
        goal = node.position;
        goal.y = transform.position.y;
        Debug.Log($"SetGoal: {goal} {direction}");
    }

    public void SetCurrentTrack(Track track)
    {
        if (track == null) return;
        string dir = Board.GetOpposite(direction);
        TrackPath p = track.GetPathFrom(dir);
        if (p == null) return;
        if (currentTrack != null) currentTrack.isInUse = false;
        currentTrack.onTrainExit?.Invoke();
        currentTrack = track;
        track.isInUse = true;
        path = p;
        direction = path.end;
        SetGoal(path.GetNext());
        track.onTrainEnter?.Invoke();
    }

    public void SetStartTrack(Track track)
    {
        currentTrack = track;
        track.isInUse = true;
        path = track.GetPathFrom("C");
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
            if (next != null)
            {
                node = next;
                SetGoal(next);
            }
            else
            {
                SetCurrentTrack(Board.inst.GetNeighbor(currentTrack, direction));
            }
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, GetGoalRotation(), 180 * turnSpeed * Time.deltaTime);
        }
    }
}
