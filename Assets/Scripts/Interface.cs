using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField]
    private Text counter = null;

    public string Counter {
        get => counter.text;
        set => counter.text = value;
    }
}