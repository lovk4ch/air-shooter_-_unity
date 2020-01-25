using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PlaneController, IPlayerObserver
{
    /// <summary>
    /// Angular acceleration
    /// </summary>
    private float lerpSpeed;

    /// <summary>
    /// Direction to nearest point or target
    /// </summary>
    private Vector3 pointDir;

    /// <summary>
    /// Waypoints list for enemy route
    /// </summary>
    [SerializeField]
    private List<Vector3> waypoints;

    /// <summary>
    /// Target detection radius
    /// </summary>
    [SerializeField]
    [Range(100, 1000)]
    private float attackRadius = 300;

    /// <summary>
    /// Plane collider (for frustrum calculation)
    /// </summary>
    public new Collider collider;

    /// <summary>
    /// Target for pursuit
    /// </summary>
    public PlaneController target { get; private set; }

    /// <summary>
    /// Check if plane is in pursuit mode
    /// </summary>
    private bool isAttack => target && targetDir.magnitude < attackRadius;

    /// <summary>
    /// Distance to target
    /// </summary>
    public Vector3 targetDir => target.transform.position - transform.position;

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

        if (!isAttack)
        {
            if (waypoints.Count > 0)
            {
                if ((transform.position - waypoints[0]).magnitude < speed * lerpSpeed * 50 || IsHit(pointDir))
                {
                    waypoints.Add(GetWaypoint(waypoints[0]));
                    waypoints.RemoveAt(0);
                    lerpSpeed = 0;
                }
                pointDir = waypoints[0] - transform.position;
            }
        }
        else
        {
            pointDir = targetDir;
            if (Quaternion.Angle(transform.rotation, Quaternion.LookRotation(pointDir)) < 5)
            {
                Gun.Shot();
            }
        }

        if (lerpSpeed < maxLerpSpeed)
            lerpSpeed += maxLerpSpeed * delta;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(pointDir), lerpSpeed);
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