using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

// Allows binding keys to certain buttons
public class keyBinding : MonoBehaviour
{
    // Selected key
    [SerializeField] string key;
    [SerializeField] Button button;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(key))
        {
            // Trigger the current button
            button.onClick.Invoke();
        }
    }
}
