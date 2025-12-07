using TMPro;
using UnityEngine;

public class BombsUiController : MonoBehaviour
{
    public TextMeshProUGUI tmpText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void updateNbBombs(string nb)
    {
        tmpText.text = nb;
    }
}
