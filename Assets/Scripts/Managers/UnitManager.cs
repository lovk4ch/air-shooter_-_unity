using System.Collections.Generic;

public class UnitManager : Manager<UnitManager>
{
    public List<IPlayerObserver> Observers { get; private set; }
    public PlaneController Player { get; private set; }

    private void Awake()
    {
        Observers = new List<IPlayerObserver>();
    }

    public void Add(IPlayerObserver observer)
    {
        Observers.Add(observer);
        if (Player) {
            observer.SetTarget(Player);
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
            Observers[i].SetTarget(Player);
        }
    }

    public void SetTarget(PlaneController player)
    {
        this.Player = player;
        Notify();
    }
}