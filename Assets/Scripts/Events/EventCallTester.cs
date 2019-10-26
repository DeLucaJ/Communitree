using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTester2 : MonoBehaviour
{
    private float time = 0.0f;
    private int rtriggered = 0;
    private EventEntry te;
    // Start is called before the first frame update
    void Start()
    {
        te = new EventEntry(
            new Action(TimeEvent)
        );
        EventHandler.AddListener("timeEvent", te);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (time >= 1.0f) 
        {
            time -= 1.0f;
            EventHandler.TriggerEvent("timeEvent");
            rtriggered++;
            EventHandler.TriggerEvent("recieverEvent", new object[2] { rtriggered, 10 - rtriggered });
        }
    }

    void TimeEvent()
    {
        Debug.Log("We still going");
    }
}
