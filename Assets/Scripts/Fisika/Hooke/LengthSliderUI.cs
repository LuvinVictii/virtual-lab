using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting; // kalau pakai TextMeshPro

public class LengthSliderUI : MonoBehaviour
{
    public Slider slider;
    public TMP_Text lengthLabel; // Ganti ke Text kalau bukan TMP
    public GameObject targetObject; // opsional: objek yang mau disesuaikan panjangnya
    public GameObject pegas;
    private float baseLength;

    void Start()
    {
        slider.onValueChanged.AddListener(UpdateLengthDisplay);
        UpdateLengthDisplay(pegas.GetComponent<HookReceiver>().deltaL); // Init di awal
        baseLength = targetObject.transform.localScale.y; // Ambil panjang awal objek
    }

    void UpdateLengthDisplay(float value)
    {
        // float value = pegas.GetComponent<HookReceiver>().deltaL; // Ambil panjang dari HookReceiver
        float temp = targetObject.transform.localScale.y - baseLength;
        value = value * 10 * (temp > 0 ? temp : -temp); // Konversi ke cm
        lengthLabel.text = value.ToString("F2") + " cm";

        // if (targetObject != null)
        // {
        //     // Contoh: ubah skala objek sesuai panjang
        //     targetObject.localScale = new Vector3(1, value / baseLength, 1);
        // }
    }
}
