using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

// Main Menu
public class MainMenu : MonoBehaviour
{
    public void SinglePlayer()
    {
        SceneManager.LoadScene("GameMain");
    }

    public void MultiplayerPlayer()
    {
        SceneManager.LoadScene("MultiplayerLobby");
    }
}
