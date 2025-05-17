using TMPro;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DeltaLIndicator : MonoBehaviour
{
    public GameObject anchor;     
    public GameObject beban;

    public TMP_Text deltaLText; // TextMeshPro untuk menampilkan deltaL
    public float lineWidth = 0.2f;
    private LineRenderer lr;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
    }

    void Update()
    {
        float dist = Vector2.Distance(anchor.transform.position, beban.transform.position);
        deltaLText.text = (dist*20).ToString("F2") + " cm"; // Update deltaL text
        if (dist < 0.01f){
            lr.enabled = false; // Disable line renderer if the distance is too small
            anchor.SetActive(false);
            beban.SetActive(false);
            return;
        }
        else if(!anchor.activeSelf && !beban.activeSelf && dist > 0.01f){
            lr.enabled = true;
            anchor.SetActive(true);
            beban.SetActive(true);
        }
        DrawPegas(anchor.transform.position, beban.transform.position);
    }

    void DrawPegas(Vector2 start, Vector2 end)
    {
        lr.positionCount = 2;
        lr.SetPosition(0, start); // Set posisi awal garis lurus atas
        lr.SetPosition(1, end);   // Set posisi akhir garis lurus bawah
    }
}
