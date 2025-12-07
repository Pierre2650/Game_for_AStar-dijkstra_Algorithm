using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dijkstra_PathFind : MonoBehaviour
{
    private EnemyController controller;
    private Animator myAni;
    private Transform target;
    public float speed;
    public LayerMask obstacles;
    private float rayCircleCastRadius =  0.15f;
    private int nbfailures = 0;
    private Vector2 horizontal = new Vector2(0.5f, -0.25f), vertical = new Vector2(0.5f, 0.25f);


    private List<Node> unVisited = new List<Node>();
    private List<Node> visited = new List<Node>();
    private Node currentNode;
    private float minDistanceToPlayer = 0.45f;


    private List<Vector2> invertedPath = new List<Vector2>();
    private int pathIndex = 0;

    
    private float searchColdownElapsed = 0;
    private float searchColdown = 8;


    private void Awake()
    {
        controller = GetComponent<EnemyController>();
        myAni = GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = controller.playerT;
        calculatePath();
        pathIndex = invertedPath.Count - 1;
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, target.transform.position) > 10f )
        {
            searchColdown = 8f;
        }
        else if (Vector2.Distance(transform.position, target.transform.position) > 5f &&  Vector2.Distance(transform.position, target.transform.position) < 10f)
        {
            searchColdown = 4f;
        }
        else
        {
            searchColdown = 0.5f;
        }

       


        if (Vector2.Distance(transform.position, target.position) < minDistanceToPlayer) return;

        // Movement
        if (invertedPath.Count > 0 && pathIndex < invertedPath.Count)
        {
            followPath();
        }


        searchColdownElapsed += Time.deltaTime;
        if(searchColdownElapsed > searchColdown)
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
        currentNode = new Node(transform.position, 0);
        visited.Add(currentNode);

        int temp = 0;
        while (temp < 999)
        {


            Node nextNeighbor = findNextNeightbor(currentNode);
            if (nextNeighbor == null)
            {
                nbfailures++;
                if (nbfailures > 3)
                {
                    rayCircleCastRadius = 0.05f;
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
                rayCircleCastRadius = 0.15f;
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
            visited.Remove(currentNode);
            invertedPath.Add(nextNode.position);
            nextNode = nextNode.parent;
            temp++;
        }

        visited.Clear();
    }



    private bool obstacleCheck( Vector2 toTest)
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
    private Node findNextNeightbor(Node currentNode)
    {
        float newDistance = currentNode.distance + 1;
        generateNeightbors(newDistance);

        float min = float.PositiveInfinity;
        Node result = null;
        foreach (Node n in unVisited) {

            
            if(n.distance < min) 
            {
                min = n.distance;
                result = n;
            }

        }

        if (result == null) {
            int rand = Random.Range(0, unVisited.Count);

            try
            {
                result = unVisited[rand];
            }
            catch
            {
                Destroy(gameObject);
            }
        }

      
        unVisited.Remove(result);
        visited.Add(result);

        return result;
    }

    private void generateNeightbors(float newDistance)
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
            if (obstacleCheck(v) || visited.Exists(x => x.position == v))
            {
                continue;
            }

            Node update = unVisited.Find(x => x.position == v);

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
                unVisited.Add(new Node(v, newDistance, currentNode));
            }
        }

        foreach (Vector2 v in diagonalNeighbors.ToList())
        {
            if (obstacleCheck(v) || visited.Exists(x => x.position == v))
            {
                continue;
            }

            Node update = unVisited.Find(x => x.position == v);

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
                unVisited.Add(new Node(v, newDistance + 1, currentNode));
            }
        }

    }


    private void OnDestroy()
    {
        controller.spawner.nbDijkstra--;
    }



    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        /*foreach (Node n in unVisited) {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(n.position, 0.2f);
        }

        foreach (Node n in visited)
        {
            // Draw a yellow sphere at the transform's position
            if (n == null)
            {
                continue;
            }
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(n.position, 0.2f);
        }

        foreach (Vector2 v in invertedPath)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(v, 0.2f);

        }


        if (currentNode != null) {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(currentNode.position, 0.2f);
        }*/


    }

}
