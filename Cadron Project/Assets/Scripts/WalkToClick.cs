using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WalkToClick : MonoBehaviour
{
    private bool[,] grid;
    public Tilemap tilemap;
    private Dictionary<Vector3, Vector3> parentmap;
    private Rigidbody2D rb;
    private Stack<Vector3> path;
    private int t;
    private int b;
    private int l;
    private int r;
    private Vector3 mouseWorldPos;
    private bool newDestination;
    private bool done;
    
    void Start()
    {
        path = new Stack<Vector3>();    
        done = true;
        TilemapCollider2D tilemapCollider = tilemap.GetComponent<TilemapCollider2D>();  
        Vector2 tilemapCenter = tilemapCollider.bounds.center;
        Vector2 tilemapExtents = tilemapCollider.bounds.extents;    

        l = (int)Mathf.Round(tilemapCenter.x - tilemapExtents.x - 6);
        t = (int)Mathf.Round(tilemapCenter.y + tilemapExtents.y + 6);
        r = (int)Mathf.Round(tilemapCenter.x + tilemapExtents.x + 6);
        b = (int)Mathf.Round(tilemapCenter.y - tilemapExtents.y - 6);   

        grid = new bool[4 * (t - b + 1), 4 * (r - l + 1)];
        for (int x = 0; x < 4 * (r - l + 1); x++) {
            for (int y = 0; y < 4 * (t - b + 1); y++) {
                Vector3 cellPosition = new Vector3(((float) 1 + x + 4 * l) / 4f,((float) 1 + y + 4 * b) / 4f);
                grid[y, x] = !IsCellOccupied(cellPosition);
            //    print($"cell positioncellPosition: {cellPosition}, cell center: {cellCenter}");
            //    if (grid[y,x]) { print("hit! " + (x + l) + ", " + (y + b)); }
            }
        }   

        rb = GetComponent<Rigidbody2D>();
    }

    private bool IsCellOccupied(Vector3 cellCenter)
    {
        Vector3Int cellPosition = tilemap.WorldToCell(cellCenter);
        (int, int)[] playersize = {(0,0), (0,-1), (1,0), (1,-1),(0,-2),(-1,-2) };
        foreach ((int,int) ps in playersize) {
            if (tilemap.HasTile(new Vector3Int(cellPosition.x + ps.Item1, cellPosition.y + ps.Item2, 0))) {
                return true;
            }
            Collider2D[] near = Physics2D.OverlapCircleAll(cellCenter, 0.125f);
            foreach (Collider2D n in near) {
                if (n.CompareTag("NPC")) { 
                    return true;  
                }
            }
        } return false;
    }


    public Stack<Vector3> AStar(Vector3 end) {

        parentmap = new Dictionary<Vector3, Vector3>();
        
        (int, int) startc = ((int) Mathf.Round(4*transform.position.x), (int) Mathf.Round(4*transform.position.y));
        (int, int)   endc = ((int) Mathf.Round(4*               end.x), (int) Mathf.Round(4*               end.y));

        Node s       = new Node(startc, DistanceEstimate(startc, endc));
        Node e       = new Node(endc,   DistanceEstimate(startc, endc));
        Node current = s;

        //open contains all nodes, sorted by heuristic value
        SortedSet<Node>       open = new SortedSet<Node>(new NodeComparer());
        open.Add(s);

        //closed contains all previously explored positions
        HashSet<(int, int)> closed = new   HashSet<(int, int)>();
        int path_length = 0;
        while (open.Count > 0) {
            current = open.Min;
            open.Remove(current);
            path_length++;
            foreach ((int, int) loc in neighbors(current)) {
                if (loc.Item2 - (4*b) < 4 * (t - b + 1) && loc.Item1 - (4*l) < 4 * (r - l + 1)) {
                    if (!(closed.Contains(loc)) && grid[loc.Item2 - (4*b), loc.Item1 - (4*l)]) {
                        // value = dist from start + dist from end
                        float g = path_length;
                        float h = DistanceEstimate(endc,   loc);
                        Node newNode = new Node(loc, h); 
                        open.Add(newNode);
                        parentmap[vec(newNode.position)] = vec(current.position);
                    }
                }
            }
            closed.Add(current.position);
        }
        Stack<Vector3> path = new Stack<Vector3>();
        Vector3 x = vec(endc);
        while (parentmap.ContainsKey(x)) {
            path.Push(x);
            x = parentmap[x];            
        }
        return path;
    }

public (int, int)[] neighbors(Node n)
{
    int x = n.position.Item1;
    int y = n.position.Item2;
    (int, int)[] ns = { (x + 1, y), (x - 1, y), (x, y + 1), (x, y - 1) };
    List<(int, int)> validNeighbors = new List<(int, int)>();
    foreach (var neighbor in ns) {
        if (IsValidNeighbor(neighbor)) {
            validNeighbors.Add(neighbor); } }
    return validNeighbors.ToArray();
}

private bool IsValidNeighbor((int, int) n)
{
    int nx = n.Item1;
    int ny = n.Item2;
    return (nx >= 4 * l && nx < 4 * (r + 1) && ny >= 4 * b && ny < 4 * (t + 1));
}
    public float DistanceEstimate((int, int) v1, (int, int) v2) {
        float dx = Mathf.Abs(v1.Item1 - v2.Item1);
        float dy = Mathf.Abs(v1.Item2 - v2.Item2);
        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.isBusy() || Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) { path = new Stack<Vector3>(); }
        if (Input.GetMouseButtonDown(0)) {
            mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mouseWorldPos.z = 0f;
            // rb.velocity = new Vector2(mouseWorldPos.x - transform.position.x, mouseWorldPos.y - transform.position.y);
            Vector3 v3 = new Vector3(Mathf.Round(mouseWorldPos.x), Mathf.Round(mouseWorldPos.y), 0);
            Collider2D[] cols = Physics2D.OverlapCircleAll(mouseWorldPos, 0.125f);
            bool b = true;
            foreach (Collider2D col in cols) { if (col.CompareTag("NPC")) { b = false; } }
            if (b && !GameManager.Instance.isBusy()) {
                path = AStar(mouseWorldPos);
                done = false;
                string ot = "";
                foreach (Vector3 p in path) { 
                    ot +=  $"({p.x}, {p.y})";
                }
                print(ot);
            }
        }
        if (path.Count > 0) {
            Vector3 targetDirection = (path.Peek() - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, path.Peek(), 1.5f * Time.deltaTime);
            rb.velocity = new Vector2(targetDirection.x, targetDirection.y);
            if (Vector3.Distance(transform.position, path.Peek()) < 0.05) {
                path.Pop();                
                newDestination = true;
            } 
            if (newDestination) {
                if      (targetDirection.x >  0.01) { PlayerMovement.SetHorizontal(1);  PlayerMovement.SetVertical(0); } 
                else if (targetDirection.x < -0.01) { PlayerMovement.SetHorizontal(-1); PlayerMovement.SetVertical(0); } 
                else if (targetDirection.y >  0.01) { PlayerMovement.SetHorizontal(0); PlayerMovement.SetVertical(1); } 
                else if (targetDirection.y < -0.01) { PlayerMovement.SetHorizontal(0); PlayerMovement.SetVertical(-1); } 
                else    { PlayerMovement.SetHorizontal(0); PlayerMovement.SetVertical(0); } 
                newDestination = false;
            }
        }
        else { 
            PlayerMovement.SetHorizontal(0); 
            PlayerMovement.SetVertical(0); 
        }
    }   
    void OnCollisionEnter2D(Collision2D collision) {
        // if (collision.gameObject.tag == "NPC") {
            path = new Stack<Vector3>();
            newDestination = true;
            rb.velocity = new Vector2(0f, 0f);
        // }
    }

    Vector3 vec((int, int) coord) {
        Vector3 v = new Vector3(((float) coord.Item1)/4f,((float) coord.Item2)/4f, 0f);
        return v;
    }
}
