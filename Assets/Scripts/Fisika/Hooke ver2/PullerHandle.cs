using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class PullerHandle : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    public Slider forceSlider;
    public Slider kSlider;
    public GameObject spring;
    private float xAdjustment;
    private RectTransform rectTransform;
    private Transform origin;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        origin = new GameObject("Origin").transform; // Buat objek kosong untuk origin
        origin.position = transform.position; // Set posisi origin ke posisi awal handle
        xAdjustment = spring.GetComponent<Spring>().xAdjustment; // Ambil nilai xAdjustment dari komponen Spring
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Pointer Down on PullerHandle");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Begin Drag on PullerHandle");
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging PullerHandle");
        rectTransform.anchoredPosition += new Vector2(eventData.delta.x, 0f) / canvas.scaleFactor;
        // rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;

        float x = (origin.position.x - rectTransform.anchoredPosition.x) / xAdjustment;

        float k = Mathf.Max(kSlider.value, 0.01f);
        float F = x * k;
        float Fclamped = Mathf.Clamp(F, forceSlider.minValue, forceSlider.maxValue);
        forceSlider.SetValueWithoutNotify(Fclamped);

        // 5. Jika ingin, kembalikan handle ke posisi yang sesuai Fclamped
        float xClamped = Fclamped / k;
        Vector2 originPos = origin.position;
        rectTransform.anchoredPosition = originPos + Vector2.right * xClamped;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("End Drag on PullerHandle");
    }
}