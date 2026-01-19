using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class UILoginRegister : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private TMP_InputField usernameField;
    [SerializeField] private TMP_InputField passwordField;
    [SerializeField] private Button loginButton;
    [SerializeField] private Button registerButton;
    [SerializeField] private TextMeshProUGUI messageText;

    [Header("Config")]
    [SerializeField] private float sceneLoadDelay = 1.2f;
    [SerializeField] private string mainMenuScene = "MainMenu";

    private DataBase dbManager;

    private const string PREF_USER_ID = "CurrentUserID";
    private const string PREF_USERNAME = "CurrentUsername";

    private void Awake()
    {
        dbManager = FindObjectOfType<DataBase>();

        if (dbManager == null)
        {
            Debug.LogError("DataBase not found in scene");
            enabled = false;
        }
    }

    private void OnEnable()
    {
        loginButton.onClick.AddListener(OnLogin);
        registerButton.onClick.AddListener(OnRegister);
        ClearMessage();
    }

    private void OnDisable()
    {
        loginButton.onClick.RemoveListener(OnLogin);
        registerButton.onClick.RemoveListener(OnRegister);
    }

    private void OnRegister()
    {
        if (!ValidateInput(out string user, out string pass))
            return;

        SetButtonsInteractable(false);

        string result = dbManager.RegisterUser(user, pass);

        if (result == "OK")
            ShowMessage("Register completed. You can now log in.", Color.green);
        else
            ShowMessage(result, Color.red);

        SetButtonsInteractable(true);
    }

    private void OnLogin()
    {
        if (!ValidateInput(out string user, out string pass))
            return;

        SetButtonsInteractable(false);

        var (success, message, userId) = dbManager.LoginUser(user, pass);

        if (success)
        {
            SaveUserSession(userId, user);
            ShowMessage("Login successful.", Color.green);
            Invoke(nameof(LoadMainScene), sceneLoadDelay);
        }
        else
        {
            ShowMessage(message, Color.red);
            SetButtonsInteractable(true);
        }
    }

    private bool ValidateInput(out string user, out string pass)
    {
        user = usernameField.text.Trim();
        pass = passwordField.text;

        if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(pass))
        {
            ShowMessage("Username and password are required.", Color.red);
            return false;
        }

        if (pass.Length < 4)
        {
            ShowMessage("Password must be at least 4 characters.", Color.red);
            return false;
        }

        return true;
    }

    private void SaveUserSession(int userId, string username)
    {
        PlayerPrefs.SetInt(PREF_USER_ID, userId);
        PlayerPrefs.SetString(PREF_USERNAME, username);
        PlayerPrefs.Save();
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene(mainMenuScene);
    }

    private void SetButtonsInteractable(bool value)
    {
        loginButton.interactable = value;
        registerButton.interactable = value;
    }

    private void ShowMessage(string msg, Color color)
    {
        if (messageText == null)
        {
            Debug.Log(msg);
            return;
        }

        messageText.text = msg;
        messageText.color = color;
    }

    private void ClearMessage()
    {
        if (messageText != null)
            messageText.text = string.Empty;
    }
}
