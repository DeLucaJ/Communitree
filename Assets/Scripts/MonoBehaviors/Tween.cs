using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tween : MonoBehaviour
{
    public enum TweenType { Position, Rotation, Scale };

    public bool atStart;
    public AnimationCurve curve;
    public float animTime;
    public Vector3 startVector;
    public Vector3 endVector;
    public TweenType type;
    public float delay = 0.0f;

    public bool relative;


    //private anim variables
    private bool animating = false;
    private float elapsedTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (atStart) Animate();

        switch(type){
            case TweenType.Position:
                gameObject.transform.localPosition = startVector = relative ? gameObject.transform.localPosition + startVector : startVector;
                endVector = relative ? gameObject.transform.localPosition + endVector : endVector;
                break;
            case TweenType.Rotation:
                gameObject.transform.localRotation = relative ? Quaternion.Euler(startVector = gameObject.transform.localRotation.eulerAngles + startVector) : Quaternion.Euler(startVector);
                endVector = relative ? gameObject.transform.localRotation.eulerAngles + endVector : endVector;
                break;
            case TweenType.Scale:
                gameObject.transform.localScale = startVector = relative ? gameObject.transform.localScale + startVector : startVector;
                endVector = relative ? gameObject.transform.localScale + endVector : endVector;
                break;
            default:
                break;
        }
    }

    public void Animate()
    {
        animating = true;
        elapsedTime = 0.0f; 
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (animating) {
            elapsedTime += Time.deltaTime;
            
            if (elapsedTime >= delay) {
                float delta = (elapsedTime - delay) / (animTime);

                switch(type){
                    case TweenType.Position:
                        gameObject.transform.localPosition= Vector3.LerpUnclamped(
                            startVector, 
                            endVector, 
                            curve.Evaluate(delta)
                            );
                        break;
                    case TweenType.Rotation:
                        gameObject.transform.localRotation= Quaternion.Euler(Vector3.LerpUnclamped(
                            startVector, 
                            endVector, 
                            curve.Evaluate(delta)
                            ));
                        break;
                    case TweenType.Scale:
                        gameObject.transform.localScale= Vector3.LerpUnclamped(
                            startVector, 
                            endVector, 
                            curve.Evaluate(delta));
                        break;
                    default:
                        break;
                }

                animating = delta < 1.0f;
            }
        }
    }
}
