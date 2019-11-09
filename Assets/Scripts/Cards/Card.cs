using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardType cardType;

    void PlayCard() {
        cardType.Trigger(new object[] {});
    }

    // Called when the mouse clicks on the Card
    void OnMouseDown() {

    }

    // Called when the mouse releases a click on the Card
    void OnMouseUp() {

    }

    // Called every frame the mouse is over the Card
    void OnMouseOver() {

    }

    // Called when the mouse first goes over the Card image
    void OnMouseEnter() {

    }

    // Called when the mouse leaves the Card Image
    void OnMouseExit() {

    }

    // Called every frame while the mouse is holding a click on the Card
    void OnMouseDrag() {

    }
}
