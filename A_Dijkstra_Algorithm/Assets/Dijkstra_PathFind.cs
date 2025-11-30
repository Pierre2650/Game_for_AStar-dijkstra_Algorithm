using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dijkstra_PathFind : MonoBehaviour
{
    public Transform target;
    public float speed;
    public LayerMask obstacles;
    private Vector2 horizontal = new Vector2(0.5f, -0.25f), vertical = new Vector2(0.5f, 0.25f);


    private List<Node> unVisited = new List<Node>();
    private List<Node> visited = new List<Node>();
    private Node currentNode;
    private float minDistanceToPlayer = 0.45f;


    private List<Vector2> invertedPath = new List<Vector2>();
    private int pathIndex = 0;

    
    private float searchColdownElapsed = 0;
    private float searchColdown = 3;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        calculatePath();
        pathIndex = invertedPath.Count - 1;
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, target.transform.position) > 13f )
        {
            searchColdown = 1f;
        }
        else
        {
            searchColdown = 0.5f;
        }

        Debug.Log("(Vector2.Distance(transform.position, target.transform.position) = " + Vector2.Distance(transform.position, target.transform.position));


        if (Vector2.Distance(transform.position, target.position) < minDistanceToPlayer) return;

        // Movement
        if (invertedPath.Count > 0 && pathIndex < invertedPath.Count)
        {
            followPath();
        }


        searchColdownElapsed += Time.deltaTime;
        if(searchColdownElapsed > searchColdown)
        {

            visited.Clear();
            unVisited.Clear();
            invertedPath.Clear();

            calculatePath();

            pathIndex = invertedPath.Count - 1;
            searchColdownElapsed = 0;
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
        while (temp < 9999)
        {

            currentNode = findNextNeightbor(currentNode);

            if (Vector2.Distance(currentNode.position, target.position) < minDistanceToPlayer)
            {
                break;
            }

            temp++;

        }

        if(temp > 999)
        {

        }

        temp = 0;


        invertedPath.Add(currentNode.position);
        Node nextNode = currentNode.parent;

        while (nextNode != null && temp < 999)
        {
            invertedPath.Add(nextNode.position);
            nextNode = nextNode.parent;
            temp++;
        }
    }



    private bool obstacleCheck( Vector2 toTest)
    {

        Collider2D collider = Physics2D.OverlapCircle(toTest, 0.2f, obstacles);

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
            result = unVisited[rand];
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

   



    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        foreach (Node n in unVisited) {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(n.position, 0.2f);
        }

        foreach (Node n in visited)
        {
            // Draw a yellow sphere at the transform's position
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
        }


    }

}
