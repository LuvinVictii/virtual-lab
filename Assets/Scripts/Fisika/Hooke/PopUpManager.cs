using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    [SerializeField] private GameObject darkBackground;
    [SerializeField] private GameObject popUpJawaban;
    [SerializeField] private GameObject popUpJawabanBg;
    [SerializeField] private GameObject popUpHasil;
    [SerializeField] private GameObject popUpHasilBg;
    [SerializeField] private GameObject popUpExit;
    [SerializeField] private GameObject popUpExitBg;
    [SerializeField] private List<GameObject> popUpTutorial;
    private int idx = 0;
    [SerializeField] private GameObject popUpTutorialBg;
    [SerializeField] private GameObject popUpCustomMass;
    [SerializeField] private GameObject popUpCustomMassBg;
    [SerializeField] private GameObject CustomMassText;
    [SerializeField] private GameObject CustomMass;
    [SerializeField] private TMPro.TMP_InputField CustomMassInputField;
    [SerializeField] private GameObject ansUserText;
    [SerializeField] private GameObject ansText;
    [SerializeField] private GameObject pegasLine;
    [SerializeField] public float ans {get; private set; } // Jawaban yang benar;
    [SerializeField] public float ansUser {get; private set; } // Jawaban user;
    [SerializeField] private TMPro.TMP_InputField inputField;


    void Start()
    {
        ansUser = -1f;
        PopUpTutorialEnable();
    }

    public void PopUpJawabanEnable(){
        pegasLine.GetComponent<PegasZigZag>().SetEnabled(false);
        darkBackground.SetActive(true);
        popUpJawaban.SetActive(true);
        popUpJawabanBg.SetActive(true);
    }
    public void PopUpJawabanDisable(){
        pegasLine.GetComponent<PegasZigZag>().SetEnabled(true);
        darkBackground.SetActive(false);
        popUpJawaban.SetActive(false);
        popUpJawabanBg.SetActive(false);
    }
    public void PopUpHasilEnable(){
        pegasLine.GetComponent<PegasZigZag>().SetEnabled(false);
        darkBackground.SetActive(true);
        popUpHasil.SetActive(true);
        popUpHasilBg.SetActive(true);
    }
    public void PopUpHasilDisable(){
        pegasLine.GetComponent<PegasZigZag>().SetEnabled(true);
        darkBackground.SetActive(false);
        popUpHasil.SetActive(false);
        popUpHasilBg.SetActive(false);
    }

    public void PopUpExitEnable(){
        pegasLine.GetComponent<PegasZigZag>().SetEnabled(false);
        darkBackground.SetActive(true);
        popUpExit.SetActive(true);
        popUpExitBg.SetActive(true);
    }
    public void PopUpExitDisable(){
        pegasLine.GetComponent<PegasZigZag>().SetEnabled(true);
        darkBackground.SetActive(false);
        popUpExit.SetActive(false);
        popUpExitBg.SetActive(false);
    }

    public void PopUpTutorialEnable(){
        pegasLine.GetComponent<PegasZigZag>().SetEnabled(false);
        darkBackground.SetActive(true);

        idx = 0;
        popUpTutorial[0].SetActive(true);
        popUpTutorialBg.SetActive(true);
    }

    public void PopUpTutorialNext(){
        popUpTutorial[idx].SetActive(false);
        idx++;
        if(idx < popUpTutorial.Count){
            popUpTutorial[idx].SetActive(true);
        }else{
            PopUpTutorialDisable();
        }
    }

    public void PopUpTutorialPrev(){
        popUpTutorial[idx].SetActive(false);
        idx--;
        if(idx >= 0){
            popUpTutorial[idx].SetActive(true);
        }else{
            // PopUpTutorialDisable();
            return;
        }
    }

    public void PopUpTutorialDisable(){
        pegasLine.GetComponent<PegasZigZag>().SetEnabled(true);
        darkBackground.SetActive(false);
        popUpTutorial[idx].SetActive(false);
        popUpTutorialBg.SetActive(false);
    }

    public void PopUpCustomMassEnable(){
        pegasLine.GetComponent<PegasZigZag>().SetEnabled(false);
        darkBackground.SetActive(true);
        popUpCustomMass.SetActive(true);
        popUpCustomMassBg.SetActive(true);
    }

    public void PopUpCustomMassDisable(){
        pegasLine.GetComponent<PegasZigZag>().SetEnabled(true);
        darkBackground.SetActive(false);
        popUpCustomMass.SetActive(false);
        popUpCustomMassBg.SetActive(false);
    }

    public void SetCustomMass(){
        if (string.IsNullOrEmpty(CustomMassInputField.text))
        {
            CustomMassInputField.text = string.Empty; // Kosongkan input field
            Debug.Log("Input kosong!");
            return;
        }

        float parsedValue;
        if (!float.TryParse(CustomMassInputField.text, out parsedValue))
        {
            CustomMassInputField.text = string.Empty; // Kosongkan input field
            Debug.Log("Input bukan angka!");
            return;
        }

        if(parsedValue < 10f || parsedValue > 300f)
        {
            CustomMassInputField.text = string.Empty; // Kosongkan input field
            Debug.Log("Input tidak valid!");
            return;
        }

        CustomMass.GetComponent<MassInfo>().SetVariable(parsedValue / 1000f);
        CustomMassText.GetComponent<TMPro.TextMeshProUGUI>().text = parsedValue.ToString() + "g";
        PopUpCustomMassDisable();
    }

    public void UserJawab(float ansUser){
        this.ansUser = ansUser;
    }

    public void SetJawaban(float ans){
        this.ans = ans;
    }

    public void SubmitJawaban()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            inputField.text = string.Empty; // Kosongkan input field
            Debug.Log("Input kosong!");
            return;
        }

        float parsedValue;
        if (!float.TryParse(inputField.text, out parsedValue))
        {
            inputField.text = string.Empty; // Kosongkan input field
            Debug.Log("Input bukan angka!");
            return;
        }

        ansUser = parsedValue;
        PopUpJawabanDisable();
        PopUpHasilEnable();
        ansUserText.GetComponent<TMPro.TextMeshProUGUI>().text = ansUser.ToString();
        // ansText.GetComponent<UnityEngine.UI.Text>().text = ans.ToString();
    }

    public void SceneExit()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
        Debug.Log("Keluar dari aplikasi");
    }    
}
