using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ButtonSelector : MonoBehaviour
{
    public List<Button> buttons;
    public List<GameObject> buttonOn;
    public List<float> values;
    // public Color selectedColor = Color.yellow;
    // public Color normalColor = Color.white;
    public GameObject jawabButton;

    public float selectedValue { get; private set; }

    private int selectedIndex = -1;

    void Start()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            int index = i; // Local copy for closure
            buttons[i].onClick.AddListener(() => Select(index));
            buttonOn[i].SetActive(false); // Set all buttonOn to inactive initially
        }

        if (buttons.Count > 0)
        {
            Select(0); // Default pilih tombol pertama
        }
    }

    void Select(int index)
    {
        selectedIndex = index;
        selectedValue = values[index];

        for (int i = 0; i < buttons.Count; i++)
        {
            buttonOn[i].SetActive(i == index);
        }

        if (index == buttons.Count - 1)
        {
            jawabButton.SetActive(true); // Show jawab button if last button is selected
        }
        else
        {
            jawabButton.SetActive(false); // Hide jawab button otherwise
        }
        
        Debug.Log("Selected value: " + selectedValue);
    }
}
