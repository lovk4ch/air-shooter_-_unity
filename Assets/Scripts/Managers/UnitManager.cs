using System.Collections.Generic;

public class UnitManager : Manager<UnitManager>
{
    public List<IPlayerObserver> Observers { get; private set; }
    private PlaneController player;

    private void Awake()
    {
        Observers = new List<IPlayerObserver>();
    }

    public void Add(IPlayerObserver observer)
    {
        Observers.Add(observer);
        if (player) {
            observer.SetTarget(player);
        }
    }

    public void Remove(IPlayerObserver observer)
    {
        Observers.Remove(observer);
    }

    public void Notify()
    {
        for (int i = 0; i < Observers.Count; i++)
        {
            Observers[i].SetTarget(player);
        }
    }

    public void SetTarget(PlaneController player)
    {
        this.player = player;
        Notify();
    }

    public void Clear()
    {
        for (int i = 0; i < Observers.Count; i++)
        {
            if (Observers[i] is PlaneController enemy)
            {
                Destroy(enemy.gameObject);
                Remove(Observers[i--]);
            }
        }
    }
}