using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class ArrowPlacer : MonoBehaviour
{
    public RectTransform anchorUI;      // Titik awal garis (dari UI RectTransform)
    public RectTransform anchorWorldPos; // Titik awal garis (dari UI RectTransform)
    public RectTransform pegas;
    public RectTransform gayaDiberikan;
    public GameObject springSource;

    void Awake()
    {
        
    }

    void Update()
    {
        float springLength;
        if(springSource.GetComponent<Spring>() != null)
            springLength = springSource.GetComponent<Spring>().GetX() * springSource.GetComponent<Spring>().xAdjustment;
        else if (springSource.GetComponent<ParallelSpring>() != null)
            springLength = springSource.GetComponent<ParallelSpring>().GetX(3) * springSource.GetComponent<ParallelSpring>().xAdjustment;
        else
            springLength = springSource.GetComponent<SeriesSpring>().GetX() * springSource.GetComponent<SeriesSpring>().xAdjustment;
        // float springLength = springSource.GetComponent<Spring>().GetX() * springSource.xAdjustment;
        // springLength = springLength;
        // float gayaLength = springLength*1000 * springLength*1000 / 1000;
        // gayaLength = (springLength > 0)? gayaLength : -gayaLength;
        float gayaLength = springLength * 0.75f;

        if (pegas != null && gayaDiberikan != null)
        {
            anchorUI.position = new Vector3(anchorWorldPos.position.x + springLength, anchorUI.position.y, 0);
            pegas.position = new Vector3(anchorUI.position.x - gayaLength, pegas.position.y, 0);
            gayaDiberikan.position = new Vector3(anchorUI.position.x + gayaLength, gayaDiberikan.position.y, 0);
        }
    }
}
