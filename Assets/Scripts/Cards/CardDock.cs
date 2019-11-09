using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDock : MonoBehaviour
{
    public Hand hand;
    public Stack<CardType> played;

    private EventEntry cardGrabber;

    // Start is called before the first frame update
    void Start()
    {
        cardGrabber = new EventEntry(new Action<CardType>(GrabCard), reciever: true);
        EventHandler.AddListener("cardPlayed", cardGrabber);
    }

    void GrabCard(CardType card)
    {
        // adds the card played to the list of played cards
        played.Push(card);
    }

    void OnDestroy()
    {
        EventHandler.RemoveListener("cardPlayed", cardGrabber);
    }
}
