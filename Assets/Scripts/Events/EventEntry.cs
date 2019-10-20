using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEntry
{
    public Delegate del;
    public object[] paramVals;

    public EventEntry(Delegate del, object[] paramVals = null)
    {
        if (del != null && del.Method != null)
        {
            this.del = del;
            if (paramVals != null) this.paramVals = paramVals;
            else paramVals = new object[0];
            //this.paramVals = new object[this.del.Method.GetParameters().Length];
        }
    }

    public void Invoke()
    {
        if(del != null /*&& paramVals != null*/)
        {
            this.del.Method.Invoke(del.Target, paramVals);
        }
    }
}
