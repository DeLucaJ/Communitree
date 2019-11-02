using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCardType", menuName = "CardType", order = 1)]
public class CardType : ScriptableObject
{
    public string eventName;
    public Sprite art;

    public void Trigger(object[] values = null) {
        EventHandler.TriggerEvent(eventName, values);
    }
}
