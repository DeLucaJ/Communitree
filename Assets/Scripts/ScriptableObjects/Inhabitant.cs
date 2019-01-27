using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Inhabitant", menuName = "Inhabitant")]
public class Inhabitant : ScriptableObject
{
    public Sprite sprite;
    public Color color;
    public Action preference;
    public float perA;
}
