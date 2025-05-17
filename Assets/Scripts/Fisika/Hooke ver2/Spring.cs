using UnityEngine;
using UnityEngine.UI; // Untuk Slider
using TMPro; // Untuk TextMeshPro

[RequireComponent(typeof(LineRenderer))]
public class Spring : MonoBehaviour
{
    public Transform anchor;     
    public Transform beban;
    private Transform origin;
    public Transform capit;
    private Vector3 scale; // Untuk menyimpan skala pegas
    private float capitOriginalLength; // Panjang pegas
    public int segmentCount = 50;
    public float waveAmplitude = 0.2f;
    public float waveFrequency = 5f;
    public float lineWidth = 0.1f;
    private float kRange;

    public float straightTopLength = 0.2f;     // Garis lurus atas
    public float straightBottomLength = 0.1f;  // Garis lurus sebelum hook
    public float xAdjustment = 1.5f; // Untuk mengatur panjang pegas
    private float x;
    private bool isEnabled = true; // Untuk mengaktifkan atau menonaktifkan pegas

    public Slider forceSlider; // Slider untuk gaya
    public Slider kSlider;     // Slider untuk konstanta pegas
    public TMP_Text forceText; // Text untuk gaya
    public TMP_Text kText;     // Text untuk konstanta pegas
    public TMP_Text xText;     // Text untuk panjang pegas

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
        origin = new GameObject("Origin").transform; // Buat objek kosong untuk origin
        origin.position = beban.position;
        kRange = kSlider.maxValue - kSlider.minValue;
        // capitOriginalLength = capit.localScale.x; // Ambil panjang awal pegas
        // scale = new Vector3(capitOriginalLength, capitOriginalLength, capitOriginalLength);
    }

    void Update()
    {
        float F = forceSlider.value;
        float k = kSlider.value;
        x = 0f; // Panjang pegas
        if (k != 0) x = F / k;

        // Update posisi beban
        Vector2 start = anchor.position;
        Vector2 end = origin.position + Vector3.right * x * xAdjustment;
        beban.position = end;

        float newWidth = lineWidth + (k - kSlider.minValue) / kRange * lineWidth / 2;
        lr.startWidth = newWidth;
        lr.endWidth = newWidth;

        // float capitLength = capitOriginalLength - x*xAdjustment; // Panjang pegas
        // scale.x = Mathf.Max(capitLength, 0.1f); // Pastikan panjang pegas tidak negatif
        // capit.localScale = scale;

        // Vector3 pos = capit.localPosition;
        // float offset = (capitOriginalLength - capitLength) * 0.5f; // geser separuh dari selisih
        // pos.x += offset;
        // capit.localPosition = pos;

        DrawPegas(start, end);

        // Update UI
        if (forceText != null) forceText.text = $"{F:0.00} N";
        if (kText != null) kText.text = $"{k:0.00} N/m";
        // if (xText != null) xText.text = $"x: {x:0.00} m";
    }

    void DrawPegas(Vector2 start, Vector2 end)
    {
        Vector2 direction = (end - start).normalized;
        float totalLength = Vector2.Distance(start, end);
        Vector2 perp = new Vector2(-direction.y, direction.x); 

        Vector2 straightTopEnd = start + direction * straightTopLength;
        Vector2 springStart = straightTopEnd;
        Vector2 springEnd = end - direction * straightBottomLength;
        Vector2 straightBottomEnd = springEnd + direction * straightBottomLength;

        int totalPoints = 1 + segmentCount + 2; // 1 untuk atas, n untuk spring, 1 untuk bawah
        lr.positionCount = totalPoints;

        int index = 0;

        // 1. Garis lurus atas
        lr.SetPosition(index++, start);

        // 2. Pegas sinusoidal
        for (int i = 0; i <= segmentCount; i++)
        {
            float t = (float)i / segmentCount;
            Vector2 basePos = Vector2.Lerp(springStart, springEnd, t);
            float sineOffset = Mathf.Sin(t * waveFrequency * Mathf.PI * 2) * waveAmplitude;
            Vector2 wavePos = basePos + perp * sineOffset;
            lr.SetPosition(index++, wavePos);
        }

        // 3. Garis lurus vertikal bawah
        lr.SetPosition(index++, end);
    }

    public void SetEnabled(bool enabled)
    {
        if (lr == null) return; // Pastikan lr sudah diinisialisasi
        isEnabled = enabled;
        lr.enabled = enabled; // Mengaktifkan atau menonaktifkan komponen LineRenderer
    }

    public float GetX()
    {
        return x; // Mengembalikan panjang pegas
    }

    public float GetF()
    {
        return forceSlider.value;
    }
}
