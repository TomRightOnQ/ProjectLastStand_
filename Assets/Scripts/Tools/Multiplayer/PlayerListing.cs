using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Realtime;

public class PlayerListing : MonoBehaviour
{
    [SerializeField] private TMP_InputField text;
    public Player Player { get; private set; }
    private bool ready = false;

    public bool Ready { get { return ready; } set { ready = value; } }

    public void SetPlayerInfo(Player p) 
    {
        Player = p;
        text.text = p.NickName;
    }
}
