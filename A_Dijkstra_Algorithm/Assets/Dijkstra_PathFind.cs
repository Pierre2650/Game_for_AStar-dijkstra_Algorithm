using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dijkstra_PathFind : MonoBehaviour
{
    public Transform target;
    public float speed;
    public LayerMask obstacles;

    public class Node
    {
        
        public Vector2 position;
        public Node parent = null;
        public int distance;

        public Node(Vector2 position, int distance)
        {
            this.position = position;
            this.distance = distance;
        }

        public Node(Vector2 position, int distance, Node parent)
        {
            this.position = position;
            this.distance = distance;
            this.parent = parent;
        }
    }

    // on commence par 4 voicin
    private List<Node> unVisited = new List<Node>();
    private List<Node> visited = new List<Node>();
    private List<Vector2> invertedPath = new List<Vector2>();
    private float searchColdownElapsed = 0;

    private Node currentNode;

    private Vector2 horizontal = new Vector2(0.5f, -0.25f), vertical = new Vector2(0.5f, 0.25f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentNode = new Node(transform.position, 0);
        visited.Add(currentNode);
        calculatePath();
        StartCoroutine(followPath());
    }

    private void Update()
    {
        
        searchColdownElapsed += Time.deltaTime;
        if(searchColdownElapsed > 2)
        {
            StopAllCoroutines();
            visited.Clear();
            unVisited.Clear();
            invertedPath.Clear();

            currentNode = new Node(transform.position, 0);
            visited.Add(currentNode);
            calculatePath();
            StartCoroutine(followPath());

            searchColdownElapsed = 0;
        }
    }

    private void calculatePath()
    {
        int temp = 0;
        while (temp < 9999)
        {

            currentNode = findNextNeightbor(currentNode);

            if (Vector2.Distance(currentNode.position, target.position) < 0.1)
            {
                break;
            }

            temp++;

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
        List<Vector2> neighbors = new List<Vector2>();
        // we go clockwise
        Vector2 up = currentNode.position + vertical;
        neighbors.Add(up);
        Vector2 right = currentNode.position + horizontal;
        neighbors.Add(right);
        Vector2 down = currentNode.position - vertical;
        neighbors.Add(down);
        Vector2 left = currentNode.position - horizontal;
        neighbors.Add(left);

        int newDistance = currentNode.distance + 1;

        foreach (Vector2 v in neighbors.ToList()) {
            if (obstacleCheck(v))
            {
                continue;
            }

            if (!unVisited.Exists(x => x.position == v))
            {
                unVisited.Add(new Node(v, newDistance, currentNode)); // up vector
            }
        }


        int min = newDistance;
        Node result = null;
        foreach (Node n in unVisited) {

            
            if(n.distance < min) 
            {
                min = newDistance;
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

    private IEnumerator followPath()
    {
        float elapsedT = 0;

        int i = invertedPath.Count-1;

        while (i>0)
        {
            Vector2 start = transform.position;

            while (elapsedT < 1)
            {

                transform.position = Vector2.Lerp(start, invertedPath[i], elapsedT);

                elapsedT += Time.deltaTime * speed;
               yield return null;
            }

            transform.position = invertedPath[i];
            elapsedT = 0;
            i--;
            //yield return new WaitForSeconds(Time.deltaTime);
        }
    }

    private void OnDrawGizmos()
    {
        // Draw a yellow sphere at the transform's position
        foreach (Node n in unVisited) {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(n.position, 0.2f);
        }

        foreach (Node n in visited)
        {
            // Draw a yellow sphere at the transform's position
            Gizmos.color = Color.green;
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


        if (target != null)
        {
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(target.position, 0.2f);
        }

    }

}
