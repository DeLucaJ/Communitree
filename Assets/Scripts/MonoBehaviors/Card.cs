using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Card : MonoBehaviour
{
    public List<Action> actions;
    public int lower;
    public int upper;

    public int[] imageLocs;

    //scripts in children for modification
    private Image[] images;
    private Text[] texts;

    private int value1;
    private int value2;
    private ActionType actionType1;
    private ActionType actionType2;

    // Start is called before the first frame update
    void Start()
    {
        //Randomly Select 2 numbers between 0 & 4 (associated with index of acitons)
        int index = Random.Range(0, actions.Count);
        Action action1 = actions[index];
        actions.RemoveAt(index);
        Action action2 = actions[Random.Range(0, actions.Count)];

        //assign types
        actionType1 = action1.type;
        actionType2 = action2.type;

        //Use lower and upper to determine value1 and value2
        value1 = Random.Range(lower, upper);
        value2 = (upper + lower) - value1;

        //Find and assign Images and Text in children
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();

        images[imageLocs[0]].sprite = action1.sprite; //skipping 0 because the card image would be grabbed first
        images[imageLocs[1]].sprite = action2.sprite;
        images[imageLocs[0]].color = action1.color;
        images[imageLocs[1]].color = action2.color;

        texts[0].text = value1.ToString();
        texts[1].text = value2.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
