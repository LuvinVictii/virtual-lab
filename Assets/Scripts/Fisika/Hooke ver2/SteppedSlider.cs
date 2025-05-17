using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Slider))]
public class SteppedSlider : MonoBehaviour
{
    public float step = 10f;               // Langkah kelipatan, misal 10
    public TMP_Text valueText;             // Untuk menampilkan nilai slider

    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();

        // Pastikan nilai min & max cocok dengan step
        float range = slider.maxValue - slider.minValue;
        if (range % step != 0)
        {
            Debug.LogWarning("Range slider tidak habis dibagi step.");
        }

        slider.wholeNumbers = false; // kita pakai float, tapi snapped manual
        slider.onValueChanged.AddListener(OnSliderChanged);

        // Set awal
        SnapToStep(slider.value);
    }

    void OnSliderChanged(float value)
    {
        SnapToStep(value);
    }

    void SnapToStep(float rawValue)
    {
        float snapped = Mathf.Round(rawValue / step) * step;

        // Cegah loop: set tanpa trigger event lagi
        slider.SetValueWithoutNotify(snapped);

        if (valueText != null)
        {
            valueText.text = $"{snapped:0}";
        }
    }
}
