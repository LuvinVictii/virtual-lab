using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class PegasZigZag : MonoBehaviour
{
    public Transform anchor;     
    public Transform beban;
    public int segmentCount = 50;
    public float waveAmplitude = 0.2f;
    public float waveFrequency = 5f;
    public float lineWidth = 0.2f;

    public float straightTopLength = 0.2f;     // Garis lurus atas
    public float straightBottomLength = 0.1f;  // Garis lurus sebelum hook
    public float hookRadius = 0.2f;            // Radius hook
    public int hookSegmentCount = 10;
    private bool isEnabled = true; // Untuk mengaktifkan atau menonaktifkan pegas

    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
    }

    void Update()
    {
        if (!isEnabled) return; // Jika tidak aktif, tidak menggambar pegas
        DrawPegas(anchor.position, beban.position);
    }

    void DrawPegas(Vector2 start, Vector2 end)
    {
        Vector2 direction = (end - start).normalized;
        float totalLength = Vector2.Distance(start, end);
        Vector2 perp = new Vector2(-direction.y, direction.x); 

        Vector2 straightTopEnd = start + direction * straightTopLength;
        Vector2 springStart = straightTopEnd;
        Vector2 springEnd = end - direction * (straightBottomLength + hookRadius);
        Vector2 straightBottomEnd = springEnd + direction * straightBottomLength;

        int totalPoints = 1 + segmentCount + 1 + 1 + hookSegmentCount + 1;
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
        lr.SetPosition(index++, straightBottomEnd);

        // 4. Hook (setengah lingkaran, default ke kiri)
        float angleStep = Mathf.PI / hookSegmentCount;
        for (int i = 0; i <= hookSegmentCount; i++)
        {
            float angle = Mathf.PI + angleStep * i;
            Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * hookRadius;
            Vector2 hookPos = end + offset;
            lr.SetPosition(index++, hookPos);
        }
    }

    public void SetEnabled(bool enabled)
    {
        if (lr == null) return; // Pastikan lr sudah diinisialisasi
        isEnabled = enabled;
        lr.enabled = enabled; // Mengaktifkan atau menonaktifkan komponen LineRenderer
    }
}
