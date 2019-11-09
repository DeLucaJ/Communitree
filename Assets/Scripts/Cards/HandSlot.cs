using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSlot
{
    public Card card;

    private Hand hand;
    private LinkedListNode<HandSlot> node = null;
    private EventEntry play;

    public HandSlot(Card card, Hand hand) {
        this.hand = hand;
        this.card = card;

        play = new EventEntry(new Action(CardPlayed));
        EventHandler.AddListener(card.cardType.eventName, play);
    }

    public void SetNode(LinkedListNode<HandSlot> node) {
        this.node = node;
    }

    void CardPlayed()
    {
        if (card.isActive) { hand.slots.Remove(node); }
        EventHandler.RemoveListener(card.cardType.eventName, play);
    }
}
