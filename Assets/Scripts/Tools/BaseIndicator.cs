using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Show the base on screen
public class BaseIndicator : MonoBehaviour
{
    public GameObject baseObject;
    public float indicatorOffset = 50f;
    public Image indicatorImage;

    private RectTransform indicatorRectTransform;
    private Vector3 basePosition;
    private Camera mainCamera;

    private void Start()
    {
        indicatorRectTransform = indicatorImage.GetComponent<RectTransform>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        basePosition = baseObject.transform.position;

        Vector3 screenPosition = mainCamera.WorldToScreenPoint(basePosition);

        float xMin = indicatorRectTransform.rect.width / 2f;
        float xMax = Screen.width - indicatorRectTransform.rect.width / 2f;
        float yMin = indicatorRectTransform.rect.height / 2f;
        float yMax = Screen.height - indicatorRectTransform.rect.height / 2f;
        screenPosition.x = Mathf.Clamp(screenPosition.x, xMin - 40f, xMax + 40f);
        screenPosition.y = Mathf.Clamp(screenPosition.y, yMin -95f, yMax - 10f);

        indicatorRectTransform.position = screenPosition + Vector3.up * indicatorOffset;
    }
}
