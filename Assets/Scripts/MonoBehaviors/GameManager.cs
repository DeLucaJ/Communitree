using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Action[] actions;
    public Inhabitant[] inhabitants;
    public treegrow tg;
    public int startingVal;

    //public float IPA; //inhabitants per Attribute
    //public float IPH; //inhabitants per Height
    

    [HideInInspector]
    public Dictionary<string, int> attributeScores = new Dictionary<string, int>();

    [HideInInspector]
    public Dictionary<string, int> inhabitantScores = new Dictionary<string, int>();

    // Start is called before the first frame update
    void Start()
    {
        FillDictionaries();
        SetTreeVals();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateActions();
        UpdateInhabitants();
    }

    void FillDictionaries() {
        //fill action dictionary
        foreach (Action act in actions) {
            attributeScores.Add(act.name, startingVal);
        }

        //fill inhabitant dictionary
        foreach (Inhabitant inhab in inhabitants) {
            inhabitantScores.Add(inhab.name, 0);
        }
    }

    private void SetTreeVals()
    {
        tg.growth = startingVal;
        tg.max_children = startingVal;
        tg.leaf_factor = startingVal;
        tg.fruit_factor = startingVal;
    }

    private void UpdateActions()
    {
        foreach(Action act in actions) {
            switch(act.name) {
                case "Branch": handleBranch(act);  break;
                case "Fruit": handleFruit(act); break;
                case "Height": handleHeight(act);  break;
                case "Leaf": handleLeaf(act);  break;
                default: break;
            }
        }
    }

    private void UpdateInhabitants()
    {
        foreach(Inhabitant inhab in inhabitants) {
            int current = attributeScores[inhab.preference.name];
            
        }
    }

    private void handleBranch(Action act) {
        tg.max_children = attributeScores[act.name];
    }

    private void handleFruit(Action act) {
        tg.fruit_factor = attributeScores[act.name];
    }

    private void handleHeight(Action act) {
        tg.growth = attributeScores[act.name];
    }

    private void handleLeaf(Action act) {
        tg.leaf_factor = attributeScores[act.name];
    }
}
