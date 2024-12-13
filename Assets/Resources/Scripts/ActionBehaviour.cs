using UnityEngine;
using System;
using System.Collections.Generic;

public enum ActionType{
    GoToLocation,
    FindCharacter,
    KillCharacter,
    ReportConspiracy
}

public class Action{
    public ActionType type;
    public List<float> parameters = new List<float>();
}

public class ActionBehaviour : MonoBehaviour
{
    // Create ActionStack
    public List<Action> actionStack = new List<Action>();

    // Get time manager for controlling the length of actions taken
    public TimeManagerBehaviour timeManagerBehaviour;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeManagerBehaviour = GameObject.Find("TimeManager").GetComponent<TimeManagerBehaviour>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
