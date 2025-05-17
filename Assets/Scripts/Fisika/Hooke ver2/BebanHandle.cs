using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BebanHandle : MonoBehaviour
{
    public Transform anchor;
    public Slider forceSlider;
    public Slider kSlider;

    private Camera cam;
    private bool isDragging = false;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;
    }

    void OnMouseUp()
    {
        isDragging = false;
    }

    void OnMouseDrag()
    {
        if (!isDragging) return;

        Vector3 screenPos = Input.mousePosition;
        screenPos.z = Mathf.Abs(cam.transform.position.z - transform.position.z);
        Vector3 mouseWorld = cam.ScreenToWorldPoint(screenPos);
        mouseWorld.z = 0f;

        transform.position = new Vector3(mouseWorld.x, transform.position.y, 0f);

        float x = transform.position.x - anchor.position.x;
        float k = Mathf.Max(kSlider.value, 0.01f);
        float F = x * k;
        forceSlider.value = F;
    }
}