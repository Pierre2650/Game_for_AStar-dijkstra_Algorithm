using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public Transform player;
    public GameObject DijkstraEnemy;
    public GameObject AstarEnemy;

    public GameObject[] SpawnSites;

    public float spawnRate;
    private float spawnElapsed;

    public float enemiesSpeed = 1;
    public int nbEnemies = 0;
    public int nbDijkstra = 0;
    public int maxNbEnemies = 4;
    public bool stopSpawn;
    public List<GameObject> enemiesList = new List<GameObject>();

    private int nbStages = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       spawnElapsed = spawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!stopSpawn && nbEnemies < maxNbEnemies)
        {

            spawnElapsed += Time.deltaTime;
            if (spawnElapsed > spawnRate) {
                GameObject site = getRandSite();

                int randEnemy = Random.Range(0, 2);
                GameObject temp;
                if (randEnemy == 0 && nbDijkstra < 3) {
                    temp = Instantiate(DijkstraEnemy,site.transform.position, Quaternion.identity, site.transform);
                    enemiesList.Add(temp);
                    temp.GetComponent<EnemyController>().spawner = this;
                    temp.GetComponent<EnemyController>().playerT = player;
                    temp.GetComponent<Dijkstra_PathFind>().speed = enemiesSpeed;
                    nbDijkstra++;
                }
                else
                {
                    temp = Instantiate(AstarEnemy, site.transform.position, Quaternion.identity, site.transform);
                    enemiesList.Add(temp);
                    temp.GetComponent<EnemyController>().spawner = this;
                    temp.GetComponent<EnemyController>().playerT = player;
                    temp.GetComponent<Astar>().speed = enemiesSpeed;
                }

                nbEnemies++;
                spawnElapsed = 0;
            }

        }

    }

    private GameObject getRandSite()
    {
        int rand = Random.Range(0, SpawnSites.Length);
        return SpawnSites[rand];
    }

    public void raiseDifficulty()
    {
        int rand = Random.Range(0, 3);

        if (rand == 0) {
            if (spawnRate > 4)
            {
                spawnRate--;
            }
        }
        if (rand == 1)
        {

            if (maxNbEnemies < 8)
            {
                maxNbEnemies++;
            }

        }
        else
        {
            if (enemiesSpeed < 2.5f)
            {
                enemiesSpeed += 0.25f;
            }

        }

    }
    public void resetAll()
    {
        foreach (GameObject en in enemiesList.ToList()) {
            enemiesList.Remove(en);
            Destroy(en);
        }

        enemiesList.Clear();
        nbEnemies = 0;
        spawnElapsed = 0;
        stopSpawn = true;
    }
}
