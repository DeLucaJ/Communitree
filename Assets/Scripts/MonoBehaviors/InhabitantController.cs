using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhabitantController : MonoBehaviour
{
    public Inhabitant type;
    
    private SpriteRenderer sr;
    private treegrow tg;

    private void ApplyData() 
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = type.sprite;
        sr.color = type.color;
    }

    // Start is called before the first frame update
    void Start()
    {
        ApplyData();
        SelectBranch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        transform.Translate(Vector3.up * Time.deltaTime);
    }
}
