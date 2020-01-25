using UnityEngine;

public class LevelManager : Manager<LevelManager>
{
    public float EnemiesCount { get; set; }

    public GameObject planePrefab;
    public Interface ui;

    private void Awake()
    {
        for (int i = 0; i < 15; i++)
        {
            Quaternion rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
            Instantiate(planePrefab, GetWaypoint(), rotation);
            EnemiesCount++;
        }
    }

    private void Update()
    {
        ui.Counter = EnemiesCount.ToString();
    }

    public static Vector3 GetWaypoint()
    {
        return new Vector3(Random.Range(0, 1000), Random.Range(50, 500), Random.Range(0, 1000));
    }
}