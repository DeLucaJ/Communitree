using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class EventHandler : MonoBehaviour
{
    public static EventHandler current;
    private Dictionary<string, EventHolder> eventDict;

    void Awake()
    {
        current = this;
        eventDict = new Dictionary<string, EventHolder>();
    }

    public static void InitializeEvent(string name)
    {
        if(!current.eventDict.ContainsKey(name)) current.eventDict.Add(name, new EventHolder());
    }

    public static void TriggerEvent(string name)
    {
        if(current.eventDict.ContainsKey(name)) current.eventDict[name].Invoke();
    }

    public static void AddListener(string name, EventEntry entry)
    {
        if(!current.eventDict.ContainsKey(name))
        {
            InitializeEvent(name);
        }
        current.eventDict[name].entries.Add(entry);
    }

    public static void RemoveListener(string name, EventEntry entry)
    {
        current.eventDict[name].entries.Remove(entry);
    }
}
