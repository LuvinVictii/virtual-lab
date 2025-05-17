using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderTickGenerator : MonoBehaviour
{
    public Slider slider;              // Referensi ke slider
    public RectTransform tickParent;  // Tempat spawn garis (biasanya UI Panel di bawah slider)
    public RectTransform SliderHead;
    public GameObject tickPrefab;     // Prefab garis kecil (pakai Image RectTransform kecil)
    public GameObject tickPrefabHigh; // Prefab garis kecil untuk high repetition
    public float step = 10f;          // Langkah kelipatan
    public int highRepetition = 5;

    void Start()
    {
        GenerateTicks();
    }

    void GenerateTicks()
    {
        if (slider == null || tickParent == null || tickPrefab == null)
        {
            Debug.LogWarning("Ada referensi yang belum diisi.");
            return;
        }

        float range = slider.maxValue - slider.minValue;
        int tickCount = Mathf.FloorToInt(range / step) + 1;

        float parentWidth = tickParent.rect.width;

        for (int i = 0; i < tickCount; i++)
        {
            float normalizedPos = i / (float)(tickCount - 1);
            float xPos = normalizedPos * parentWidth - (parentWidth / 2f);
            GameObject tick = null;
            if (i % highRepetition == 0)
            {
                tick = Instantiate(tickPrefabHigh, tickParent);
                TMP_Text tickText = tick.GetComponentInChildren<TMP_Text>();
                tickText.text = (slider.minValue + i * step).ToString("F0"); // Format angka sesuai kebutuhan
                // Instantiate()
            }
            else
            {
                tick = Instantiate(tickPrefab, tickParent);
            }
            // GameObject tick = (i % highRepetition == 0) ? Instantiate(tickPrefabHigh, tickParent) : Instantiate(tickPrefab, tickParent); // Pilih prefab berdasarkan repetisi tinggi
            // GameObject tick = Instantiate(tickPrefab, tickParent);
            RectTransform rt = tick.GetComponent<RectTransform>();

            rt.anchoredPosition = new Vector2(xPos, 0f);
        }
        SliderHead.SetParent(tickParent.parent);
        SliderHead.SetParent(tickParent);
    }
}
