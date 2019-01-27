using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardDock : MonoBehaviour
{
    public Vector2Int[] cardPoints;
    public GameObject cardPrefab;
    public bool randomDelay;
    public float lowerDelay;
    public float upperDelay;
    public float deathDelay;
    public int cardLimit;

    private GameObject[] cards = new GameObject[3];
    private bool resetting = false;
    private float elapsedTime = 0.0f;
    private float spawnDelay;
    private int usedCards = 0;
    private Text cardsLeft;
    
    private float[] SortedRandomDelays(int howMany)
    {
        float[] vals = new float[howMany];

        for (int i = 0; i < howMany; i++) {
            vals[i] = UnityEngine.Random.Range(lowerDelay, upperDelay);
        }

        Array.Sort(vals);
        return vals;
    }
    // Start is called before the first frame update
    void Start()
    {
        cardsLeft = GetComponentInChildren<Text>();

        CreateCards();
        UpdateCardsLeft();
        spawnDelay = deathDelay + upperDelay + cards[1].GetComponent<Tween>().animTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (resetting && usedCards < cardLimit) {
            elapsedTime += Time.deltaTime;

            if(elapsedTime > spawnDelay) {
                CreateCards();
                elapsedTime = 0.0f;
                resetting = false;
            }
        }
    }

    private void UpdateCardsLeft(){
        cardsLeft.text = (cardLimit - usedCards).ToString();
    }

    void CreateCards()
    {   
        float[] delays = SortedRandomDelays(cards.Length);
        for (int i = 0; i < cards.Length; i++){
            cards[i] = Instantiate(cardPrefab, 
                transform.position + new Vector3(cardPoints[i].x, cardPoints[i].y, 0), 
                Quaternion.identity);
            cards[i].transform.SetParent(gameObject.transform);
            if (randomDelay) {
                cards[i].GetComponent<Tween>().delay = delays[i];
            }
        }
    }

    public void ResetCards()
    {
        float[] delays = SortedRandomDelays(cards.Length);
        for (int i = 0; i < cards.Length; i++) {
            float delay = randomDelay ? deathDelay + delays[i] : deathDelay;
            Tween t = cards[i].GetComponents<Tween>()[1];
            t.delay = delay;
            t.Animate();
            Destroy(cards[i], delay + t.animTime);
        }
        usedCards++;
        resetting = true;
        UpdateCardsLeft();
    }
}
