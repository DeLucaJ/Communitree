using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventEntry
{
    public bool reviever; // Takes its parameters from the EventHolder
    public Delegate del;
    public object[] paramVals; // if no caller params are sent, these would be the default params

    public EventEntry(Delegate del, object[] paramVals = null, bool reciever = false)
    {
        if (del != null && del.Method != null)
        {
            this.del = del;
            if (paramVals != null) this.paramVals = paramVals;
            else paramVals = new object[0];
            //this.paramVals = new object[this.del.Method.GetParameters().Length];
        }
        this.reviever = reciever;
    }

    public void Invoke(object[] callerParams)
    {
        if (del != null) {
            if (this.reviever && callerParams != null) {
                this.del.Method.Invoke(del.Target, callerParams);
            }
            else {
                this.del.Method.Invoke(del.Target, this.paramVals);
            }
        }
    }
}
