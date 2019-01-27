using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafColor : MonoBehaviour
{
    public Vector3 variance;

    // Start is called before the first frame update
    void Start()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.color = sr.color + new Color(
            (Random.value - 0.5f) * variance.x,
            (Random.value - 0.5f) * variance.y,
            (Random.value - 0.5f) * variance.z,  
            1.0f
        );
    }
}
