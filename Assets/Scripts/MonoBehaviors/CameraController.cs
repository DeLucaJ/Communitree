using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public treegrow tg;    
    public float buffer_scale = 0.85f;
    public AnimationCurve anim_curve;
    public float anim_time = 1.0f;

    private Camera camera;
    private float elapsedTime;
    public bool animating;  // Public to scale bkg gradient
    private Vector3 old_pos;
    private Vector3 target_pos;
    private float old_size;
    private float target_size;
    private int resizes = 0;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float cam_max_y = camera.transform.position.y + camera.orthographicSize;
        if (animating) {
            elapsedTime += Time.deltaTime;
            float delta = Mathf.Clamp(elapsedTime / anim_time, 0.0f, 1.0f);

            camera.orthographicSize = Mathf.LerpUnclamped(old_size, target_size, anim_curve.Evaluate(delta));
            camera.transform.position = Vector3.LerpUnclamped(old_pos, target_pos, anim_curve.Evaluate(delta));
            animating = delta < 1.0f;
            tg.paused = animating;
        }
        else if (tg.max_y > buffer_scale * cam_max_y) {
            old_pos = camera.transform.position;
            target_pos = old_pos + new Vector3(0, camera.orthographicSize) * .5f * Mathf.Pow(0.95f, resizes);
            old_size = camera.orthographicSize;
            target_size = old_size * 1.5f * Mathf.Pow(0.95f, resizes);

            //resizes++;
            elapsedTime = 0.0f;
            animating = true;
            tg.paused = animating;
        }
    }
}
