using TMPro;
using UnityEngine;

public class GameUIText : MonoBehaviour
{
    [SerializeField] TMP_Text myTurn;
    [SerializeField] TMP_Text pointScore;
    [SerializeField] TMP_Text reroll;

    private void Update()
    {
        myTurn.text = ScoreManager.Instance.Turns.ToString();
        pointScore.text = ScoreManager.Instance.GetStageGoal().ToString();
        reroll.text = ScoreManager.Instance.GetReroll().ToString();
    }
}