using System.Collections;
using UnityEngine;

public class indicationUIController : MonoBehaviour
{

    public GameObject indicator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(notifyAnimation());
    }

    private IEnumerator notifyAnimation()
    {
        int nbTilts = 3;
        float duration = 2.5f;
        while (nbTilts > 0)
        {


            indicator.SetActive(false);


            yield return new WaitForSeconds(duration / (3 * 2f));


            indicator.SetActive(true);

            yield return new WaitForSeconds(duration / (3 * 2f));

            nbTilts--;


        }

        indicator.SetActive(false);
    }

}
