using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAttributes : MonoBehaviour
{
    public float lightStartWidth = 0.01f;
    public float lightEndWidth = 0.8f;
    public float lightMaxLength = 10.0f;

    Gradient gradient;
    GradientColorKey[] colorKey;
    GradientAlphaKey[] alphaKey;

    // Start is called before the first frame update
    void Start()
    {
        LineRenderer lr = GetComponent<LineRenderer>();
        lr.GetComponent<Renderer>().material.SetColor("_TintColor", new Color(1, 1, 1, 0.5f));

        gradient = new Gradient();
        // Populate the color keys at the relative time 0 and 1 (0 and 100%)
        colorKey = new GradientColorKey[2];
        colorKey[0].color = Color.white;
        colorKey[0].time = 0.0f;
        colorKey[1].color = Color.cyan;
        colorKey[1].time = 1.0f;

        // Populate the alpha keys at relative time 0 and 1  (0 and 100%)
        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 0.2f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);

        //// Color at the relative time 0.25 (25 %)
        //Debug.Log(gradient.Evaluate(0.25f));

        lr.colorGradient = gradient;
        lr.enabled = true;
    }
}
