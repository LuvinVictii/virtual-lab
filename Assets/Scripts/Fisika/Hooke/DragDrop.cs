using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private Transform originalParent;
    public Transform hookParent;
    private HookReceiver hookReceiver;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        originalParent = transform.parent;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Transform currParent = transform.parent;
        hookReceiver = currParent.GetComponentInParent<HookReceiver>();

        canvasGroup.blocksRaycasts = false;
        // canvasGroup.alpha = 0.6f;

        transform.SetParent(canvas.transform); // Taruh di atas UI
        // // Kalau gak dijatuhkan ke tempat lain
        if (transform.parent == canvas.transform && hookReceiver != null)
        {
            Debug.Log("HookReceiver tidak null, reset hook");
            hookReceiver.ResetHook();
        }
        transform.SetParent(canvas.transform); // Taruh di atas UI
        transform.SetAsLastSibling(); // Taruh di paling atas

    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        // transform.SetParent(parentAfterDrag);
        bool droppedOnValid = false;

        if (eventData.pointerEnter != null)
        {
            if (eventData.pointerEnter.GetComponentInParent<HookReceiver>() != null){
                if(eventData.pointerEnter.GetComponentInParent<HookReceiver>().isHooked == false){
                    droppedOnValid = true;
                    eventData.pointerEnter.GetComponentInParent<HookReceiver>().isHooked = true;
                }
            }
        }

        Debug.Log("Dropped on: " + eventData.pointerEnter.name);

        Debug.Log("Dropped on valid: " + droppedOnValid);

        transform.SetParent(droppedOnValid ? hookParent : originalParent);
        transform.localPosition = Vector3.zero; // Reset posisi setelah drag
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Clicked on: " + gameObject.name);
    }
}