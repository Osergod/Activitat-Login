using UnityEngine;
using TMPro;
using UnityEngine.UI; // <- Necessari per al Button

public class UILoginRegister : MonoBehaviour
{
    [Header("Login UI")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private TMP_InputField loginUserField;
    [SerializeField] private TMP_InputField loginPassField;

    [Header("Register UI")]
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private TMP_InputField registerUserField;
    [SerializeField] private TMP_InputField registerPassField;

    [Header("Popup")]
    [SerializeField] private GameObject popupPanel;
    [SerializeField] private TextMeshProUGUI popupText;
    [SerializeField] private Button popupAcceptButton; // Botó d'acceptar

    private DataBase db;
    private UIManager uiManager;
    GameObject lastPanel;

    private void Awake()
    {
        db = FindObjectOfType<DataBase>();
        uiManager = FindObjectOfType<UIManager>();
        popupPanel.SetActive(false);

        // Assignar la funció al botó
        popupAcceptButton.onClick.AddListener(ClosePopup);
    }

    public void OnLoginButton()
    {
        string user = loginUserField.text.Trim();
        string pass = loginPassField.text;

        var result = db.LoginUser(user, pass);

        if (result.success)
        {
            PlayerPrefs.SetInt("CurrentUserID", result.userId);
            PlayerPrefs.SetString("CurrentUsername", user);
            PlayerPrefs.Save();

            uiManager.ShowMain(user);
        }
        else
        {
            ShowPopup(result.message);
        }
    }

    public void OnRegisterButton()
    {
        string user = registerUserField.text.Trim();
        string pass = registerPassField.text;

        string result = db.RegisterUser(user, pass);

        if (result == "OK")
        {
            ShowPopup("Usuari registrat correctament");
        }
        else
        {
            ShowPopup(result);
        }
    }

    public void OnLogoutButton()
    {
        PlayerPrefs.DeleteKey("CurrentUserID");
        PlayerPrefs.DeleteKey("CurrentUsername");
        uiManager.ShowLogin();
    }

    public void ClosePopup()
    {
        popupPanel.SetActive(false);
        if (lastPanel != null) lastPanel.SetActive(true);
    }

    private void ShowPopup(string message)
    {
        popupText.text = message;

        if (loginPanel.activeSelf) lastPanel = loginPanel;
        if (registerPanel.activeSelf) lastPanel = registerPanel;

        lastPanel.SetActive(false);
        popupPanel.SetActive(true);
    }
}
