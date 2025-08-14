using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIBtn : MonoBehaviour
{
    [SerializeField] GameObject popUpUi;

    float score = 5f;
    float reRoll = 5f;
    private void Awake()
    {
        popUpUi.SetActive(false);
        score = 0;
    }

    private void Update()
    {

    }

    public void PopupUi()
    {
        popUpUi.SetActive(true);
    }

    public void Close()
    {
        popUpUi.SetActive(false);
    }

    public void GoToMenu()
    {
        //LoadStartScene
    }

    public void ReRoll()
    {
        
    }
}
