using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Numbers jumping out when taking damage
public class DamageNumber : MonoBehaviour
{
    [SerializeField] private float flyDuration = 1.5f;
    [SerializeField] private float fadeDuration = 1.5f;
    [SerializeField] private float flySpeed = 0.4f;
    [SerializeField] private TextMeshProUGUI _text;
    private Vector3 initialPosition;

    public void Init(float damage, Vector3 monsterPosition)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(monsterPosition);
        screenPos.z = 0f;
        _text.transform.position = screenPos;

        _text.text = Mathf.Abs(damage).ToString();
        initialPosition = _text.transform.position;
        StartCoroutine(Fly());
        StartCoroutine(FadeOut());
    }

    private IEnumerator Fly()
    {
        float elapsed = 0f;
        Vector3 targetPosition = initialPosition + Vector3.up * flySpeed;

        while (elapsed < flyDuration)
        {
            elapsed += Time.deltaTime;
            _text.transform.position = Vector3.Lerp(initialPosition, targetPosition, elapsed / flyDuration);
            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float elapsed = 0f;
        Color startColor = _text.color;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / fadeDuration;
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(1f, 0f, t));
            _text.color = newColor;
            yield return null;
        }
        Destroy(gameObject);
    }
}
