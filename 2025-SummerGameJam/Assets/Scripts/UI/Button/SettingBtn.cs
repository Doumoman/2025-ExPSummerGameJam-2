using UnityEngine;

public class SettingBtn : MonoBehaviour
{
    [SerializeField] GameObject settingPopup;
    public void OnclickSettingButton()
    {
        settingPopup.SetActive(true);
    }
}
