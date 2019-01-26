using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDock : MonoBehaviour
{
    public Vector2Int[] cardPoints;
    public GameObject cardPrefab;

    private GameObject[] cards = new GameObject[3];

    // Start is called before the first frame update
    void Start()
    {
        CreateCards();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateCards()
    {
        for (int i = 0; i < cardPoints.Length; i++){
            cards[i] = Instantiate(cardPrefab, 
                transform.position + new Vector3(cardPoints[i].x, cardPoints[i].y, 0), 
                Quaternion.identity);
            cards[i].transform.SetParent(gameObject.transform);
        }
    }
}
