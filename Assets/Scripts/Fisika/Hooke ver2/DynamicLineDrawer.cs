using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class DynamicLineDrawer : MonoBehaviour
{
    public RectTransform anchorUI;      // Titik awal garis (dari UI RectTransform)
    public GameObject springSource;         // Komponen Spring sebagai sumber nilai x
    public float lineWidth = 0.1f;        // Lebar garis
    public float yOffset = 0f;          // Offset vertikal dari anchor
    public float zPosition = 0f;        // Z untuk posisi line (biar kelihatan)
    public float markerLength = 0.2f;   // Panjang garis marker
    public float arrowMultiplier = 1.1f;
    public bool isArrow = false; // Apakah garis ini panah
    public bool isLawanArah = false;
    public bool isGaya = false;
    public int springIdx = 1;
    public bool isMentok = false;
    private Toggle isNilai;
    [SerializeField] private bool isEnabled = false;
    public TMP_Text xText;

    private LineRenderer lr;
    private Camera cam;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        cam = Camera.main;
        lr.enabled = isEnabled;

        lr.positionCount = 5;
        lr.useWorldSpace = true;
        lr.startWidth = lineWidth;
        lr.endWidth = lineWidth;
    }

    void Start()
    {
        isNilai = GameObject.Find("ToggleNilai").GetComponent<Toggle>();
    }

    void Update()
    {
        if (!isEnabled) return; // Jika tidak aktif, keluar dari fungsi
        // Pastikan ada referensi yang valid
        if (anchorUI == null || springSource == null) return;

        // Ambil posisi anchor dalam world space
        Vector3 anchorWorldPos = anchorUI.position + new Vector3(0f, yOffset, zPosition);

        float xWorldLength;
        // Ambil nilai x dari spring dan konversi ke panjang garis
        if (springSource.GetComponent<Spring>() != null)
            xWorldLength = springSource.GetComponent<Spring>().GetX() * springSource.GetComponent<Spring>().xAdjustment;
        else if (springSource.GetComponent<ParallelSpring>() != null)
            xWorldLength = springSource.GetComponent<ParallelSpring>().GetX(springIdx) * springSource.GetComponent<ParallelSpring>().xAdjustment;
        else
        {
            if (!isArrow)
                xWorldLength = springSource.GetComponent<SeriesSpring>().GetX() * springSource.GetComponent<SeriesSpring>().xAdjustment;
            else
                xWorldLength = springSource.GetComponent<SeriesSpring>().GetF() * springSource.GetComponent<SeriesSpring>().xAdjustment / 100f;
        }
        Vector3 endPos = anchorWorldPos + new Vector3(xWorldLength, 0f, 0f);

        if (isArrow && springSource.GetComponent<SeriesSpring>() == null){
            // endPos.x *= arrowMultiplier;
            endPos.x *= 1.1f;
        }

        anchorWorldPos.z = zPosition; // Set Z untuk anchor
        endPos.z = zPosition;         // Set Z untuk endPos

        if (isLawanArah)
        {
            endPos.x = anchorWorldPos.x - (endPos.x - anchorWorldPos.x);
        }

        lr.SetPosition(0, anchorWorldPos);
        lr.SetPosition(1, endPos);

        if (anchorWorldPos.x != endPos.x || anchorWorldPos.x != endPos.x / 1.1f)
        {
            float arrowXOffset;
            if (!isArrow)
            {
                arrowXOffset = 0f;
            }
            else
            {
                arrowXOffset = markerLength * 0.5f; // Set offset untuk panah
                if (endPos.x - anchorWorldPos.x >0){
                    arrowXOffset = Mathf.Min(arrowXOffset, endPos.x - anchorWorldPos.x); // Pastikan offset tidak lebih kecil dari panjang garis
                }
                else
                {
                    arrowXOffset = Mathf.Max(-arrowXOffset, endPos.x - anchorWorldPos.x); // Pastikan offset tidak lebih besar dari panjang garis
                }
                // arrowXOffset = (endPos.x - anchorWorldPos.x > 0) ? arrowXOffset : -arrowXOffset; // Sesuaikan arah panah
                // arrowXOffset = Mathf.Min(arrowXOffset, endPos.x - anchorWorldPos.x); // Pastikan offset tidak lebih besar dari panjang garis
            }
            // Garis pendek tegak lurus ke atas dari ujung garis
            Vector3 markerStart = endPos - new Vector3(arrowXOffset, markerLength, 0f);
            Vector3 markerEnd = endPos + new Vector3(-arrowXOffset, markerLength, 0f);

            lr.SetPosition(2, markerStart);
            lr.SetPosition(3, endPos);
            lr.SetPosition(4, markerEnd);

        }
        else
        {
            lr.SetPosition(2, endPos);
            lr.SetPosition(3, endPos);
            lr.SetPosition(4, endPos);
        }
        // Update Posisi Text
        if (xText != null)
        {
            
            if (isGaya){
                // xText.transform.position = endPos + new Vector3(((endPos.x > anchorWorldPos.x) ? 1f : -1f) - (endPos.x - anchorWorldPos.x)/2, 0.5f, 0f);
                if(springSource.GetComponent<Spring>() != null){
                    xText.transform.position = endPos + new Vector3(((endPos.x > anchorWorldPos.x) ? 2f : -2.2f) * 0.5f, 0f, 0f);
                    xText.text = $"{springSource.GetComponent<Spring>().GetF():0.00} N"; // Update text dengan nilai gaya
                }
                else if (springSource.GetComponent<ParallelSpring>() != null){
                    xText.transform.position = endPos + new Vector3(((endPos.x > anchorWorldPos.x) ? 2f : -2.2f) * 0.5f, 0f, 0f);
                    xText.text = $"{springSource.GetComponent<ParallelSpring>().GetF(springIdx):0.00} N"; // Update text dengan nilai gaya
                }// xText.text = $"{springSource.GetF():0.00} N"; // Update text dengan nilai gaya
                else{
                    xText.transform.position = endPos + new Vector3(((endPos.x > anchorWorldPos.x) ? 2f : -2.2f) * 0.5f, 0f, 0f);
                    if (springSource.GetComponent<SeriesSpring>().GetX() > 0.7 && isMentok)
                    {
                        xText.transform.position = endPos + new Vector3(0f, -0.5f, 0f);
                        // Debug.Log("X = " + springSource.GetComponent<SeriesSpring>().GetX());
                    }
                    xText.text = $"{springSource.GetComponent<SeriesSpring>().GetF():0.00} N"; // Update text dengan nilai gaya
                }
            }
            else
            {    
                xText.transform.position = (endPos - anchorWorldPos)/2 + new Vector3(0f, -0.5f, 0f); // Offset sedikit ke atas
                if(springSource.GetComponent<Spring>() != null)
                    xText.text = $"{springSource.GetComponent<Spring>().GetX():0.000} m"; // Update text dengan nilai x
                else if (springSource.GetComponent<ParallelSpring>() != null)
                    xText.text = $"{springSource.GetComponent<ParallelSpring>().GetX(springIdx):0.000} m"; // Update text dengan nilai x
                else
                {
                    xText.transform.position = endPos + new Vector3(((endPos.x > anchorWorldPos.x) ? 1f : -1f), 0f, 0f);
                    xText.text = $"{springSource.GetComponent<SeriesSpring>().GetX():0.000} m"; // Update text dengan nilai x
                }
                // xText.text = $"{springSource.GetX():0.000} m"; // Update text dengan nilai x
            }
        }
    }
    public void SetVisible()
    {
        isEnabled = !isEnabled;
        lr.enabled = isEnabled;
        if (xText != null && isNilai != null)
        {
            xText.gameObject.SetActive(isEnabled && isNilai.isOn);
        }
        else if (xText != null)
        {
            xText.gameObject.SetActive(isEnabled);
        }
    }
}
