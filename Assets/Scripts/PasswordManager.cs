using UnityEngine;
using UnityEngine.UI;
using System.Security.Cryptography;
using System.Text;

public class PasswordManager : MonoBehaviour
{
    public GameObject loginPanel;
    public TMPro.TMP_InputField passwordInput;
    public Button submitButton;

    private const string CORRECT_HASH = "74456497a36426e5ab258434e6ededd6f6a631a52e7c922e6ca11dafdcfb0c98";
    private const string PREF_KEY = "AppUnlocked";

    void Start()
    {
        // PlayerPrefs.DeleteKey("AppUnlocked");
        if (PlayerPrefs.GetInt(PREF_KEY, 0) == 1)
        {
            // Sudah di-unlock sebelumnya
            loginPanel.SetActive(false);
        }
        else
        {
            // Masih ke-lock
            loginPanel.SetActive(true);
            submitButton.onClick.AddListener(CheckPassword);
        }
    }

    public void CheckPassword()
    {
        string input = passwordInput.text;
        string hashedInput = Hash(input);

        if (hashedInput == CORRECT_HASH)
        {
            Debug.Log("Password benar!");
            PlayerPrefs.SetInt(PREF_KEY, 1);
            PlayerPrefs.Save();

            loginPanel.SetActive(false);
        }
        else
        {
            passwordInput.text = string.Empty; // Kosongkan input field
            Debug.Log("Password salah.");
        }
    }

    string Hash(string input)
    {
        using (SHA256 sha = SHA256.Create())
        {
            byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in bytes)
                builder.Append(b.ToString("x2"));
            return builder.ToString();
        }
    }
}
