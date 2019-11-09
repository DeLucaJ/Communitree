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
        // if not in dock, play card
    }

    // Called every frame the mouse is over the Card
    void OnMouseOver() {
        // Make the Frame glow
    }

    // Called when the mouse first goes over the Card image
    void OnMouseEnter() {
        // Zoom on Card
    }

    // Called when the mouse leaves the Card Image
    void OnMouseExit() {
        // Return to Normal Size/Position/Shaders
    }

    // Called every frame while the mouse is holding a click on the Card
    void OnMouseDrag() {
        // Card follows the mouse
    }
}
