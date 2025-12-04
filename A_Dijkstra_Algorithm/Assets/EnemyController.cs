using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public SpawnerController spawner;
    public Transform playerT;
    public int fireLayer = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.layer == fireLayer)
        {
            Destroy(collision.gameObject);
            spawner.nbEnemies--;
            Destroy(gameObject);
        }


    }


}
