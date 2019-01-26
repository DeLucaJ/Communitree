using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card Action", menuName = "Card Action")]
public class Action : ScriptableObject
{
    public enum ActionType { branch, trunk, fruit, leaf };

    public int lowerBound;
    public int upperBound;
    public Sprite image;
    public ActionType type;
    public Color color;
}
