using System.Collections;
using UnityEngine;

public class MapEndController : MonoBehaviour
{
    public MapGenerator generator;
    public Transform cameraT;
    public GameObject player;
    public SpawnerController spawnerController;
    public float cameraMouvSpeed;

    private Vector3 cameraStartPos;
    private Vector3 playerStartPos;
    private bool cinematique;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraStartPos = cameraT.position;
        playerStartPos = new Vector3(-10.71f,6.05f);


    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void newMap()
    {
        StartCoroutine(moveCameraToNewMap());
        spawnerController.resetAll();
        spawnerController.raiseDifficulty();
        player.GetComponent<PlayerController>().restrained = true;
        player.GetComponent<Rigidbody2D>().linearVelocity = Vector3.zero;
        StartCoroutine(movePlayer());
    }

    private IEnumerator moveCameraToNewMap()
    {
        Vector3 start = cameraT.position;
        Vector3 end = new Vector3(27, -15.45f, cameraT.position.z);

        float elapsed = 0, percent = 0;

        while(elapsed < cameraMouvSpeed)
        {

            percent = elapsed / cameraMouvSpeed;

            cameraT.position = Vector3.Lerp(start, end, percent);    

            elapsed += Time.deltaTime;
            yield return null;
        }

        cameraT.position = end;
        generator.destroyAll();

        yield return new WaitForSeconds(0.5f);
        backInPlace();
            

    }


    private void backInPlace()
    {
        player.transform.position = playerStartPos;
        player.GetComponent<PlayerController>().restrained = false;
        cameraT.position = cameraStartPos;
        generator.generateMap();
        spawnerController.stopSpawn = false;
    }

    private IEnumerator movePlayer()
    {

        Vector3 start = player.transform.position;
        Vector3 end = new Vector3(16.32f, -9.41f, player.transform.position.z);

        float elapsed = 0, percent = 0;

        while (elapsed < cameraMouvSpeed)
        {

            percent = elapsed / cameraMouvSpeed;

            player.transform.position = Vector3.Lerp(start, end, percent);

            elapsed += Time.deltaTime;
            yield return null;
        }

        player.transform.position = end;
        generator.destroyAll();
        cinematique = false;


    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>() != null && !cinematique)
        {
            cinematique = true;
            newMap();
            spawnerController.resetAll();
        }
    }
}
