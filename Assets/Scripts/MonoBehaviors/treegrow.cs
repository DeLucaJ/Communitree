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
        public float growthNormalizer;

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
            this.growthNormalizer = parent.growthNormalizer * scale;
        }

        public void grow(int degree, AnimationCurve growth_curve) {
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

            float scale = growth_step * degree * growth_curve.Evaluate(Mathf.Clamp(line.positionCount / growthNormalizer, 0.0f, 1.0f));

            nextPoint.x += Mathf.Cos(Mathf.Deg2Rad * angleDir) * scale;
            nextPoint.y += Mathf.Sin(Mathf.Deg2Rad * angleDir) * scale;
            nextWidth += scale / 20.0f;
            length += scale;
        }

        public void lerp(AnimationCurve anim_curve, float delta) {
            delta = delta > 1 ? 1 : delta;
            float alpha = anim_curve.Evaluate(delta);
            line.SetPosition(line.positionCount - 1, Vector3.LerpUnclamped(lastPoint, nextPoint, alpha));
            line.widthMultiplier = Mathf.LerpUnclamped(lastWidth, nextWidth, alpha);
        }
    }

    public LineRenderer trunk;
    public GameObject leaf_prefab;
    public GameObject fruit_prefab;
    public GameObject branch_prefab;

    public float scale_factor = 0.7f;
    public float angle_variance = 10.0f;
    public float max_angle_range = 30.0f;
    public float growth_step = 1.0f;
    public AnimationCurve growth_curve;


    public float grow_anim_time = 1.0f;
    public AnimationCurve grow_anim_curve;

    public int min_points_to_branch = 6;
    public int max_branch_depth = 3;
    public int max_children = 6;    
    public int growth = 5;
    public int leaf_factor = 1;
    public int fruit_factor = 1;

    [HideInInspector]
    public float min_x = 0;
    [HideInInspector]
    public float max_x = 0;
    [HideInInspector]
    public float max_y = 0;
    [HideInInspector]
    public bool paused = false;

    // Trunk growth
    private float trunk_veer = 1f;
    private List<List<Branch>> branchInfo;    
    private bool animating = false;
    private float elapsedTime;
    private int grows = 0;
    private int total_growth = 0;
    private int branch_interval = 3;
    private int grow_steps;
    private float simplify_tolerance = 0.001f;
    private int leaves = 0;
    private int fruits = 0;
    private int branches = 1; //Start w/ Trunk
    private bool grewLeaves = false;
    private bool grewFruit = false;


    // Tree Growth functions    
    public int leafCount() {
        return leaves;
    }

    public int fruitCount() {
        return fruits;
    }

    public int branchCount() {
        return branches;
    }

    public int getHeight() {
        return branchInfo[0][0].line.positionCount;
    }

    public int getDepth() {
        return branchInfo.Count;
    }

    public int branchesAtTier(int tier) {
        return branchInfo[tier].Count;
    }

    public int branchPathLength(int tier, int branch) {
        return branchInfo[tier][branch].line.positionCount;
    }

    public List<Vector2> pathToBranch(int tier, int branch, int pos) {
        Branch b = branchInfo[tier][branch];
        List<Vector2> path = new List<Vector2>();
        for (int p = 0; p < pos; p += 3) {
            if (p > pos) p = pos;
            path.Add(b.line.GetPosition(p));
        }
        if (tier == 0 && b.branchFrom == -1) {
            return path;
        } else {
            List<Vector2> partialPath = pathToBranch(tier - 1, b.branchFrom, b.branchAt);
            partialPath.AddRange(path);
            return partialPath;
        }
    }

    public bool isAnimating() {
        return animating;
    }

    public bool isGrowing() {
        return grow_steps < growth || isAnimating() || !grewFruit || !grewLeaves;
    }

    public void start_grow() {
        grow_steps = 0;
        grewLeaves = false;
        grewFruit = false;
    }

    private void grow(int degree) {
        animating = true;
        elapsedTime = 0.0f;
        foreach (List<Branch> tiers in branchInfo) {
            foreach (Branch b in tiers) {
                b.grow(degree, growth_curve);
                Vector2 point = b.line.GetPosition(b.line.positionCount - 1);
                max_x = Mathf.Max(point.x, max_x);
                min_x = Mathf.Min(point.x, min_x);
                max_y = Mathf.Max(point.y, max_y);

                if (b.branchFrom == -1)
                    b.midAngle += trunk_veer;
            }
        }
    }    

    private void setLineParams(LineRenderer line) {
        line.material = trunk.material;
        line.numCornerVertices = trunk.numCornerVertices;
        line.widthCurve = trunk.widthCurve;        
        line.endColor  = trunk.endColor;
        line.startColor = trunk.startColor;
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

        branches++;      
    }

    private int leafCuttoff = 8;
    private void growLeaves() {
        float chance = leaf_factor / 100.0f + 0.05f;
        float toLookAt = 0.1f + 0.1f * chance;
        float scale = 0.9f;

        foreach (List<Branch> tiers in branchInfo) {
            foreach (Branch b in tiers) {
                if (b.line.positionCount > leafCuttoff) {
                    int count = 0;
                    int max = (int) (b.line.positionCount * toLookAt) + 1;
                    int pos = b.line.positionCount;
                    float delay = 0;
                    while (count < max) {
                        pos--;
                        count++;
                        if (chance > Random.value) {
                            leaves++;
                            GameObject go = Instantiate(leaf_prefab);
                            go.transform.SetParent(this.gameObject.transform);
                            go.transform.localPosition = b.line.GetPosition(pos);
                            float angle = b.midAngle - 90.0f;
                            angle += Random.Range(-90.0f, 90.0f) * scale;
                            go.transform.Rotate(new Vector3(0, 0, angle));
                            Tween twn = go.GetComponent<Tween>();
                            twn.delay = delay;
                            delay += 0.05f;
                            twn.startVector = Vector3.zero;
                            twn.endVector = Vector3.one;
                            twn.type = Tween.TweenType.Scale;
                            twn.Animate();
                            if (chance > Random.value) {
                                pos++;
                                count--;
                            }
                        }
                    }
                }
            }
            scale *= scale_factor;
        }
    }

    private int fruitCuttoff = 10;
    private void growFruit() {
        float chance = fruit_factor / 100.0f + 0.05f;
        float toLookAt = 0.1f + 0.1f * chance;
        float scale = 1.0f;
        float delay = 0;

        foreach (List<Branch> tiers in branchInfo) {
            foreach (Branch b in tiers) {
                if (b.line.positionCount > fruitCuttoff) {
                    bool done = false;
                    int pos = b.line.positionCount - Random.Range(7, 10);                 
                    while (!done) {
                        done = true;
                        if (b.branchFrom != -1 && chance > Random.value) {
                            fruits++;
                            GameObject go = Instantiate(fruit_prefab);
                            go.transform.SetParent(this.gameObject.transform);
                            go.transform.localPosition = b.line.GetPosition(pos);
                            go.transform.localPosition += new Vector3(Random.Range(-2.0f, 2.0f), Random.Range(-0.5f, 0.5f), 0) * scale;
                            Tween[] tweens = go.GetComponents<Tween>();
                            Tween twn = tweens[0];
                            twn.delay = delay;
                            tweens[1].delay = delay;
                            delay += 0.08f;
                            twn.startVector = Vector3.zero;
                            twn.endVector = Vector3.one * scale;
                            twn.type = Tween.TweenType.Scale;
                            twn.Animate();
                            if (chance > Random.value)
                                done = false;
                        }
                    }
                    
                }
            }
            scale *= scale_factor;
        }
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
        data.growthNormalizer = 100.0f;

        branchInfo[0].Add(data);
        trunk_veer *= Random.value > 0.5f ? -1 : 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (paused)
            return;

        if (animating) {
            elapsedTime += Time.deltaTime;
            float delta = elapsedTime / grow_anim_time;
            animating = elapsedTime < grow_anim_time;
            foreach (List<Branch> tiers in branchInfo) {
                foreach(Branch b in tiers) {
                    b.lerp(grow_anim_curve, delta);
                    if (!animating) b.line.Simplify(simplify_tolerance);
                }
            }            

        } else if (grow_steps < growth) {
            if ( grows <= branch_interval) {
                grow(1);
                grows++;
                total_growth++;
                grow_steps++;
            } else if (grows > branch_interval) {
                int depth = branchInfo.Count;
                depth = depth < max_branch_depth ? depth : max_branch_depth;
                for (int i = depth - 1; i >= 0; --i) {
                    for (int b = branchInfo[i].Count - 1; b >= 0; --b) {
                        int tierMaxChildren = max_children / ((int)Mathf.Pow(2, i));
                        if (branchInfo[i][b].line.positionCount > min_points_to_branch &&
                            branchInfo[i][b].children < tierMaxChildren &&
                            (tierMaxChildren - branchInfo[i][b].children) / ((float) tierMaxChildren) > Random.value)
                            addBranch(i, b);
                    }
                }
                grows = 0;
            }
        } else if (!grewLeaves) {
            growLeaves();
            grewLeaves = true;

        } else if (!grewFruit) {
            growFruit();
            grewFruit = true;
        }
    }
}
