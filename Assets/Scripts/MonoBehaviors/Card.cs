using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Card : MonoBehaviour
{
    public List<Action> actions;
    public int lower;
    public int upper;
    public int sum;

    public int[] imageLocs;

    //scripts in children for modification
    private Image[] images;
    private Text[] texts;

    private int value1;
    private int value2;
    private Action action1;
    private Action action2;
    public Button button;
    private Tween[] tweens = new Tween[3];
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();

        //Randomly Select 2 numbers between 0 & 4 (associated with index of acitons)
        int index = Random.Range(0, actions.Count);
        action1 = actions[index];
        actions.RemoveAt(index);
        action2 = actions[Random.Range(0, actions.Count)];

        //Use lower and upper to determine value1 and value2
        value1 = Random.Range(lower, upper);

        if (value1 == 0) value1++;
        else if (value1 == sum) value1--;
        
        value2 = sum - value1;

        //Find and assign Images and Text in children
        images = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<Text>();

        images[imageLocs[0]].sprite = action1.sprite; //skipping 0 because the card image would be grabbed first
        images[imageLocs[1]].sprite = action2.sprite;
        images[imageLocs[0]].color = action1.color;
        images[imageLocs[1]].color = action2.color;

        texts[0].text = value1.ToString();
        texts[1].text = value2.ToString();

        //Assign Button & Function
        //button = gameObject.GetComponent<button>();
        button.onClick.AddListener(Dispatch);

        //find tweens
        //0 = Create
        //1 = Death
        tweens = GetComponents<Tween>();
    }

    void Dispatch() {
        //send data to game manager
        gm.attributeScores[action1.name] += value1;
        gm.attributeScores[action2.name] += value2;
        Debug.Log(action1.name + "\t: " + gm.attributeScores[action1.name].ToString());
        Debug.Log(action2.name + "\t: " + gm.attributeScores[action2.name].ToString());

        //call ResetCards on parent
        CardDock dock = gameObject.GetComponentInParent<CardDock>();
        dock.ResetCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
