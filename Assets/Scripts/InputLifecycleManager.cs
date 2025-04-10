using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;


public class InputLifecycleManager
{
    
    public Dictionary<string,float> inputLifecycles = new Dictionary<string, float>();

    public InputLifecycleManager()
    {
        if(inputLifecycles == null)
        {
            inputLifecycles = new Dictionary<string, float>();
        }
        else
        {
            inputLifecycles.Clear();
        }
    }

    public void AddInputLifecycle(string actionName)
    {
        
        inputLifecycles.Add(actionName, 0f);
    }
    public bool GetPressed(string actionName)
    {
        if (Input.GetButtonDown(actionName))
        {
            inputLifecycles[actionName] = 0.2f;
        }
        bool isPressed = inputLifecycles[actionName] > 0f;
        inputLifecycles[actionName] -= Time.deltaTime;
        return isPressed;
    }

}
