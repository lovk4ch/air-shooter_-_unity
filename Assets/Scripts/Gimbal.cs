using UnityEngine;

public class Gimbal : Manager<Gimbal>, IPlayerObserver
{
    private PlaneController target;

    // [SerializeField]
    // private float speed = 3f;

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

            /*Quaternion rotation = Quaternion.Euler(target.transform.rotation.eulerAngles.x, target.transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, speed * delta);*/
            transform.rotation = target.transform.rotation;
        }
    }
}