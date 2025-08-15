using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SrartUiBtn : MonoBehaviour
{
    [SerializeField] GameObject SettingPanel;
    [SerializeField] GameObject CreditPanel;

    bool panelIsOn = false;

    private void Awake()
    {
        SettingPanel.SetActive(false);
        CreditPanel.SetActive(false);
    }

    public void NewStartBtn()
    {
        GameManager.Inst.isReset = true;
        StageManager.Instance.LoadGameScene();
    }
    public void LoadStartBtn()
    {
        GameManager.Inst.isReset = false;
        StageManager.Instance.LoadGameScene();
    }

    public void SettingBtn()
    {
        SettingPanel.SetActive(true);
        panelIsOn = true;
    }

    public void RestartBtn()
    {
        //load
    }

    public void ClosePanel()
    {
        SettingPanel.SetActive(false);
        CreditPanel.SetActive(false);
        panelIsOn = false;
    }
    public void QuitBtn()
    {
        Application.Quit();
    }
}
