using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleGround : MonoBehaviour
{
    Vector3 initPos;

    private void Start()
    {
        initPos = transform.position;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        CameraController cam = gameObject.transform.parent.GetComponent<CameraController>();
        if (cam.animating)
        {
            gameObject.transform.localScale += new Vector3(1.25f, 0, 0);
            gameObject.transform.position = initPos;
        }
    }
}
