using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public Transform player;
    public GameObject DijkstraEnemy;
    public GameObject AstarEnemy;

    public GameObject[] SpawnSites;

    public float spawnRate;
    private float spawnElapsed;

    public int nbEnemies = 0;
    public int nbDijkstra = 0;
    public int maxNbEnemies = 4;
    private bool stopSpawn;
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
                if (randEnemy == 0 && nbDijkstra < 2) {
                    temp = Instantiate(DijkstraEnemy,site.transform.position, Quaternion.identity, site.transform);
                    temp.GetComponent<EnemyController>().spawner = this;
                    temp.GetComponent<EnemyController>().playerT = player;
                    nbDijkstra++;
                }
                else
                {
                    temp = Instantiate(AstarEnemy, site.transform.position, Quaternion.identity, site.transform);
                    temp.GetComponent<EnemyController>().spawner = this;
                    temp.GetComponent<EnemyController>().playerT = player;
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
}
