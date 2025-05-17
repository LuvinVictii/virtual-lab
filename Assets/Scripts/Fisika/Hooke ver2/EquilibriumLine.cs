using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class EquilibriumLine : MonoBehaviour
{
    public Transform from;
    public Transform to;

    private LineRenderer lr;

    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2;
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;

        lr.enabled = false; // Awalnya tidak aktif
    }

    void Update()
    {
        if (!lr.enabled || from == null || to == null) return;

        Vector3 p1 = from.position;
        Vector3 p2 = to.position;

        // Set Z ke 0 biar di layer yang sama
        p1.z = 0;
        p2.z = 0;

        lr.SetPosition(0, p1);
        lr.SetPosition(1, p2);
    }

    public void SetVisible()
    {
        lr.enabled = !lr.enabled;
    }
}
