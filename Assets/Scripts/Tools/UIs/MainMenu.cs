using UnityEngine;
using UnityEngine.SceneManagement;

// Main Menu
public class MainMenu : MonoBehaviour
{
    public void QuitGame()
    {
        Application.Quit();
    }

    public void SinglePlayer()
    {
        SceneManager.LoadScene("GameMain");
    }

    public void MultiplayerPlayer()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }
}
