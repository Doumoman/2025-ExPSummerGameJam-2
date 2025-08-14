using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUIBtn : MonoBehaviour
{
    [SerializeField] GameObject popUpUi;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI ReRollext;

    float score = 5f;
    float reRoll = 5f;
    private void Awake()
    {
        popUpUi.SetActive(false);
        score = 0;
    }

    private void Update()
    {
        //score = ScoreManager.instance.score
        //reRoll = GameManager.reRoll
        ScoreText.text = "Score: " + score.ToString();
        ReRollext.text = "ReRoll: " + reRoll.ToString();
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
       //
    }
}
