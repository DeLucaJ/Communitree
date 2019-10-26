using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHolder
{
    public List<EventEntry> entries;

    public EventHolder(List<EventEntry> entries = null)
    {
        if (entries != null) {
            this.entries = entries;
        }
        else {
            this.entries = new List<EventEntry>();
        }
    }

    public void Invoke(object[] callerParams = null)
    {
        foreach (EventEntry entry in entries)
        {
            entry.Invoke(callerParams);
        }
    }
}
