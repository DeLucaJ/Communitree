using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action[] actions;
    public int startingVal;
    //public Inhabitant[] inhabitants;

    [NonSerialized]
    public Dictionary<string, int> attributeScores = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        FillDictionary();
    }

    void FillDictionary() {
        //fill dictionary
        for (int i = 0; i < actions.Length; i++){
            attributeScores.Add(actions[i].name, startingVal);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
