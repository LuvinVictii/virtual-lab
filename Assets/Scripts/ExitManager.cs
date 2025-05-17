using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitManager : MonoBehaviour
{
    [SerializeField] private GameObject darkBackground;
    [SerializeField] private GameObject popUpExit;
    [SerializeField] private GameObject popUpExitBg;

    public void PopUpExitEnable(){
        darkBackground.SetActive(true);
        popUpExit.SetActive(true);
        popUpExitBg.SetActive(true);
    }
    public void PopUpExitDisable(){
        darkBackground.SetActive(false);
        popUpExit.SetActive(false);
        popUpExitBg.SetActive(false);
    }
    public void ExitApp(){
        Application.Quit();
        Debug.Log("Exit App");
    }
}
