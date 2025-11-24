using System;
using System.Collections.Generic;
using UnityEngine;

public class Dijkstra_PathFind : MonoBehaviour
{
    public Transform objective;


    public class Node
    {
        public Vector2 position;
        public float distance = float.PositiveInfinity;
        public bool visited = false;
    }

    // on commence par 4 voicin
    private int  nbVoicins = 4; //  up, down, left , right
    private List<Vector3> unVisited = new List<Vector3>();
    private Vector2 horizontal = new Vector2(0.5f, -0.25f), vertical = new Vector2(0.5f, 0.25f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        calculateNodeDistances(transform.position);

        

    }

    private void calculateNodeDistances(Vector2 node)
    {
        // we go clockwise
       // unVisited[0] = Vector2.Distance(unVisited[i], objective.position)
        unVisited[0] = node + vertical; // up vector
        unVisited[1] = node + horizontal; // right
        unVisited[2] = node - vertical; // down
        unVisited[3] = node - horizontal; // Left

        float min = float.MaxValue;
        Vector3 result = node;
        for (int i = 0; i < nbVoicins; i++)
        {
            if (Vector2.Distance(unVisited[i], objective.position) < min)
            {
                min = Vector2.Distance(unVisited[i], objective.position); 
                result = unVisited[i];
            }
            
        }

        return unVisited[i];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
