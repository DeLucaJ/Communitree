using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InhabitantController : MonoBehaviour
{
    public Inhabitant type;
    public int moveIndex;

    [HideInInspector]
    public treegrow tg;

    private SpriteRenderer sr;
    private Tween[] tw;
    private List<Vector2> path;
    private int index = 1;

    private bool go = false;

    private void ApplyData() 
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = type.sprite;
        sr.color = type.color;
        
        tw = GetComponents<Tween>();
    }

    public void Go() {
        SelectBranch();
        go = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        ApplyData();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move() {
        if (go && !tw[moveIndex].IsAnimating()) {
            tw[moveIndex].startVector = new Vector3(path[index - 1].x, path[index - 1].y, 0);
            tw[moveIndex].endVector = new Vector3(path[index].x, path[index].y, 0);
            tw[moveIndex].Animate();
            index++;

            if (index >= path.Count) go = false;
        }
    }

    private void SelectBranch()
    {
        int tier = Random.Range(1, tg.getDepth());
        int branch = Random.Range(0, tg.branchesAtTier(tier));
        int point = Random.Range(0, tg.branchPathLength(tier, branch));
        path = tg.pathToBranch(tier, branch, point);
    }
}
