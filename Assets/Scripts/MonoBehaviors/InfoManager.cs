using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoManager : MonoBehaviour
{
    public GameObject slotPrefab;
    public List<Action> actions;
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
        slotText = new Text[actions.Count];
        slotImages = new Image[actions.Count];

        GameObject slot;
        for (int i = 0; i < actions.Count; i++){
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

            //assign
            slotImages[i].sprite = actions[i].sprite;
            slotImages[i].color = actions[i].color;
        }

        //Debug.Log(gm.attributeScores.Count + " " + actions.Count + " " + slotText.Length + " " + slotImages.Length);
    }

    private void UpdateSlots()
    {
        if (gm.attributeScores.Count > 0){
            for (int i = 0; i < slotText.Length; i++) {
                slotText[i].text = gm.attributeScores[actions[i].name].ToString();
            }
        }
    }
}
