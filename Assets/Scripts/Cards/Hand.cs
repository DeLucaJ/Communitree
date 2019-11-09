﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    public LinkedList<HandSlot> slots;
    public CardDock dock;

    public void AddCard(Card card) {
        HandSlot slot = new HandSlot(card, this);
        slots.AddLast(slot);
        slot.SetNode(slots.Last);
    }
}
