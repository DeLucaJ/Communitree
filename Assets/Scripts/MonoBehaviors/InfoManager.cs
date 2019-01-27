using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public List<Action> actions;
    public List<Inhabitant> inhabitants;
    public Vector2 startPos;
    public int increment;

    private Text[] slotText;
    private Image[] slotImages;
    private GameManager gm;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
         CreateSlots();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSlots();
    }

    private void CreateSlots()
    {
        slotText = new Text[actions.Count + inhabitants.Count];
        slotImages = new Image[actions.Count + inhabitants.Count];

        GameObject slot;
        for (int i = 0; i < (actions.Count + inhabitants.Count); i++){
            slot = Instantiate(slotPrefab, 
                transform.position + new Vector3(
                    startPos.x,
                    startPos.y + i * increment, 
                    0
                ), 
                Quaternion.identity
            );
            slot.transform.SetParent(gameObject.transform);
            slotText[i] = slot.GetComponentInChildren<Text>();
            slotImages[i] = slot.GetComponentInChildren<Image>();
        }

        int j = 0;
        foreach (Action act in actions) {
            slotImages[j].sprite = act.sprite;
            slotImages[j].color = act.color;
            j++;
        }

        foreach (Inhabitant inhab in inhabitants) {
            slotImages[j].sprite = inhab.sprite;
            slotImages[j].color = inhab.color;
            j++;
        }
    }

    private void UpdateSlots()
    {
        if (gm.attributeScores.Count > 0){
            for (int i = 0; i < gm.attributeScores.Count; i++) {
                slotText[i].text = gm.attributeScores[actions[i].name].ToString();
            }
        }

        if (gm.inhabitantScores.Count > 0){
            for (int i = gm.attributeScores.Count; i < slotText.Length; i++) {
                slotText[i].text = gm.inhabitantScores[inhabitants[i - gm.attributeScores.Count].name].ToString();
            }
        }
    }
}
