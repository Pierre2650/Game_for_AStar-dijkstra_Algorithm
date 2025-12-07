using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public SpawnerController spawner;
    public Transform playerT;
    public GameObject extraLife;
    public int fireLayer = 0;
    private bool hasBonus = false;

    private void Start()
    {
        int rand = Random.Range(0, 101);

        if (rand < 10)
        {
            hasBonus = true;
        }
    }
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

            if (hasBonus)
            { Instantiate(extraLife, transform.position, Quaternion.identity, transform.parent); }
        }


    }


}
