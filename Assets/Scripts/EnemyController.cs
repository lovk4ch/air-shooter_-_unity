using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PlaneController, IPlayerObserver
{
    /// <summary>
    /// Pursuit mode for enemy
    /// </summary>
    private bool isChase;

    /// <summary>
    /// Angular acceleration
    /// </summary>
    private float lerpSpeed;

    /// <summary>
    /// Waypoints list for enemy route
    /// </summary>
    [SerializeField]
    private List<Vector3> waypoints;

    /// <summary>
    /// Plane collider (for frustrum calculation)
    /// </summary>
    public new Collider collider;

    /// <summary>
    /// Direction to nearest point or target
    /// </summary>
    public Vector3 targetDir { get; private set; }

    /// <summary>
    /// Target for pursuit
    /// </summary>
    public PlaneController target { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        UnitManager.Instance.Add(this);
        lerpSpeed = maxLerpSpeed;

        waypoints = new List<Vector3> {
            GetWaypoint(transform.position)
        };
        waypoints.Add(GetWaypoint(waypoints[0]));
    }

    private void OnDestroy()
    {
        if (!InputManager.Instance || !UnitManager.Instance)
            return;

        UnitManager.Instance.Remove(this);
        LevelManager.Instance.EnemiesCount--;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsCrash(collision))
        {
            if (!collision.gameObject.GetComponent<ProjectileMoveScript>().isEnemyProjectile)
            {
                LevelManager.Instance.DefeatedCount++;
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Check that the path to the point is not blocked by anything
    /// </summary>
    /// <param name="dir">Direction to target point</param>
    /// <returns></returns>
    private bool IsHit(Vector3 dir)
    {
        RaycastHit hit = LevelManager.GetRay(transform.position, dir);
        return hit.collider != null && hit.distance < speed * 15;
    }

    /// <summary>
    /// Generate new waypoint for plane route
    /// </summary>
    /// <param name="startPoint">Current plane position</param>
    /// <returns></returns>
    private Vector3 GetWaypoint(Vector3 startPoint)
    {
        Vector3 point = LevelManager.GetWaypoint();
        Vector3 dir = point - startPoint;

        if (!IsHit(dir))
        {
            return point;
        }
        else
        {
            return GetWaypoint(startPoint);
        }
    }

    protected override void Update()
    {
        base.Update();
        float delta = Time.deltaTime;

        if (waypoints.Count > 0 && !isChase)
        {
            if ((transform.position - waypoints[0]).magnitude < speed * lerpSpeed * 50 || IsHit(targetDir))
            {
                waypoints.Add(GetWaypoint(waypoints[0]));
                waypoints.RemoveAt(0);
                lerpSpeed = 0;
            }
            else if (target && (transform.position - target.transform.position).magnitude < speed * 15)
            {
                isChase = true;
                lerpSpeed = 0;
            }
        }

        if (lerpSpeed < maxLerpSpeed)
            lerpSpeed += maxLerpSpeed * delta;

        if (isChase && target) {
            targetDir = target.transform.position - transform.position;
            if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(targetDir)) < 5)
            {
                Gun.Shot();
            }
        }
        else {
            targetDir = waypoints[0] - transform.position;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(targetDir), lerpSpeed);
        transform.Translate(transform.forward * speed * delta, Space.World);
    }

    /// <summary>
    /// Sets the plane for pursuit mode
    /// </summary>
    /// <param name="target">Target plane</param>
    public void SetTarget(PlaneController target)
    {
        this.target = target;
    }
}