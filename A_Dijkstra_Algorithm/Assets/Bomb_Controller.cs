using System.Collections;
using UnityEngine;

public class Bomb_Controller : MonoBehaviour
{
    private Animator anim;

    [Header("Fire")]
    public GameObject firePrefab;
    public bool spawnFire = true;
    public int flamesLength = 1;
    private int currentLength = 0;

    private float spawnIntervalElapsed = 0;
    private float spawnIntervalDur = 0.02f;
    private Vector2 horizontal = new Vector2(0.5f, -0.25f), vertical = new Vector2(0.5f, 0.25f);
    private Vector2 up, right, down, left;
    private bool continueUp = true, continueRight = true, continueDown = true, continueLeft = true;

    [Header("Explosion")]
    public float timetoExplode = 3f;
    private float elapsedToExplode = 0;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        up = right = down =  left =  new Vector2(transform.position.x, transform.position.y + 0.2f);

    }

    // Update is called once per frame
    void Update()
    {

       

        if (spawnFire)
        {
            spawnFlames();

            if(currentLength == flamesLength)
            {
                spawnFire = false;
                Destroy(gameObject);
            }
        }
        else
        {
            elapsedToExplode += Time.deltaTime;
            if (elapsedToExplode > timetoExplode)
            {
                anim.SetTrigger("Explode");
                StartCoroutine(toSpawnFlames());
                elapsedToExplode = 0;
            }

        }
    }


    private IEnumerator toSpawnFlames()
    {
        yield return new WaitForSeconds(0.85f);
        spawnFire = true;
        //Destroy(gameObject);
    }
    private void spawnFlames()
    {

        spawnIntervalElapsed += Time.deltaTime;
        if (spawnIntervalElapsed > spawnIntervalDur)
        {

            up = up + vertical;
            right = right + horizontal;
            down = down - vertical;
            left = left - horizontal;
            Vector2[] directions = {up,right, down, left};

            for(int i = 0; i < directions.Length; i++)
            {
                if (!obstacleCheck(directions[i]))
                {
                    if (i == 0 && !continueUp)
                    {
                        continue;

                    } else if (i == 1 && !continueRight)
                    {
                        continue;

                    } else if (i == 2 && !continueDown)
                    {
                        continue;
                    }
                    else if (i == 3 && !continueLeft)
                    {
                        continue;
                    }

                    Instantiate(firePrefab, directions[i], transform.rotation, transform.parent);

                    
                }
                else
                {
                    switch (i)
                    {
                        case 0:
                            continueUp = false;
                            break;
                        case 1:
                            continueRight = false;
                            break;
                        case 2:
                            continueDown = false;
                            break;
                        case 3:
                            continueLeft = false;
                            break;
                    }
                }
            }



            spawnIntervalElapsed = 0f;
            currentLength++;

        }


    }

    private bool obstacleCheck(Vector2 toTest)
    {

        Collider2D collider = Physics2D.OverlapCircle(toTest, 0.2f);
        if (collider == null) { return false; }

        if(collider.GetComponent<DestroyableObject_Controller>() != null || collider.GetComponent<EnemyController>() != null || collider.GetComponent<PlayerController>() != null)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

}
