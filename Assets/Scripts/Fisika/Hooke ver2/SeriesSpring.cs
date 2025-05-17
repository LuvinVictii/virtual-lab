using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeriesSpring : MonoBehaviour
{
    public Transform anchor;
    public Transform joint; // Titik sambungan antara kedua pegas
    public Transform beban;

    private Transform origin1;
    private Transform origin2;

    public LineRenderer lr1, lr2;

    public Slider forceSlider;
    public Slider kSlider1;
    public Slider kSlider2;
    public TMP_Text forceText;
    public TMP_Text k1Text;
    public TMP_Text k2Text;

    public int segmentCount = 30;
    public float waveAmplitude = 0.2f;
    public float waveFrequency = 5f;
    public float lineWidth = 0.08f;

    public float straightTopLength = 0.2f;
    public float straightBottomLength = 0.1f;
    public float xAdjustment = 1.5f;

    private float x1, x2;

    void Start()
    {
        origin1 = new GameObject("Origin").transform;
        origin1.position = joint.position;
        origin2 = new GameObject("Origin").transform;
        origin2.position = beban.position;

        lr1.startWidth = lineWidth;
        lr1.endWidth = lineWidth;
        lr2.startWidth = lineWidth;
        lr2.endWidth = lineWidth;
    }

    void Update()
    {
        float F = forceSlider.value;
        float k1 = Mathf.Max(kSlider1.value, 0.01f);
        float k2 = Mathf.Max(kSlider2.value, 0.01f);

        // Perhitungan gaya pegas seri
        x1 = F / k1;
        x2 = F / k2;
        float xTotal = x1 + x2;
        // xTotal = F / (1 / (1f / k1 + 1f / k2));
        // float kTotal = (k1 * k2) / (k1 + k2);
        // float xTotal = F / kTotal;

        Vector2 mid = origin1.position + Vector3.right * x1 * xAdjustment;
        Vector2 end = origin2.position + Vector3.right * xTotal * xAdjustment;

        joint.position = mid;
        beban.position = end;

        DrawPegas(anchor.position, mid, lr1, k1, kSlider1.minValue, kSlider1.maxValue);
        DrawPegas(mid, end, lr2, k2, kSlider2.minValue, kSlider2.maxValue);

        // Update UI
        if (forceText != null) forceText.text = $"{F:0.00} N";
        if (k1Text != null) k1Text.text = $"{k1:0.00} N/m";
        if (k2Text != null) k2Text.text = $"{k2:0.00} N/m";
    }

    void DrawPegas(Vector2 start, Vector2 end, LineRenderer lr, float k, float kMin, float kMax)
    {
        end.y = start.y;
        Vector2 dir = (end - start).normalized;
        Vector2 perp = new Vector2(-dir.y, dir.x);

        Vector2 springStart = start + dir * straightTopLength;
        Vector2 springEnd = end - dir * straightBottomLength;

        int totalPoints = 1 + segmentCount + 2;
        lr.positionCount = totalPoints;

        int idx = 0;
        lr.SetPosition(idx++, start);

        for (int i = 0; i <= segmentCount; i++)
        {
            float t = (float)i / segmentCount;
            Vector2 basePos = Vector2.Lerp(springStart, springEnd, t);
            float offset = Mathf.Sin(t * waveFrequency * Mathf.PI * 2) * waveAmplitude;
            lr.SetPosition(idx++, basePos + perp * offset);
        }

        lr.SetPosition(idx, end);

        // Update ketebalan sesuai k
        float kRange = kMax - kMin;
        float newWidth = lineWidth + (k - kMin) / kRange * lineWidth / 2;
        lr.startWidth = newWidth;
        lr.endWidth = newWidth;
    }

    public float GetX()
    {
        // if (idx == 1) return x1;
        // else if (idx == 2) return x2;
        // else return x1 + x2;
        return x1 + x2;
    }

    public float GetF() => forceSlider.value;
}
