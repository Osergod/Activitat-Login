using UnityEngine;
using TMPro;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject loginPanel;
    [SerializeField] private GameObject registerPanel;
    [SerializeField] private GameObject mainPanel;

    [Header("Main Panel")]
    [SerializeField] private TextMeshProUGUI welcomeText;

    [Header("Animation Settings")]
    [SerializeField] private float animationDuration = 0.4f;
    [SerializeField] private Ease animationEase = Ease.OutBack;

    private void Start()
    {
        ShowLoginInstant();
    }

    #region Public Methods (UI)

    public void ShowLogin()
    {
        ChangePanel(loginPanel);
    }

    public void ShowRegister()
    {
        ChangePanel(registerPanel);
    }

    public void ShowMain(string username)
    {
        welcomeText.text = "Usuari connectat: " + username;
        ChangePanel(mainPanel);
    }

    #endregion

    #region Private Methods

    private void ShowLoginInstant()
    {
        loginPanel.SetActive(true);
        loginPanel.transform.localScale = Vector3.one;

        registerPanel.SetActive(false);
        mainPanel.SetActive(false);
    }
    private void ChangePanel(GameObject nextPanel)
    {
        GameObject currentPanel = null;
        if (loginPanel.activeSelf) currentPanel = loginPanel;
        else if (registerPanel.activeSelf) currentPanel = registerPanel;
        else if (mainPanel.activeSelf) currentPanel = mainPanel;

        if (currentPanel == null)
        {
            nextPanel.SetActive(true);
            nextPanel.transform.localScale = Vector3.zero;
            nextPanel.transform.DOScale(Vector3.one, animationDuration).SetEase(animationEase);
            return;
        }

        nextPanel.SetActive(true);
        nextPanel.transform.localScale = Vector3.zero;

        currentPanel.transform.DOScale(Vector3.zero, animationDuration)
            .SetEase(animationEase)
            .OnComplete(() =>
            {
                currentPanel.SetActive(false);

                nextPanel.transform.DOScale(Vector3.one, animationDuration)
                    .SetEase(animationEase);
            });
    }

    #endregion
}
