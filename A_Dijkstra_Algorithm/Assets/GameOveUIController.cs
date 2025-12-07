using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOveUIController : MonoBehaviour
{

    public GameObject uiScreen;
    public SpawnerController spawner;
    public void showUI()
    {
        uiScreen.SetActive(true);
        spawner.resetAll();

    }

    public void reStart()
    {
        SceneManager.LoadScene("PathFindingGame");
    }
}
