using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DisableNilai : MonoBehaviour
{
    public List<Toggle> toggles;
    public Toggle nilaiToggle;
    // public List<TMP_Text> texts;

    public void Check()
    {
        // texts[idx].gameObject.SetActive(toggles[idx].isOn);
        bool value = true;
        foreach (Toggle toggle in toggles)
        {
            if (toggle.isOn)
            {
                value = false;
                break;
            }
        }
        // nilaiToggle.isOn = value;
        gameObject.SetActive(value);
    }
}
