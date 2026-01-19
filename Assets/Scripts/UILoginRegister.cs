using UnityEngine;
using TMPro;

public class UILoginRegister : MonoBehaviour
{
    [Header("Login UI")]
    [SerializeField] private TMP_InputField loginUserField;
    [SerializeField] private TMP_InputField loginPassField;
    [SerializeField] private TextMeshProUGUI loginMessage;

    [Header("Register UI")]
    [SerializeField] private TMP_InputField registerUserField;
    [SerializeField] private TMP_InputField registerPassField;
    [SerializeField] private TextMeshProUGUI registerMessage;

    private DataBase db;
    private UIManager uiManager;

    private void Awake()
    {
        db = FindObjectOfType<DataBase>();
        uiManager = FindObjectOfType<UIManager>();
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

            loginMessage.text = "";
            uiManager.ShowMain(user);
        }
        else
        {
            loginMessage.text = result.message;
        }
    }

    public void OnRegisterButton()
    {
        string user = registerUserField.text.Trim();
        string pass = registerPassField.text;

        string result = db.RegisterUser(user, pass);

        if (result == "OK")
        {
            registerMessage.text = "Usuari registrat correctament";
        }
        else
        {
            registerMessage.text = result;
        }
    }

    public void OnLogoutButton()
    {
        PlayerPrefs.DeleteKey("CurrentUserID");
        PlayerPrefs.DeleteKey("CurrentUsername");
        uiManager.ShowLogin();
    }
}
