using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Astar : MonoBehaviour
{
    public class AstarNode : Node
    {
        public float heuristique = 0;

        public AstarNode(Vector2 position, float distance, float heuristique) : base(position,distance)
        {
            this.heuristique = heuristique;
        }

        public AstarNode(Vector2 position, float distance, Node parent, float heuristique) : base(position,distance,parent)
        {
            this.heuristique = heuristique;
        }
    }

    private EnemyController controller;
    private Animator myAni;

    private Transform target;
    public float speed;
    public LayerMask obstacles;
    private float rayCircleCastRadius = 0.15f;
    private int nbfailures = 0;
    private Vector2 horizontal = new Vector2(0.5f, -0.25f), vertical = new Vector2(0.5f, 0.25f);

    private List<AstarNode> unVisited = new List<AstarNode>();
    private List<AstarNode> visited = new List<AstarNode>();
    private AstarNode currentNode;
    private float minDistanceToPlayer = 0.45f;

    private List<Vector2> invertedPath = new List<Vector2>();
    private int pathIndex = 0;

    private float searchColdownElapsed = 0;
    public float searchColdown = 1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        controller = GetComponent<EnemyController>();
        myAni = GetComponent<Animator>();
    }
    void Start()
    {
        target = controller.playerT;
        calculatePath();
        pathIndex = invertedPath.Count - 1;
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector2.Distance(transform.position, target.position) < minDistanceToPlayer) return;

        // Movement
        if (invertedPath.Count > 0 && pathIndex < invertedPath.Count)
        {
            followPath();
        }


        searchColdownElapsed += Time.deltaTime;
        if (searchColdownElapsed > searchColdown)
        {
            invertedPath.Clear();

            calculatePath();

            pathIndex = invertedPath.Count - 1;
            searchColdownElapsed = 0;
        }

    }

    private void setSprite(Vector2 velocity)
    {

        if (velocity.x < 0f)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            transform.eulerAngles = Vector3.zero;
        }

        if (velocity.y < 0 && velocity.x == 0f)
        {
            myAni.SetFloat("VelocityX", Mathf.Abs(velocity.y));
        }
        else
        {

            myAni.SetFloat("VelocityY", velocity.y);
            myAni.SetFloat("VelocityX", Mathf.Abs(velocity.x));

        }

    }

    private void followPath()
    {

        if (pathIndex < 0)
        {
            visited.Clear();
            unVisited.Clear();
            invertedPath.Clear();

            calculatePath();

            pathIndex = invertedPath.Count - 1;
            searchColdownElapsed = 0;
        }

        Vector2 nextPos = invertedPath[pathIndex];

        transform.position = Vector2.MoveTowards(transform.position, nextPos, speed * Time.deltaTime);

        Vector2 velocity = nextPos - (Vector2)transform.position;
        setSprite(velocity.normalized);

        if (Vector2.Distance(transform.position, nextPos) < 0.01f)
        {
            pathIndex--; // Move to next node
        }
    }

    private void calculatePath()
    {
        float h = Vector2.Distance(transform.position, target.position);
        currentNode = new AstarNode(transform.position, 0, h );
        visited.Add(currentNode);


        int temp = 0;
        while (temp < 9999)
        {

            AstarNode nextNeighbor = findNextNeightbor(currentNode);
            if (nextNeighbor == null)
            {
                nbfailures++;
                if (nbfailures > 3) {
                    rayCircleCastRadius = 0.08f;
                    nbfailures = 0;
                }
                break;
            }
            else
            {
                currentNode = nextNeighbor;
            }

            if (Vector2.Distance(currentNode.position, target.position) < minDistanceToPlayer)
            {
                rayCircleCastRadius = 0.18f;
                nbfailures = 0;
                break;
            }

            temp++;

        }
        temp = 0;

        unVisited.Clear();
        visited.Remove(currentNode);
        invertedPath.Add(currentNode.position);
        Node nextNode = currentNode.parent;

        while (nextNode != null && temp < 999)
        {
            invertedPath.Add(nextNode.position);
            nextNode = nextNode.parent;
            temp++;
        }

        visited.Clear();
    }

    private AstarNode findNextNeightbor(AstarNode currentNode)
    {
        float newDistance = currentNode.distance + 1;
        generateNeightbors(newDistance, currentNode.heuristique);

        float min = float.PositiveInfinity;
        AstarNode result = null;
        foreach (AstarNode n in unVisited)
        {


            if ((n.distance + n.heuristique) < min)
            {
                min = (n.distance + n.heuristique);
                result = n;
            }

        }

        unVisited.Remove(result);
        visited.Add(result);

        return result;
    }

    private void generateNeightbors(float newDistance, float currentHeuristique)
    {
        List<Vector2> straightNeighbors = new List<Vector2>();
        List<Vector2> diagonalNeighbors = new List<Vector2>();

        // we go clockwise
        //----------Straight-------------
        Vector2 up = currentNode.position + vertical;
        straightNeighbors.Add(up);

        Vector2 right = currentNode.position + horizontal;
        straightNeighbors.Add(right);

        Vector2 down = currentNode.position - vertical;
        straightNeighbors.Add(down);

        Vector2 left = currentNode.position - horizontal;
        straightNeighbors.Add(left);

        //-----------Diagonals------------
        Vector2 upRight = currentNode.position + (vertical + horizontal);
        diagonalNeighbors.Add(upRight);

        Vector2 downRight = currentNode.position + (-vertical + horizontal);
        diagonalNeighbors.Add(downRight);

        Vector2 downLeft = currentNode.position + (-vertical - horizontal);
        diagonalNeighbors.Add(downLeft);

        Vector2 upLeft = currentNode.position + (vertical - horizontal);
        diagonalNeighbors.Add(upLeft);


        foreach (Vector2 v in straightNeighbors.ToList())
        {
            float h = Vector2.Distance(v, target.position);

            if (obstacleCheck(v) || h > currentHeuristique || visited.Exists(x => x.position == v) )
            {
                continue;
            }

            AstarNode update = unVisited.Find(x => x.position == v);

            if (update != null)
            {
                if (newDistance < update.distance)
                {
                    update.distance = newDistance;
                    update.parent = currentNode;
                    
                }
            }
            else
            {
               
                unVisited.Add(new AstarNode(v, newDistance, currentNode,h));
            }
        }

        foreach (Vector2 v in diagonalNeighbors.ToList())
        {
            float h = Vector2.Distance(v, target.position);

            if (obstacleCheck(v) || h > currentHeuristique || visited.Exists(x => x.position == v) )
            {
                continue;
            }


            AstarNode update = unVisited.Find(x => x.position == v);

            if (update != null)
            {
                if (newDistance < update.distance)
                {
                    update.distance = newDistance;
                    update.parent = currentNode;
                }

            }
            else
            {
                
                unVisited.Add(new AstarNode(v, newDistance + 1, currentNode,h));
            }
        }


       


    }

    private bool obstacleCheck(Vector2 toTest)
    {

        Collider2D collider = Physics2D.OverlapCircle(toTest, rayCircleCastRadius, obstacles);

        if (collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    private void OnDrawGizmos()
    {
       // Draw a yellow sphere at the transform's position
        /*foreach (AstarNode n in unVisited)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(n.position, 0.2f);
        }

        foreach (AstarNode n in visited)
        {
            // Draw a yellow sphere at the transform's position
            if(n == null)
            {
                continue;
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(n.position, 0.2f);
        }

        foreach (Vector2 v in invertedPath)
        {
            if (v == null) continue;
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(v, 0.2f);

        }


        if (currentNode != null)
        {

            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(currentNode.position, 0.2f);
        }*/


    }
}
