using UnityEngine;

public class PlayerController : PlaneController
{
    private KeyAction boost, shot;

    [Range(0, 1.5f)]
    public float rotationSpeed = 0.3f;

    protected override void Awake()
    {
        base.Awake();

        SetInput();
        UnitManager.Instance.SetTarget(this);
    }

    private void SetInput()
    {
        boost = new KeyAction(KeyInputMode.KeyPressed, InputManager.KeyAcceleration, SetBoost);
        InputManager.Instance.AddKeyAction(boost);

        shot = new KeyAction(KeyInputMode.KeyPressed, InputManager.KeyFire, Gun.Shot);
        InputManager.Instance.AddKeyAction(shot);
    }

    private void SetBoost()
    {
        if (speed < maxSpeed)
            speed += force * Time.deltaTime;
    }

    protected override void Update()
    {
        Move(InputManager.Instance.GetAxis(InputManager.VerticalAxis),
            InputManager.Instance.GetAxis(InputManager.HorizontalAxis));

        base.Update();
    }

    private void OnDestroy()
    {
        if (!InputManager.Instance || !UnitManager.Instance)
            return;

        UnitManager.Instance.SetTarget(null);
        InputManager.Instance.RemoveKeyActions(this);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!IsCrush(collision))
        {
            if (collision.gameObject.GetComponent<ProjectileMoveScript>().isEnemyProjectile)
                Destroy(gameObject);
        }
    }

    private void Move(float pitch, float roll)
    {
        float delta = Time.deltaTime;

        Quaternion rotation = transform.rotation * Quaternion.AngleAxis(pitch * speed * rotationSpeed, Vector3.left) * Quaternion.AngleAxis(roll * speed * rotationSpeed, Vector3.back);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, maxLerpSpeed);
        transform.Translate(transform.forward * speed * delta, Space.World);
    }
}