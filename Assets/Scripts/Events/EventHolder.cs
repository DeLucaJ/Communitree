using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHolder
{
    public List<EventEntry> entries;

    public EventHolder()
    {
        entries = new List<EventEntry>();
    }

    public void Invoke()
    {
        foreach (EventEntry entry in entries)
        {
            entry.Invoke();
        }
    }
}
