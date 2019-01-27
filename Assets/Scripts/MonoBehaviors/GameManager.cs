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
    public GameObject inhabitantPrefab;
    public Vector3 instancePoint;
    public Vector3 spawnPoint;
    public float spawnDelay;
    
    [HideInInspector]
    public Dictionary<string, int> attributeScores = new Dictionary<string, int>();

    [HideInInspector]
    public Dictionary<string, int> inhabitantScores = new Dictionary<string, int>();

    private Queue<GameObject> inhabitantsQueue = new Queue<GameObject>();
    private float elapsedTime = 0.0f;

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
        UpdateQueue();
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
            float attrib = grabPreferredAttribute(inhab.name);

            if (attrib < 1) return;

            float height = tg.getHeight();
            float inhabs = inhabitantScores[inhab.name];

            float currentRatio = (inhabs /attrib);
            
            if (currentRatio < inhab.perA) {
                int newInhabitants = (int) ((inhab.perA - currentRatio) * (/*height*/ + attrib));

                if (newInhabitants < 1) break;

                SpawnInhabitants(inhab, newInhabitants);
            }
        }
    }

    private void UpdateQueue()
    {
        if (inhabitantsQueue.Count < 1) return;

        elapsedTime += Time.deltaTime;

        if (elapsedTime >= spawnDelay) {
            GameObject current = inhabitantsQueue.Dequeue();

            current.transform.position = spawnPoint;

            //animate
        
            elapsedTime = 0.0f;
        }
    }

    private int grabPreferredAttribute(String name) {
        int ret;
        switch(name){
                case "Branchies":
                    ret = tg.branchCount();
                    break;
                case "Leafies":
                    ret = tg.leafCount();
                    break;
                case "Fruities":
                    ret = tg.fruitCount();
                    break;
                default:
                    ret = 0;
                    break;
        }
        return ret;
    }

    private void SpawnInhabitants(Inhabitant type, int howMany) {
        
        for (int i = 0; i < howMany; i++) {
            GameObject current = Instantiate(inhabitantPrefab, instancePoint, Quaternion.identity);
            InhabitantController ic = current.GetComponent<InhabitantController>();
            ic.type = type;

            inhabitantsQueue.Enqueue(current);
            Debug.Log(inhabitantsQueue.Count);
            inhabitantScores[type.name]++;
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
