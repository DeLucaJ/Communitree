using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDock : MonoBehaviour
{
    public Vector2Int[] cardPoints;
    public GameObject cardPrefab;
    public bool randomDelay;
    public float lowerDelay;
    public float upperDelay;
    public float deathDelay;

    private GameObject[] cards = new GameObject[3];
    private bool resetting = false;
    private float elapsedTime = 0.0f;
    private float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        CreateCards();
        spawnDelay = deathDelay + upperDelay + cards[1].GetComponent<Tween>().animTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (resetting) {
            elapsedTime += Time.deltaTime;

            if(elapsedTime > spawnDelay) {
                CreateCards();
                elapsedTime = 0.0f;
                resetting = false;
            }
        }
    }

    void CreateCards()
    {
        for (int i = 0; i < cards.Length; i++){
            cards[i] = Instantiate(cardPrefab, 
                transform.position + new Vector3(cardPoints[i].x, cardPoints[i].y, 0), 
                Quaternion.identity);
            cards[i].transform.SetParent(gameObject.transform);
            if (randomDelay) cards[i].GetComponent<Tween>().delay = UnityEngine.Random.Range(lowerDelay, upperDelay);
        }
    }

    public void ResetCards()
    {
        for (int i = 0; i < cards.Length; i++) {
            float delay = randomDelay ? deathDelay + UnityEngine.Random.Range(lowerDelay, upperDelay) : deathDelay;
            Tween t = cards[i].GetComponents<Tween>()[1];
            t.delay = delay;
            t.Animate();
            Destroy(cards[i], delay + t.animTime);
        }

        //CreateCards();
        resetting = true;
    }
}
