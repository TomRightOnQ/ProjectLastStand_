using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

// Generate random custom property to a player
public class RandomProperty : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable properties = new ExitGames.Client.Photon.Hashtable();
    [SerializeField] private TextMeshProUGUI text;
    private void SetCustomNumber() 
    {
        System.Random rand = new System.Random();
        int res = rand.Next(0, 9999);
        text.text = res.ToString();

        properties["RandomNumber"] = res;
        PhotonNetwork.LocalPlayer.CustomProperties = properties;
    }

    public void OnClick_Button()
    {
        SetCustomNumber();
    }

}
