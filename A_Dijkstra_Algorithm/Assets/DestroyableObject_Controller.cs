using UnityEngine;

public class DestroyableObject_Controller : MonoBehaviour
{
    public int fireLayer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("trigger");
        if (collision.gameObject.layer == fireLayer)
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }


}
