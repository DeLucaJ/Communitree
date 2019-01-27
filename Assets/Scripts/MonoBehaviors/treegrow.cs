using System.Collections.Generic;
using UnityEngine;

public class treegrow : MonoBehaviour
{

    class Branch {
        public LineRenderer line;
        public int branchFrom;
        public int branchAt;
        public int children;
        public float length;
        public float growth_step;
        public float midAngle;
        public float angle_var;
        public float max_angle_range;
        public float angleDir;
        public Vector3 lastPoint;
        public Vector3 nextPoint;
        public float lastWidth;
        public float nextWidth;

        public Branch(LineRenderer line) {
            length = 0.0f;
            children = 0;
            this.line = line;
        }

        public Branch(LineRenderer line, Branch parent, float scale, float midAngle, int from, int at) {
            length = 0.0f;
            children = 0;
            this.line = line;
            this.branchFrom = from;
            this.branchAt = at;
            this.growth_step = parent.growth_step * scale;
            this.midAngle = midAngle;
            this.angleDir = this.midAngle;
            this.max_angle_range = parent.max_angle_range * scale;
            this.angle_var = parent.angle_var * scale;
            this.lastPoint = line.GetPosition(0);
            this.nextPoint = this.lastPoint;
            this.lastWidth = line.widthMultiplier;
            this.nextWidth = this.lastWidth;
        }

        public void grow(int degree) {
            // Setup new vertex
            line.positionCount++;
            lastWidth = nextWidth = line.widthMultiplier;
            nextPoint = lastPoint = line.GetPosition(line.positionCount - 2);
            line.SetPosition(line.positionCount - 1, lastPoint);

            // Set target to grow to
            if (angleDir >= midAngle + max_angle_range)
                angleDir += Random.Range(-1.0f, 0.0f) * angle_var;
            else if (angleDir <= midAngle - max_angle_range)
                angleDir += Random.Range(0.0f, 1.0f) * angle_var;
            else
                angleDir += Random.Range(-1.0f, 1.0f) * angle_var;

            nextPoint.x += Mathf.Cos(Mathf.Deg2Rad * angleDir) * growth_step * degree;
            nextPoint.y += Mathf.Sin(Mathf.Deg2Rad * angleDir) * growth_step * degree;
            nextWidth += growth_step * degree / 20.0f;
            length += growth_step * degree;
        }

        public void lerp(AnimationCurve anim_curve, float delta) {
            delta = delta > 1 ? 1 : delta;
            float alpha = anim_curve.Evaluate(delta);
            line.SetPosition(line.positionCount - 1, Vector3.LerpUnclamped(lastPoint, nextPoint, alpha));
            line.widthMultiplier = Mathf.LerpUnclamped(lastWidth, nextWidth, alpha);
        }
    }

    public LineRenderer trunk;
    public GameObject branch_prefab;

    public float scale_factor = 0.7f;
    public float angle_variance = 10.0f;
    public float max_angle_range = 30.0f;
    public float growth_step = 1.0f;


    public float grow_anim_time = 1.0f;
    public AnimationCurve grow_anim_curve;

    public int min_points_to_branch = 6;
    public int max_children = 6;
    public int max_branch_depth = 3;
    public int max_growth = 5;

    [HideInInspector]
    public float min_x = 0;
    [HideInInspector]
    public float max_x = 0;
    [HideInInspector]
    public float max_y = 0;
    [HideInInspector]
    public bool paused = false;

    // Trunk growth
    private List<List<Branch>> branchInfo;    
    private bool animating = false;
    private float elapsedTime;
    private int grows = 0;
    private int total_growth = 0;
    private float last_y = 0;
    private int branch_interval = 8;
    


    // Tree Growth functions    
    public bool isAnimating() {
        return animating;
    }

    private void grow(int degree) {
        animating = true;
        elapsedTime = 0.0f;
        foreach (List<Branch> tiers in branchInfo) {
            foreach (Branch b in tiers) {
                b.grow(degree);
                Vector2 point = b.line.GetPosition(b.line.positionCount - 1);
                max_x = Mathf.Max(point.x, max_x);
                min_x = Mathf.Min(point.x, min_x);
                max_y = Mathf.Max(point.y, max_y);
            }
        }
    }    

    private void setLineParams(LineRenderer line) {
        line.material = trunk.material;
        line.numCornerVertices = trunk.numCornerVertices;
        line.widthCurve = trunk.widthCurve;
        line.useWorldSpace = false;        
    }

    private void addBranch(int tier, int b) {
        Branch parent = branchInfo[tier][b];
        parent.children++; 
        int pos = parent.line.positionCount - 1;
        float dist = 0;
            
        GameObject go = Instantiate(branch_prefab);
        go.transform.SetParent(this.gameObject.transform);
        go.transform.localPosition = Vector3.zero;
        go.transform.localScale = Vector3.one;

        LineRenderer line = go.GetComponent<LineRenderer>();
        setLineParams(line);
        line.widthMultiplier = parent.line.widthMultiplier * trunk.widthCurve.Evaluate(1.0f - dist / parent.length);
        line.SetPosition(0, parent.line.GetPosition(pos));        

        float angle = Random.Range(2.5f * parent.max_angle_range, 3f * parent.max_angle_range);
        angle *= parent.children % 2 == 0 ? 1 : -1;
        angle *= Random.value > 0.1f ? -1 : 1;
        angle = parent.midAngle + angle;

        Branch branch = new Branch(line, parent, scale_factor, angle, 0, pos);

        if (branchInfo.Count == tier + 1)
            branchInfo.Add(new List<Branch>());
        branchInfo[tier + 1].Add(branch);      
    }

    // Start is called before the first frame update
    void Start()
    {
        branchInfo = new List<List<Branch>>();
        branchInfo.Add(new List<Branch>());

        trunk.widthMultiplier = 0.5f;

        Branch data = new Branch(trunk);
        data.branchFrom = -1;
        data.branchAt = -1;
        data.growth_step = growth_step;
        data.midAngle = 90.0f;
        data.angleDir = data.midAngle;
        data.max_angle_range = max_angle_range;
        data.angle_var = angle_variance;
        data.lastPoint = trunk.GetPosition(0);
        data.nextPoint = data.lastPoint;
        data.lastWidth = trunk.widthMultiplier;
        data.nextWidth = data.lastWidth;

        branchInfo[0].Add(data);
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
            return;


        if (animating) {
            elapsedTime += Time.deltaTime;
            float delta = elapsedTime / grow_anim_time;
            foreach (List<Branch> tiers in branchInfo) {
                foreach(Branch b in tiers) {
                    b.lerp(grow_anim_curve, delta);
                }
            }
            animating = elapsedTime < grow_anim_time;

        } else {
            if (total_growth < max_growth && grows <= branch_interval) {
                grow(1);
                grows++;
                total_growth++;
                last_y = this.gameObject.transform.position.y;
            } else if (grows > branch_interval) {
                int depth = branchInfo.Count;
                depth = depth < max_branch_depth ? depth : max_branch_depth;
                for (int i = depth - 1; i >= 0; --i) {
                    for (int b = branchInfo[i].Count - 1; b >= 0; --b)
                        if (branchInfo[i][b].line.positionCount > min_points_to_branch &&
                            branchInfo[i][b].children < max_children - b) 
                            addBranch(i, b);
                }
                grows = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.B)) {
            max_growth += 10;
        }
    }
}
