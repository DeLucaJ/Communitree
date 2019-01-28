using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeSpecial : MonoBehaviour
{

    public treegrow tg;
    public GameObject button;
    private int count = 0;
    private int max_count = 8;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (count < max_count && !tg.isGrowing()) {
            count++;
            tg.start_grow();
        } else if (count >= max_count) {
            button.SetActive(true);
        }
            
    }
}
