using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PlaneController, IPlayerObserver
{
    private bool isChase;
    private float lerpSpeed;

    [SerializeField]
    private List<Vector3> waypoints;

    public new Collider collider;

    public Vector3 targetDir { get; private set; }
    public PlaneController target { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        UnitManager.Instance.Add(this);
        // UnitManager.Instance.SetTarget(this);

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
        if (!IsCrush(collision))
        {
            if (!collision.gameObject.GetComponent<ProjectileMoveScript>().isEnemyProjectile)
                Destroy(gameObject);
        }
    }

    private RaycastHit GetObstacle(Vector3 dir)
    {
        Ray ray = new Ray(transform.position, dir);
        Physics.Raycast(ray, out RaycastHit hit);
        return hit;
    }

    private bool IsHit(Vector3 dir)
    {
        RaycastHit hit = GetObstacle(dir);
        return hit.collider != null && hit.distance < speed * 15;
    }

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

    public void SetTarget(PlaneController target)
    {
        this.target = target;
    }
}