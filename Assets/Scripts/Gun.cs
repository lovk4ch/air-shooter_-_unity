using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField]
    private float reloadTime = .1f;
    private float currentTime;
    private bool isEnemy;

    public GameObject cannon;
    public GameObject projectile;

    private void Awake()
    {
        isEnemy = GetComponent<PlaneController>() is EnemyController;
        currentTime = reloadTime;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
    }

    private void Fire(GameObject projectile, GameObject barrel)
    {
        GameObject prj = Instantiate(projectile, barrel.transform.position, barrel.transform.rotation);
        if (prj.GetComponent<ProjectileMoveScript>() is ProjectileMoveScript pms)
        {
            pms.isEnemyProjectile = isEnemy;
            pms.SetMuzzle(barrel.transform);
        }
    }

    public void Shot()
    {
        if (!projectile)
            return;

        if (currentTime >= reloadTime)
        {
            Fire(projectile, cannon);
            currentTime = 0;
        }
    }
}