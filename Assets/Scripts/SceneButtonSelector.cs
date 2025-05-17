using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneButtonSelector : MonoBehaviour
{
    
    public void OnButtonClick(string sceneName)
    {
        // Load the specified scene
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}
