using UnityEngine;

public class Gimbal : Manager<Gimbal>, IPlayerObserver
{
    private PlaneController target;

    [SerializeField]
    private float speed = 6;

    private void Awake()
    {
        UnitManager.Instance.Add(this);
    }

    public void SetTarget(PlaneController target)
    {
        this.target = target;
    }

    private void LateUpdate()
    {
        float delta = Time.deltaTime;

        if (target)
        {
            transform.position = target.transform.position;
            transform.rotation = Quaternion.Lerp(transform.rotation, target.transform.rotation, speed * delta);
        }
    }
}