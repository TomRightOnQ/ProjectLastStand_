using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

// Main Menu
public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_InputField nickName;
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SinglePlayer()
    {
        GameSettings.Instance.SetName(nickName.text);
        SceneManager.LoadScene("GameMain");
    }

    public void MultiplayerPlayer()
    {
        GameSettings.Instance.SetName(nickName.text);
        SceneManager.LoadScene("MultiplayerLobby");
    }

    public void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
