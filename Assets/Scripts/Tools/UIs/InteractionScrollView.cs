using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using static WeaponConfigs;

public class InteractionScrollView : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollView;
    [SerializeField] private RectTransform content;
    private int selectedItemIndex = 0;

    void Start()
    {
        scrollView = this.GetComponent<ScrollRect>();
    }

    void Update()
    {
        // Scroll to the next item when scrolling down
        if (Input.mouseScrollDelta.y < 0)
        {
            selectedItemIndex++;
            UpdateHighlightedItem();
        }

        // Scroll to the previous item when scrolling up
        if (Input.mouseScrollDelta.y > 0)
        {
            selectedItemIndex--;
            UpdateHighlightedItem();
        }

        // Select the highlighted item when the F key is pressed
        if (Input.GetKeyUp(KeyCode.F))
        {
            SelectHighlightedItem();
        }

        // Highlight the first item if no items are currently highlighted
        if (content.transform.childCount > 0 && selectedItemIndex < 0)
        {
            selectedItemIndex = 0;
            UpdateHighlightedItem();
        }
    }

    void UpdateHighlightedItem()
    {
        // Ensure the selected item index stays within the valid range
        selectedItemIndex = Mathf.Clamp(selectedItemIndex, 0, content.transform.childCount - 1);

        // Update the highlighting of the items
        for (int i = 0; i < content.transform.childCount; i++)
        {
            ItemListings item = content.transform.GetChild(i).GetComponent<ItemListings>();
            bool isHighlighted = (i == selectedItemIndex);
            item.SetHighlight(isHighlighted);
        }
    }

    void SelectHighlightedItem()
    {
        // Perform the selection action based on the selected item index
        if (selectedItemIndex >= 0 && selectedItemIndex < content.transform.childCount)
        {
            ItemListings listing = content.transform.GetChild(selectedItemIndex).GetComponent<ItemListings>();
            Debug.Log("Selected item: " + listing.name);
            // Get Weapons
            WeaponConfig weaponData = WeaponConfigs.Instance._getWeaponConfig(listing.WeaponIndex);
            // Show the WeaponChpice page
            WeaponChoice.Instance.ShowPanel();
            WeaponChoice.Instance.SetWeaponInfo(2, weaponData);
            WeaponChoice.Instance.SetID(listing.DroppedId);
        }
    }
}
