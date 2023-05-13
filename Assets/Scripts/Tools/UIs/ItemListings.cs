using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using static WeaponConfigs;

// Item Listings
public class ItemListings : MonoBehaviourPunCallbacks
{
    // Display
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI FText;

    // Data
    [SerializeField] private int weaponIndex;
    private int level;
    private int viewID;
    [SerializeField] private long droppedId;

    public long DroppedId { get { return droppedId; } set { droppedId = value; } }
    public int WeaponIndex { get { return weaponIndex; } set { weaponIndex = value; } }

    private void Start()
    {
        transform.SetParent(GameUI.Instance.Content.transform);
        transform.localScale = Vector3.one;
    }

    // Set up an listing info
    public void SetUp()
    {
        WeaponConfig weaponData = WeaponConfigs.Instance._getWeaponConfig(weaponIndex);
        // set details
        nameText.text = weaponData._name;
        weaponIndex = weaponData.id;
    }
    public void SetHighlight(bool isHighlighted)
    {
        // Implement your highlighting logic here, such as changing color or applying visual effects
        if (isHighlighted)
        {
            FText.color = Color.yellow;
        }
        else
        {
            FText.color = Color.white;
        }
    }
}
