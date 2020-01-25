using UnityEngine;

public class LevelManager : Manager<LevelManager>
{
    private KeyAction spawn;

    /// <summary>
    /// Current enemies in scene
    /// </summary>
    private float enemiesCount;
    public float EnemiesCount {
        get { return enemiesCount; }
        set {
            if (value < enemiesCount)
                CreateEnemy();

            enemiesCount = value;
        }
    }

    /// <summary>
    /// Player's defeated enemies
    /// </summary>
    public float DefeatedCount { get; set; }

    public GameObject planePrefab;
    public GameObject playerPrefab;

    [SerializeField]
    private Interface ui;
    public Transform GameLayer => ui.GameLayer.transform;

    private void Awake()
    {
        for (int i = 0; i < 15; i++)
        {
            CreateEnemy();
        }
        SetInput();
        CreatePlayer();
    }

    private void SetInput()
    {
        spawn = new KeyAction(KeyInputMode.KeyDown, InputManager.KeySpawn, CreatePlayer);
        InputManager.Instance.AddKeyAction(spawn);
    }

    private void Update()
    {
        ui.Counter = DefeatedCount.ToString();
    }

    private void OnDestroy()
    {
        if (!InputManager.Instance)
            return;

        InputManager.Instance.RemoveKeyActions(this);
    }

    private void CreateEnemy()
    {
        Quaternion rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
        Instantiate(planePrefab, GetWaypoint(), rotation);
        EnemiesCount++;
    }

    public void CreatePlayer()
    {
        if (!UnitManager.Instance.Player)
        {
            Quaternion rotation = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up);
            Instantiate(playerPrefab, GetWaypoint(), rotation);
            EnableGameLayer();
        }
    }

    public void EnableGameLayer()
    {
        ui.GameLayer.SetActive(true);
    }

    public void DisableGameLayer()
    {
        ui.GameLayer.SetActive(false);
    }

    public static RaycastHit GetRay(Vector3 pos, Vector3 dir)
    {
        Ray ray = new Ray(pos, dir);
        Physics.Raycast(ray, out RaycastHit hit);
        return hit;
    }

    public static Vector3 GetWaypoint()
    {
        Vector3 point = new Vector3(Random.Range(0, 1000), Random.Range(50, 500), Random.Range(0, 1000));
        if (GetRay(point, Vector3.down).collider.name == "Terrain")
            return point;
        else
            return GetWaypoint();
    }
}