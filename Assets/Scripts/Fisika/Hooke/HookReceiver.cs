using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class HookReceiver : MonoBehaviour, IDropHandler
{
    public Transform anchor;
    public bool isHooked = false;
    public float baseLength;
    public float k = 50f;
    public float damping = 0.1f; // Untuk meredam bouncing
    public float bounceDuration = 10f; // Lama total bouncing
    public GameObject jawabanText;
    [SerializeField] private float randBot = 1f;
    [SerializeField] private float randTop = 2.5f;
    public float deltaL { get; private set; } // Perpanjangan pegas

    private Vector3 initialHookPosition;
    private Coroutine bounceRoutine;

    private void Awake()
    {
        baseLength = Vector3.Distance(anchor.position, transform.position);
        initialHookPosition = transform.position;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if(isHooked)
        {
            return; // Jika sudah terhubung, tidak perlu melakukan apa-apa
        }
        if (eventData.pointerDrag != null)
        {
            RectTransform dragged = eventData.pointerDrag.GetComponent<RectTransform>();
            MassInfo massInfo = dragged.GetComponent<MassInfo>();
            RectTransform hook = GetComponent<RectTransform>();

            // Jadikan beban child dari hook
            // dragged.SetParent(hook);
            float bebanHeight = dragged.rect.height;
            dragged.localPosition = new Vector3(0, -bebanHeight / 2, 0);
            // dragged.localPosition = Vector3.zero;

            if (massInfo != null)
            {
                // isHooked = true;
                float m = massInfo.mass;
                float g = 9.81f;
                float F = m * g;
                deltaL = F / k;
                Debug.Log("F: " + F + " deltaL: " + deltaL);
                deltaL *= 0.5f;

                float targetLength = baseLength + deltaL + Random.Range(-0.005f, 0.005f);
                Vector3 targetPosition = anchor.position - new Vector3(0, targetLength, 0);

                if (bounceRoutine != null)
                    StopCoroutine(bounceRoutine);

                bounceRoutine = StartCoroutine(BounceToPosition(targetPosition));
            }
        }
    }

    private IEnumerator BounceToPosition(Vector3 targetPos)
    {
        float elapsed = 0f;
        Vector3 startPos = transform.position;
        float amplitude = (targetPos - startPos).magnitude;
        float frequency = 20f; // bisa disesuaikan

        while (true)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / bounceDuration;

            // Fungsi sinusoidal dengan peredaman
            float offset = amplitude * Mathf.Exp(-damping * t * frequency) * Mathf.Cos(frequency * t * Mathf.PI * 2);
            transform.position = targetPos + new Vector3(0, offset, 0);

            if (offset < 0.001f && offset > -0.001f)
            {
                break; // Berhenti jika sudah cukup dekat
            }

            yield return null;
        }

        transform.position = targetPos; // Pastikan berhenti di posisi akhir
    }

    public void ResetHook()
    {
        if (bounceRoutine != null)
            StopCoroutine(bounceRoutine);

        transform.position = initialHookPosition;

        isHooked = false;
        deltaL = 0f; // Reset perpanjangan pegas
    }

    public void SetVariable(float value)
    {
        k = value;
    }

    public void RandomK()
    {
        k = Random.Range(randBot, randTop);
        Debug.Log("K: " + k);
        jawabanText.GetComponent<TMPro.TextMeshProUGUI>().text = (k*10).ToString("F2");
    }
}