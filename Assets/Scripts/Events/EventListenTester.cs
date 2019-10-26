using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTester : MonoBehaviour
{
    private int limit = 10;

    private EventEntry te1;
    private EventEntry te2;
    private EventEntry te3;
    private EventEntry rc1;

    // Start is called before the first frame update
    void Start()
    {
        te1 = new EventEntry(
            new Action<int>(TimeEvent1),
            new object[1] { 1 }
        );
        te2 = new EventEntry(
            new Action<string>(TimeEvent2),
            new object[1] { " second has passed" }
        );
        te3 = new EventEntry(
            new Action(TimeEvent3)
        );
        rc1 = new EventEntry(
            new Action<int, int>(RecieverEvent1),
            reciever: true
        );
        EventHandler.AddListener("timeEvent", te1);
        EventHandler.AddListener("timeEvent", te2);
        EventHandler.AddListener("timeEvent", te3);
        EventHandler.AddListener("recieverEvent", rc1);
    }

    void OnDestroy()
    {
        EventHandler.RemoveListener("timeEvent", te1);
        EventHandler.RemoveListener("timeEvent", te2);
        EventHandler.RemoveListener("timeEvent", te3);
        EventHandler.RemoveListener("recieverEvent", rc1);
    }

    // Update is called once per frame
    void Update()
    {
        if (limit == 0) Destroy(gameObject);
    }
    void TimeEvent1(int i)
    {
        Debug.Log("TE1 " + i);
    }

    void TimeEvent2(string s)
    {
        Debug.Log("TE2 " + s);
    }

    void TimeEvent3()
    {
        limit--;
        Debug.Log("TE3 " + limit);
    }

    void RecieverEvent1(int t, int x) {
        Debug.Log("Recieved " + t + " & " + x);
    }
}
