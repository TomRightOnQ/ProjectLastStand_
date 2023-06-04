using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SliderBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private Slider slider;

    private void Update()
    {
        valueText.text = Mathf.Floor(slider.value * 100).ToString();
    }
}
