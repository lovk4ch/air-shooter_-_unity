using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Text counter = null;
    [SerializeField]
    private GameObject gameLayer = null;

    public string Counter {
        get => counter.text;
        set => counter.text = value;
    }

    public GameObject GameLayer
    {
        get => gameLayer;
        set => gameLayer = value;
    }
}