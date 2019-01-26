using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ActionType { branch, trunk, fruit, leaf };

[CreateAssetMenu(fileName = "New Card Action", menuName = "Card Action")]
public class Action : ScriptableObject
{
    public Sprite sprite;
    public ActionType type;
    public Color color;
}
