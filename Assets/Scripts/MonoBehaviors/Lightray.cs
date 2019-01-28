using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightray : MonoBehaviour {
    public RaycastHit2D hit;
    //public RaycastHit2D ground;
    public List<float[]> _xShadowRange;
    public int rays = 90;   // Recommend standard unit circle angles
    public GameObject lightLRPrefab;
    private List<GameObject> lines;
    private float startTime;

    private Vector2 center;
    private float angle;
    public float rotSpeed = 0.0001f;
    public float radius = 18f;
    public float rayLen = 20f;
    private float lastX;
    private float lastPos;

    void Start()
    {
        _xShadowRange = new List<float[]>();
        startTime = Time.time;
        center = transform.position;
        lastX = -Mathf.Infinity;
        MakeRays();
        lastPos = Camera.main.transform.position.y;
    }

    void Update()
    {
        float pos = Camera.main.transform.position.y;
        if (lastPos < pos)
        {            
            radius += (pos - lastPos) * 2;
            rayLen += (pos - lastPos) * 4;
            lastPos = pos;
        }

        Raycasting(rayLen);
        PointMove(radius);
    }

    void MakeRays()
    {
        lines = new List<GameObject>();

        for (int i = 0; i < rays; i++)
        {
            GameObject newLine = Instantiate(lightLRPrefab, transform) as GameObject;
            lines.Add(newLine);

            LineRenderer lightLR = lines[i].GetComponent<LineRenderer>();

            Vector3[] initLaserPositions = new Vector3[2] { Vector3.zero, Vector3.zero };
            lightLR.SetPositions(initLaserPositions);
        }
    }

    void Raycasting(float length)
    {

        Vector3 posTrans = Vector3.zero;
        float x, y, ang;

        for (int i = 0; i < rays; i++)
        {
            ang = (360.0f / rays) * i * Mathf.Rad2Deg;
            x = Mathf.Cos(ang);
            y = Mathf.Sin(ang);
            posTrans = new Vector3(x, y, 0);

            Vector3 target = posTrans;
            Vector3 endPos = transform.position + (length * target);

            // NOTE: LINECAST checks for known position; RAYCAST checks in specified direction
            hit = Physics2D.Raycast(transform.position, target, 1 << LayerMask.NameToLayer("ThrowShade"));
            //ground = Physics2D.Raycast(transform.position, target, 1 << LayerMask.NameToLayer("SafeUnsafe"));

            // Debug lines
            if (hit.collider)
            {
                endPos = hit.point;

                //float[] safe = new float[2] { -Mathf.Infinity, -Mathf.Infinity };
                //Debug.Log("K Size: " + _xShadowRange.Count);
                //if (endPos[0] > lastX)
                //{
                //    safe = new float[2] { lastX, endPos[0] };
                //    _xShadowRange.Add(safe);  // Change to x range
                //}
                ////if (!_xShadowRange.Contains(safe)))

                Debug.DrawRay(transform.position, target, Color.cyan);
            }

            LineRenderer lr = lines[i].GetComponent<LineRenderer>();
            lr.SetPosition(0, transform.position);
            lr.SetPosition(1, endPos);
        }
    }

    void PointMove(float radius)
    {
        //Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //point.z = 0;

        angle += rotSpeed * Time.deltaTime;
        var offset = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
        transform.position = center + offset;
    }
}
