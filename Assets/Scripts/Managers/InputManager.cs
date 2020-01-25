using System.Collections.Generic;
using UnityEngine;

public enum KeyInputMode
{
    KeyPressed,
    KeyUp,
    KeyDown
}

public class KeyAction
{
    public delegate void Callback();
    public KeyInputMode keyMode;
    public KeyCode code;
    public Callback callback;

    public KeyAction(KeyInputMode keyMode, KeyCode code, Callback callback)
    {
        this.keyMode = keyMode;
        this.code = code;
        this.callback = callback;
    }
}

public class InputManager : Manager<InputManager>
{
    public const KeyCode KeyAcceleration = KeyCode.LeftShift;
    public const KeyCode KeyFire = KeyCode.LeftControl;

    public const string HorizontalAxis = "Horizontal";
    public const string VerticalAxis = "Vertical";

    private List<KeyAction> actions;

    private void Awake()
    {
        actions = new List<KeyAction>();
    }

    public float GetAxis(string axisName)
    {
        return Input.GetAxis(axisName);
    }

    public void AddKeyAction(KeyAction item)
    {
        actions.Add(item);
    }

    public void RemoveKeyAction(KeyAction item)
    {
        actions.Remove(item);
    }

    public void RemoveKeyActions(MonoBehaviour obj)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            if (actions[i].callback.Target.GetType() == obj.GetType())
                actions.RemoveAt(i--);
        }
    }

    private void Update()
    {
        for (int i = 0; i < actions.Count; i++)
        {
            switch (actions[i].keyMode)
            {
                case KeyInputMode.KeyPressed:
                    {
                        if (Input.GetKey(actions[i].code))
                        {
                            actions[i].callback.Invoke();
                        }
                        break;
                    }
                case KeyInputMode.KeyUp:
                    {
                        if (Input.GetKeyUp(actions[i].code))
                        {
                            actions[i].callback.Invoke();
                        }
                        break;
                    }
                case KeyInputMode.KeyDown:
                    {
                        if (Input.GetKeyDown(actions[i].code))
                        {
                            actions[i].callback.Invoke();
                        }
                        break;
                    }
            }
        }
    }
}