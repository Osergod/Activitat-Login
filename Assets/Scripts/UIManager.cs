using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject mainPanel;

    [Header("Main Panel")]
    [SerializeField] private TextMeshProUGUI welcomeText;

    private void Start()
    {
        ShowLogin();
    }

    public void ShowLogin()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
        mainPanel.SetActive(false);
    }

    public void ShowRegister()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
        mainPanel.SetActive(false);
    }

    public void ShowMain(string username)
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(false);
        mainPanel.SetActive(true);

        welcomeText.text = "Usuari connectat: " + username;
    }
}
