using UnityEngine;
using TMPro;

public class Version : MonoBehaviour
{
    [SerializeField] GameSettings gameSettings;
    private TextMeshProUGUI text;
    private void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = gameSettings.GameVersion;
    }
}
