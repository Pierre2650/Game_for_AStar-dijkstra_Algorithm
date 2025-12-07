using UnityEngine;

public class DestroyableObject_Controller : MonoBehaviour
{
    public int fireLayer = 0;
    private GameObject Bonus;
    public GameObject[] BonusesPrefab;
    private bool hasBonus = false;
    public MapGenerator generator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        generator = transform.parent.parent.GetComponent<MapGenerator>();

        int rand = Random.Range(0, 101);

        if(rand < 25)
        {
            hasBonus = true;
            int rand2 = Random.Range(0, BonusesPrefab.Length);
            Bonus = BonusesPrefab[rand2];
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == fireLayer)
        {
            Destroy(collision.gameObject);

            if(hasBonus)
            { Instantiate(Bonus, transform.position, Quaternion.identity, transform.parent); }

            generator.Objects.Remove(gameObject);
            Destroy(gameObject);
        }
    }

    


}
