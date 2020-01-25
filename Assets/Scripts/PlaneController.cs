using UnityEngine;

[RequireComponent(typeof(Gun))]
public abstract class PlaneController : MonoBehaviour
{
    [Range(10, 60)]
    public float minSpeed = 30;
    [Range(60, 150)]
    public float maxSpeed = 60;
    public float speed;

    [Range(0, 100)]
    public float force = 15;

    [Range(0, 1)]
    public float maxLerpSpeed = 0.05f;

    public Gun Gun => GetComponent<Gun>();

    protected virtual void Awake()
    {
        speed = maxSpeed;
    }

    protected virtual void Update()
    {
        if (speed > minSpeed)
            speed -= force * Time.deltaTime / 6;
        else
            speed = minSpeed;
    }

    protected virtual bool IsCrush(Collision collision)
    {
        if (collision.collider.CompareTag("Projectile"))
            return false;

        Destroy(gameObject);
        return true;
    }
}