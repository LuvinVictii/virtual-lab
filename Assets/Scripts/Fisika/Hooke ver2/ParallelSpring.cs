using UnityEngine;
using UnityEngine.UI;
using TMPro;

// [RequireComponent(typeof(LineRenderer))]
public class ParallelSpring : MonoBehaviour
{
    public Transform anchor1, anchor2; // Anchor masing-masing pegas
    public Transform beban;
    private Transform origin;

    public int segmentCount = 30;
    public float waveAmplitude = 0.2f;
    public float waveFrequency = 5f;
    public float lineWidth = 0.08f;

    public float straightTopLength = 0.2f;
    public float straightBottomLength = 0.1f;
    public float xAdjustment = 1.5f;

    public Slider forceSlider;
    public Slider kSlider1;
    public Slider kSlider2;
    public TMP_Text forceText;
    public TMP_Text k1Text;
    public TMP_Text k2Text;
    // public TMP_Text xText;

    public LineRenderer lr1;
    public LineRenderer lr2;
    private float kRange;

    private float x;

    void Start()
    {
        origin = new GameObject("Origin").transform;
        origin.position = beban.position;

        lr1.startWidth = lineWidth;
        lr1.endWidth = lineWidth;
        lr2.startWidth = lineWidth;
        lr2.endWidth = lineWidth;

        kRange = kSlider1.maxValue - kSlider1.minValue;
    }

    void Update()
    {
        float F = forceSlider.value;
        float k1 = Mathf.Max(kSlider1.value, 0.01f);
        float k2 = Mathf.Max(kSlider2.value, 0.01f);
        float kTotal = k1 + k2;

        x = F / kTotal;

        Vector2 end = origin.position + Vector3.right * x * xAdjustment;
        beban.position = end;

        float newWidth = lineWidth + (k1 - kSlider1.minValue) / kRange * lineWidth / 2;
        lr1.startWidth = newWidth;
        lr1.endWidth = newWidth;

        newWidth = lineWidth + (k2 - kSlider2.minValue) / kRange * lineWidth / 2;
        lr2.startWidth = newWidth;
        lr2.endWidth = newWidth;

        DrawPegas(anchor1.position, end, lr1);
        DrawPegas(anchor2.position, end, lr2);

        // Update UI
        if (forceText != null) forceText.text = $"{F:0.00} N";
        if (k1Text != null) k1Text.text = $"{k1:0.00} N/m";
        if (k2Text != null) k2Text.text = $"{k2:0.00} N/m";
        // if (xText != null) xText.text = $"x: {x:0.00} m";
    }

    void DrawPegas(Vector2 start, Vector2 end, LineRenderer lr)
    {
        end.y = start.y; // Set y coordinate to be the same as start
        Vector2 direction = (end - start).normalized;
        Vector2 perp = new Vector2(-direction.y, direction.x);

        Vector2 springStart = start + direction * straightTopLength;
        Vector2 springEnd = end - direction * straightBottomLength;

        int totalPoints = 1 + segmentCount + 2;
        lr.positionCount = totalPoints;

        int index = 0;
        lr.SetPosition(index++, start);

        for (int i = 0; i <= segmentCount; i++)
        {
            float t = (float)i / segmentCount;
            Vector2 basePos = Vector2.Lerp(springStart, springEnd, t);
            float offset = Mathf.Sin(t * waveFrequency * Mathf.PI * 2) * waveAmplitude;
            lr.SetPosition(index++, basePos + perp * offset);
        }

        lr.SetPosition(index, end);
    }

    // public float GetX1() => x * (kSlider1.value / (kSlider1.value + kSlider2.value));
    // public float GetX2() => x * (kSlider2.value / (kSlider1.value + kSlider2.value));
    // public float GetX() => x;
    public float GetX(int idx){
        if (idx == 1)
            return x * (kSlider1.value / (kSlider1.value + kSlider2.value));
        else if (idx == 2)
            return x * (kSlider2.value / (kSlider1.value + kSlider2.value));
        else
            return x;
    }
    public float GetF(int idx){
        if (idx == 1)
            return forceSlider.value * (kSlider1.value / (kSlider1.value + kSlider2.value));
        else if (idx == 2)
            return forceSlider.value * (kSlider2.value / (kSlider1.value + kSlider2.value));
        else
            return forceSlider.value;
    }
}
