using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public SpawnerController spawner;
    public Transform playerT;
    public int fireLayer = 0;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null)
        {
            player.LifePoints = 0;

        }


        if (collision.gameObject.layer == fireLayer)
        {
            Destroy(collision.gameObject);
            spawner.nbEnemies--;
            spawner.enemiesList.Remove(gameObject);
            Destroy(gameObject);
        }


    }


}
