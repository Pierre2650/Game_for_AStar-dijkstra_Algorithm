using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LifesUIController : MonoBehaviour
{
    public Image[] imgs;
    public Sprite imgSprite;
    private int imageIndex = 2;

    private GameOveUIController gameOverController;

    private void Start()
    {
        gameOverController = GetComponent<GameOveUIController>();
    }



    private IEnumerator notifyAnimation()
    {
        int nbTilts = 3;
        float duration = 1;
        while (nbTilts > 0)
        {

            foreach (Image image in imgs)
            {
                image.enabled = false;
            }

            yield return new WaitForSeconds(duration / (3 * 2f));

            foreach (Image image in imgs)
            {
                image.enabled = true;
            }

            yield return new WaitForSeconds(duration / (3 * 2f));

            nbTilts--;


        }

    }
    public void lifeLost()
    {
        if (imageIndex < 0) { return; }

        imgs[imageIndex].color = new Color(imgs[imageIndex].color.r, imgs[imageIndex].color.g, imgs[imageIndex].color.b, 0);
        imageIndex--;

        if (imageIndex < 0)
        {
            gameOverController.showUI();
        }
        else
        {
            StartCoroutine(notifyAnimation());
        }

    }

    public void lifeGained()
    {
        if (imageIndex == 2) { return; }

        imgs[imageIndex].color = new Color(imgs[imageIndex].color.r, imgs[imageIndex].color.g, imgs[imageIndex].color.b, 1);
        imageIndex++;


        StartCoroutine(notifyAnimation());

    }
}
