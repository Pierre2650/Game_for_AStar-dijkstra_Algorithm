using UnityEngine;

public class BombFire_Controller : MonoBehaviour
{
    private float elapsedT = 0;
    private float duration = 0.8f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        elapsedT += Time.deltaTime;
        if (elapsedT > duration) {
            Destroy(gameObject);
        }
        
    }
}
