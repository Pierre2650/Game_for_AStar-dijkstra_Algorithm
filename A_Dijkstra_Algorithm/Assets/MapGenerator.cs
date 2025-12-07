using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject horizontalPrefab;
    public GameObject verticalPrefab;
    public GameObject[] BonusesPrefab;
    public Vector2 verticalStart;
    public Vector2[] verticalEndStart;
    public int vLength;
    public Vector2 horizontalStart;
    public Vector2[] horizontalEndStart;
    public int hLength;

    private Vector2 vVertical = new Vector2(2, 1);
    private Vector2 vHorizontal = new Vector2(1.49f, -0.75f);

    private Vector2 hHorizontal = new Vector2(1.49f, -0.74f);
    private Vector2 hVertical = new Vector2(2f, 0.998f);

    public int spawnProbability;

    public List<GameObject> Objects = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generateMap();

    }

    public void generateMap()
    {
        Debug.Log("generateMap Called");
        generateCenterBonus();


        generateHorizontalDestroyables();

        generateVerticalDestroyables();


        //End -----------------------------------

        generateVerticalEndDestroyables();

        generateHorizontalEndDestroyables();

    }

    private void generateCenterBonus()
    {
        int rand = rand = Random.Range(0, BonusesPrefab.Length);

        Instantiate(BonusesPrefab[rand], new Vector3(0, 0.45f, 0), Quaternion.identity);



    }
    private void generateVerticalDestroyables()
    {
        int rand = 0;
        Vector2 verticalBoxesHDisplacement = verticalStart;
        for (int i = 0; i < hLength; i++)
        {
            Vector2 verticalBoxesVDisplacement = verticalBoxesHDisplacement;

            for (int j = 0; j < vLength; j++)
            {
                rand = Random.Range(0, 101);

                if (rand > spawnProbability)
                {
                    verticalBoxesVDisplacement += vVertical;
                    continue;
                }

                if ((i == 0 || i == hLength - 1) && j == vLength - 1)
                {
                    continue;
                }


                GameObject newObj = Instantiate(verticalPrefab, verticalBoxesVDisplacement, Quaternion.identity, transform);
                Objects.Add(newObj);

                verticalBoxesVDisplacement += vVertical;
            }

            verticalBoxesHDisplacement += vHorizontal;

        }

    }
    private void generateVerticalEndDestroyables()
    {
        int rand = 0;

        Vector2 verticalBoxesVDisplacement = verticalEndStart[0];
        for (int j = 0; j < 5; j++)
        {
            rand = Random.Range(0, 101);

            if (rand > 80)
            {
                verticalBoxesVDisplacement += vVertical;
                continue;

            }

            GameObject newObj = Instantiate(verticalPrefab, verticalBoxesVDisplacement, Quaternion.identity, transform);
            Objects.Add(newObj);

            verticalBoxesVDisplacement += vVertical;
        }

        verticalBoxesVDisplacement = verticalEndStart[1];
        for (int j = 0; j < 3; j++)
        {

            if (rand > 80)
            {
                verticalBoxesVDisplacement += vVertical;
                continue;

            }

            GameObject newObj = Instantiate(verticalPrefab, verticalBoxesVDisplacement, Quaternion.identity, transform);
            Objects.Add(newObj);

            verticalBoxesVDisplacement += vVertical;
        }

        verticalBoxesVDisplacement = verticalEndStart[2];
        for (int j = 0; j < 3; j++)
        {

            if (rand > 80)
            {
                verticalBoxesVDisplacement += vVertical;
                continue;

            }

            GameObject newObj = Instantiate(verticalPrefab, verticalBoxesVDisplacement, Quaternion.identity, transform);
            Objects.Add(newObj);

            verticalBoxesVDisplacement += vVertical;
        }

    }

   

    private void generateHorizontalDestroyables() {
        int rand = 0;
        Vector2 horrizontalBoxesVDisplacement = horizontalStart;
        for (int j = 0; j < vLength; j++)
        {
            Vector2 horrizontalBoxesHDisplacement = horrizontalBoxesVDisplacement;

            for (int i = 0; i < hLength; i++)
            {
                rand = Random.Range(0, 101);

                if (rand > spawnProbability)
                {
                    horrizontalBoxesHDisplacement += hHorizontal;
                    continue;
                }

                if (i == hLength - 1 && j == vLength - 1)
                {
                    continue;
                }

                GameObject newObj = Instantiate(horizontalPrefab, horrizontalBoxesHDisplacement, Quaternion.identity, transform);
                Objects.Add(newObj);

                horrizontalBoxesHDisplacement += hHorizontal;
            }

            horrizontalBoxesVDisplacement += hVertical;

        }
    }

    private void generateHorizontalEndDestroyables()
    {
        int rand = 0;
        Vector2 horrizontalBoxesHDisplacement = horizontalEndStart[0];
        for (int i = 0; i < 5; i++)
        {
            rand = Random.Range(0, 101);

            if (rand > spawnProbability)
            {
                horrizontalBoxesHDisplacement += hHorizontal;
                continue;
            }

            GameObject newObj = Instantiate(horizontalPrefab, horrizontalBoxesHDisplacement, Quaternion.identity, transform);
            Objects.Add(newObj);

            horrizontalBoxesHDisplacement += hHorizontal;
        }

        horrizontalBoxesHDisplacement = horizontalEndStart[1];
        for (int i = 0; i < hLength; i++)
        {
            rand = Random.Range(0, 101);

            if (rand > spawnProbability)
            {
                horrizontalBoxesHDisplacement += hHorizontal;
                continue;
            }

            GameObject newObj = Instantiate(horizontalPrefab, horrizontalBoxesHDisplacement, Quaternion.identity, transform);
            Objects.Add(newObj);

            horrizontalBoxesHDisplacement += hHorizontal;
        }

    }

    public void destroyAll()
    {
        foreach (GameObject obj in Objects.ToList())
        {
            Destroy(obj);
        }

        Objects.Clear();
    }
    
}
